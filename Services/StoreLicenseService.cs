using Windows.ApplicationModel;
using Windows.Services.Store;

namespace KabyliaTaste.Services
{
    public sealed class StoreLicenseCheckResult
    {
        public bool IsPackagedApp { get; init; }
        public bool IsLicenseValid { get; init; }
        public bool IsTrial { get; init; }
        public DateTimeOffset? ExpirationDate { get; init; }
        public string? ErrorMessage { get; init; }

        public static StoreLicenseCheckResult Unpackaged() => new()
        {
            IsPackagedApp = false,
            IsLicenseValid = true
        };

        public static StoreLicenseCheckResult Valid(bool isTrial, DateTimeOffset? expirationDate) => new()
        {
            IsPackagedApp = true,
            IsLicenseValid = true,
            IsTrial = isTrial,
            ExpirationDate = expirationDate
        };

        public static StoreLicenseCheckResult Invalid(string message) => new()
        {
            IsPackagedApp = true,
            IsLicenseValid = false,
            ErrorMessage = message
        };
    }

    public sealed class StoreLicenseService
    {
        public async Task<StoreLicenseCheckResult> CheckLicenseAsync()
        {
            if (!IsPackagedApp())
                return StoreLicenseCheckResult.Unpackaged();

            try
            {
                var context = StoreContext.GetDefault();
                var license = await context.GetAppLicenseAsync();

                if (!license.IsActive)
                    return StoreLicenseCheckResult.Invalid("Une licence Microsoft Store valide est introuvable.");

                if (license.IsTrial)
                {
                    if (license.ExpirationDate <= DateTimeOffset.Now)
                        return StoreLicenseCheckResult.Invalid("Votre essai de 7 jours a expiré.");

                    return StoreLicenseCheckResult.Valid(true, license.ExpirationDate);
                }

                return StoreLicenseCheckResult.Valid(false, null);
            }
            catch (Exception ex)
            {
                return StoreLicenseCheckResult.Invalid($"Impossible de vérifier la licence Microsoft Store : {ex.Message}");
            }
        }

        private static bool IsPackagedApp()
        {
            try
            {
                _ = Package.Current;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
