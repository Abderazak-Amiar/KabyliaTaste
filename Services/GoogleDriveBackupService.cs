using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using KabyliaTaste.Models;
using Microsoft.Data.Sqlite;
using System.Threading;

namespace KabyliaTaste.Services
{
    public sealed class GoogleDriveBackupService
    {
        private const string BackupFileName = "KabyliaTaste-app.db";
        private const string ApplicationName = "KabyliaTaste";

        public void UploadDatabaseBackup(StoreSettings settings, string localDatabasePath, IProgress<int>? progress = null)
        {
            EnsureSettings(settings);

            if (!System.IO.File.Exists(localDatabasePath))
                throw new FileNotFoundException("The local database file was not found.", localDatabasePath);

            var tempPath = Path.Combine(Path.GetTempPath(), $"KabyliaTaste-{Guid.NewGuid():N}.db");
            try
            {
                progress?.Report(0);
                CreateConsistentBackup(localDatabasePath, tempPath);

                using var service = CreateDriveService(settings);
                var folderId = ResolveFolderId(service, settings.GoogleDriveFolderId);
                var existing = FindLatestBackupFile(service, folderId);

                using var stream = System.IO.File.OpenRead(tempPath);
                var fileMetadata = new Google.Apis.Drive.v3.Data.File
                {
                    Name = BackupFileName,
                    MimeType = "application/octet-stream"
                };

                if (!string.IsNullOrWhiteSpace(folderId))
                {
                    fileMetadata.Parents = new List<string> { folderId! };
                }

                if (existing != null)
                {
                    var request = service.Files.Update(fileMetadata, existing.Id, stream, "application/octet-stream");
                    request.Fields = "id,name,modifiedTime";
                    ConfigureUploadProgress(request, stream, progress);
                    request.Upload();
                }
                else
                {
                    var request = service.Files.Create(fileMetadata, stream, "application/octet-stream");
                    request.Fields = "id,name,modifiedTime";
                    ConfigureUploadProgress(request, stream, progress);
                    request.Upload();
                }

                progress?.Report(100);
            }
            finally
            {
                TryDelete(tempPath);
            }
        }

        public void DownloadDatabaseBackup(StoreSettings settings, string localDatabasePath)
            => DownloadDatabaseBackup(settings, localDatabasePath, null);

        public void DownloadDatabaseBackup(StoreSettings settings, string localDatabasePath, IProgress<int>? progress = null)
        {
            EnsureSettings(settings);

            using var service = CreateDriveService(settings);
            var folderId = ResolveFolderId(service, settings.GoogleDriveFolderId);
            var file = FindLatestBackupFile(service, folderId)
                ?? throw new InvalidOperationException("No backup file was found in Google Drive.");

            var tempPath = Path.Combine(Path.GetTempPath(), $"KabyliaTaste-{Guid.NewGuid():N}.db");
            try
            {
                progress?.Report(0);

                using (var output = System.IO.File.Create(tempPath))
                {
                    var request = service.Files.Get(file.Id);
                    ConfigureDownloadProgress(request.MediaDownloader, file, progress);
                    request.Download(output);
                }

                progress?.Report(100);
                ReplaceLocalDatabase(localDatabasePath, tempPath);
            }
            finally
            {
                TryDelete(tempPath);
            }
        }

        public async Task<string> GenerateRefreshTokenAsync(StoreSettings settings)
        {
            EnsureClientSettings(settings);

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = settings.GoogleDriveClientId,
                    ClientSecret = settings.GoogleDriveClientSecret
                },
                Scopes = new[] { DriveService.Scope.DriveFile }
            });

            var app = new AuthorizationCodeInstalledApp(flow, new LocalServerCodeReceiver());
            var credential = await app.AuthorizeAsync(ApplicationName, CancellationToken.None);

            if (string.IsNullOrWhiteSpace(credential.Token.RefreshToken))
                throw new InvalidOperationException("Google did not return a refresh token. Try again and approve offline access.");

            return credential.Token.RefreshToken;
        }

        private static DriveService CreateDriveService(StoreSettings settings)
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = settings.GoogleDriveClientId,
                    ClientSecret = settings.GoogleDriveClientSecret
                },
                Scopes = new[] { DriveService.Scope.DriveFile }
            });

            var credential = new UserCredential(
                flow,
                ApplicationName,
                new TokenResponse { RefreshToken = settings.GoogleDriveRefreshToken });

            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        private static Google.Apis.Drive.v3.Data.File? FindLatestBackupFile(DriveService service, string? folderId)
        {
            var request = service.Files.List();
            request.PageSize = 10;
            request.Fields = "files(id,name,modifiedTime,size)";
            request.OrderBy = "modifiedTime desc";
            request.Q = BuildQuery(folderId);

            var result = request.Execute();
            return result.Files?.FirstOrDefault();
        }

        private static string? ResolveFolderId(DriveService service, string? folderIdOrName)
        {
            if (string.IsNullOrWhiteSpace(folderIdOrName))
                return null;

            var trimmed = folderIdOrName.Trim();

            try
            {
                var request = service.Files.Get(trimmed);
                request.Fields = "id,mimeType";
                var file = request.Execute();

                if (string.Equals(file.MimeType, "application/vnd.google-apps.folder", StringComparison.OrdinalIgnoreCase))
                    return file.Id;
            }
            catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Fall back to resolving by folder name.
            }

            var search = service.Files.List();
            search.PageSize = 10;
            search.Fields = "files(id,name)";
            search.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{EscapeDriveQueryValue(trimmed)}' and trashed = false";

            var result = search.Execute();
            return result.Files?.FirstOrDefault()?.Id;
        }

        private static string BuildQuery(string? folderId)
        {
            var clauses = new List<string>
            {
                $"name = '{BackupFileName.Replace("'", "\\'")}'",
                "trashed = false"
            };

            if (!string.IsNullOrWhiteSpace(folderId))
            {
                clauses.Add($"'{folderId}' in parents");
            }

            return string.Join(" and ", clauses);
        }

        private static string EscapeDriveQueryValue(string value)
        {
            return value.Replace("'", "\\'");
        }

        private static void CreateConsistentBackup(string sourceDatabasePath, string destinationPath)
        {
            var sourceConnectionString = new SqliteConnectionStringBuilder
            {
                DataSource = sourceDatabasePath,
                Pooling = false
            }.ToString();

            var destinationConnectionString = new SqliteConnectionStringBuilder
            {
                DataSource = destinationPath,
                Pooling = false
            }.ToString();

            using var source = new SqliteConnection(sourceConnectionString);
            source.Open();

            using var destination = new SqliteConnection(destinationConnectionString);
            destination.Open();

            source.BackupDatabase(destination);
        }

        private static void ReplaceLocalDatabase(string localDatabasePath, string sourcePath)
        {
            var backupPath = $"{localDatabasePath}.before-drive-restore-{DateTime.Now:yyyyMMdd-HHmmss}";
            if (System.IO.File.Exists(localDatabasePath))
            {
                System.IO.File.Copy(localDatabasePath, backupPath, true);
            }

            System.IO.File.Copy(sourcePath, localDatabasePath, true);
        }

        private static void EnsureSettings(StoreSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.GoogleDriveClientId) ||
                string.IsNullOrWhiteSpace(settings.GoogleDriveClientSecret) ||
                string.IsNullOrWhiteSpace(settings.GoogleDriveRefreshToken))
            {
                throw new InvalidOperationException("Google Drive settings are incomplete. Fill Client ID, Client Secret, and Refresh Token first.");
            }
        }

        private static void EnsureClientSettings(StoreSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.GoogleDriveClientId) ||
                string.IsNullOrWhiteSpace(settings.GoogleDriveClientSecret))
            {
                throw new InvalidOperationException("Google Drive Client ID and Client Secret are required to generate a refresh token.");
            }
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }
            catch
            {
                // ignore cleanup issues
            }
        }

        private static void ConfigureUploadProgress(object request, Stream stream, IProgress<int>? progress)
        {
            if (progress == null)
                return;

            var eventInfo = request.GetType().GetEvent("ProgressChanged");
            if (eventInfo == null)
                return;

            Action<IUploadProgress> handler = uploadProgress =>
            {
                if (!stream.CanSeek || stream.Length <= 0)
                    return;

                if (uploadProgress.Status == UploadStatus.Uploading || uploadProgress.Status == UploadStatus.Completed)
                {
                    var percent = (int)Math.Min(100, Math.Round(uploadProgress.BytesSent * 100d / stream.Length));
                    progress.Report(percent);
                }
            };

            eventInfo.AddEventHandler(request, handler);
        }

        private static void ConfigureDownloadProgress(object request, Google.Apis.Drive.v3.Data.File file, IProgress<int>? progress)
        {
            if (progress == null)
                return;

            var mediaDownloaderProperty = request.GetType().GetProperty("MediaDownloader");
            var mediaDownloader = mediaDownloaderProperty?.GetValue(request);
            if (mediaDownloader == null)
                return;

            var eventInfo = mediaDownloader.GetType().GetEvent("ProgressChanged");
            if (eventInfo == null)
                return;

            EventHandler<IDownloadProgress> handler = (sender, downloadProgress) =>
            {
                if (!file.Size.HasValue || file.Size.Value <= 0)
                    return;

                if (downloadProgress.Status == DownloadStatus.Downloading ||
                    downloadProgress.Status == DownloadStatus.Completed)
                {
                    var percent = (int)Math.Min(100, Math.Round(downloadProgress.BytesDownloaded * 100d / file.Size.Value));
                    progress.Report(percent);
                }
            };

            eventInfo.AddEventHandler(mediaDownloader, handler);
        }

        private static void ConfigureDownloadProgress(
            Google.Apis.Download.MediaDownloader mediaDownloader,
            Google.Apis.Drive.v3.Data.File file,
            IProgress<int>? progress)
        {
            if (progress == null || !file.Size.HasValue || file.Size.Value <= 0)
                return;

            Action<Google.Apis.Download.IDownloadProgress> handler = downloadProgress =>
            {
                if (downloadProgress.Status is Google.Apis.Download.DownloadStatus.Downloading
                    or Google.Apis.Download.DownloadStatus.Completed)
                {
                    var percent = (int)Math.Min(
                        100,
                        Math.Round(downloadProgress.BytesDownloaded * 100d / file.Size.Value));

                    progress.Report(percent);
                }
            };

            mediaDownloader.ProgressChanged += handler;
        }
    }
}
