param(
    [string]$Configuration = 'Release',
    [string]$RuntimeIdentifier = 'win-x64',
    [string]$Version = '1.0.1.0'
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
Add-Type -AssemblyName System.Drawing

function New-LogoPng {
    param(
        [Parameter(Mandatory = $true)] [string]$Path,
        [Parameter(Mandatory = $true)] [int]$Size,
        [Parameter(Mandatory = $true)] [System.Drawing.Color]$Background,
        [Parameter(Mandatory = $true)] [System.Drawing.Color]$Foreground,
        [string]$Text = 'A'
    )

    $bitmap = New-Object System.Drawing.Bitmap $Size, $Size
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
        $graphics.Clear($Background)

        $fontSize = [Math]::Max(10, [int]($Size * 0.55))
        $font = New-Object System.Drawing.Font('Segoe UI', $fontSize, [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Pixel)
        $brush = New-Object System.Drawing.SolidBrush($Foreground)
        $format = New-Object System.Drawing.StringFormat
        $format.Alignment = [System.Drawing.StringAlignment]::Center
        $format.LineAlignment = [System.Drawing.StringAlignment]::Center

        $rect = New-Object System.Drawing.RectangleF 0, 0, $Size, $Size
        $graphics.DrawString($Text, $font, $brush, $rect, $format)

        $bitmap.Save($Path, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        if ($graphics) { $graphics.Dispose() }
        if ($bitmap) { $bitmap.Dispose() }
    }
}

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptRoot
$projectPath = Join-Path $repoRoot 'KabyliaTaste.csproj'
$publishDir = Join-Path $scriptRoot 'publish'
$stagingDir = Join-Path $scriptRoot 'staging'
$outputDir = Join-Path $scriptRoot 'artifacts'
$certPath = Join-Path $scriptRoot 'AmiarStoreManager.pfx'
$certPassword = ConvertTo-SecureString 'AmiarStoreManager!2026' -AsPlainText -Force
$packagePath = Join-Path $outputDir "AmiarStoreManager_$Version`_x64.msix"
$certificateSubject = 'CN=Abderazak Amiar'
$publisher = $certificateSubject

function Import-DevCertificate {
    param(
        [Parameter(Mandatory = $true)] [string]$PfxPath,
        [Parameter(Mandatory = $true)] [System.Security.SecureString]$Password
    )

    Import-PfxCertificate -FilePath $PfxPath -CertStoreLocation 'Cert:\CurrentUser\TrustedPeople' -Password $Password | Out-Null
    Import-PfxCertificate -FilePath $PfxPath -CertStoreLocation 'Cert:\CurrentUser\Root' -Password $Password | Out-Null
}

foreach ($path in @($publishDir, $stagingDir, $outputDir)) {
    if (Test-Path $path) {
        Remove-Item $path -Recurse -Force
    }
    New-Item -ItemType Directory -Path $path | Out-Null
}

Write-Host 'Publishing application...'
dotnet publish $projectPath -c $Configuration -r $RuntimeIdentifier --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=false /p:PublishDir="$publishDir\" | Out-Null

Write-Host 'Staging package contents...'
Copy-Item -Path (Join-Path $publishDir '*') -Destination $stagingDir -Recurse -Force

$assetsDir = Join-Path $stagingDir 'Assets'
New-Item -ItemType Directory -Path $assetsDir | Out-Null

New-LogoPng -Path (Join-Path $assetsDir 'StoreLogo.png') -Size 50 -Background ([System.Drawing.Color]::FromArgb(0, 102, 204)) -Foreground ([System.Drawing.Color]::White)
New-LogoPng -Path (Join-Path $assetsDir 'Square44x44Logo.png') -Size 44 -Background ([System.Drawing.Color]::FromArgb(0, 102, 204)) -Foreground ([System.Drawing.Color]::White)
New-LogoPng -Path (Join-Path $assetsDir 'Square71x71Logo.png') -Size 71 -Background ([System.Drawing.Color]::FromArgb(0, 102, 204)) -Foreground ([System.Drawing.Color]::White)
New-LogoPng -Path (Join-Path $assetsDir 'Square150x150Logo.png') -Size 150 -Background ([System.Drawing.Color]::FromArgb(0, 102, 204)) -Foreground ([System.Drawing.Color]::White)
New-LogoPng -Path (Join-Path $assetsDir 'Wide310x150Logo.png') -Size 310 -Background ([System.Drawing.Color]::FromArgb(0, 102, 204)) -Foreground ([System.Drawing.Color]::White)
New-LogoPng -Path (Join-Path $assetsDir 'SplashScreen.png') -Size 620 -Background ([System.Drawing.Color]::FromArgb(0, 102, 204)) -Foreground ([System.Drawing.Color]::White)

$manifest = @'
<?xml version="1.0" encoding="utf-8"?>
<Package xmlns="http://schemas.microsoft.com/appx/manifest/foundation/windows10"
         xmlns:uap="http://schemas.microsoft.com/appx/manifest/uap/windows10"
         xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities"
         IgnorableNamespaces="uap rescap">
  <Identity Name="AmiarStoreManager"
            Publisher="{0}"
            Version="{1}"
            ProcessorArchitecture="x64" />
  <Properties>
    <DisplayName>Amiar Store Manager</DisplayName>
    <PublisherDisplayName>Abderazak Amiar</PublisherDisplayName>
    <Logo>Assets\StoreLogo.png</Logo>
    <Description>Amiar Store Manager</Description>
  </Properties>
  <Dependencies>
    <TargetDeviceFamily Name="Windows.Desktop" MinVersion="10.0.19041.0" MaxVersionTested="10.0.22621.0" />
  </Dependencies>
  <Resources>
    <Resource Language="en-us" />
  </Resources>
  <Applications>
    <Application Id="AmiarStoreManager"
                 Executable="AmiarStoreManager.exe"
                 EntryPoint="Windows.FullTrustApplication">
      <uap:VisualElements DisplayName="Amiar Store Manager"
                          Description="Amiar Store Manager"
                          BackgroundColor="transparent"
                          Square150x150Logo="Assets\Square150x150Logo.png"
                          Square44x44Logo="Assets\Square44x44Logo.png">
        <uap:DefaultTile Wide310x150Logo="Assets\Wide310x150Logo.png"
                         Square71x71Logo="Assets\Square71x71Logo.png" />
        <uap:SplashScreen Image="Assets\SplashScreen.png" />
      </uap:VisualElements>
    </Application>
  </Applications>
  <Capabilities>
    <rescap:Capability Name="runFullTrust" />
  </Capabilities>
</Package>
'@ -f $publisher, $Version

$manifestPath = Join-Path $stagingDir 'AppxManifest.xml'
Set-Content -Path $manifestPath -Value $manifest -Encoding UTF8

if (-not (Test-Path $certPath)) {
    Write-Host 'Creating signing certificate...'
    $cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject $certificateSubject -CertStoreLocation 'Cert:\CurrentUser\My'
    Export-PfxCertificate -Cert $cert -FilePath $certPath -Password $certPassword | Out-Null
}

Import-DevCertificate -PfxPath $certPath -Password $certPassword

Write-Host 'Creating MSIX package...'
& makeappx pack /d $stagingDir /p $packagePath /o | Out-Null

Write-Host 'Signing MSIX package...'
try {
    & signtool sign /fd SHA256 /f $certPath /p 'AmiarStoreManager!2026' /tr http://timestamp.digicert.com /td SHA256 $packagePath | Out-Null
}
catch {
    & signtool sign /fd SHA256 /f $certPath /p 'AmiarStoreManager!2026' $packagePath | Out-Null
}

Remove-Item $stagingDir -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item $certPath -Force -ErrorAction SilentlyContinue

Write-Host "MSIX package created at: $packagePath"
