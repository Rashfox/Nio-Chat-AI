; Nio Chat Installer Script - Inno Setup 6
; Output: NioChatSetup.exe (~60-70MB setelah kompresi LZMA)

#define AppName "Nio Chat"
#define AppVersion "1.0.1"
#define AppPublisher "Nio"
#define AppExeName "NioChat.exe"
#define SourceDir "app_publish"

[Setup]
AppId={{F4A2B3C1-8E5D-4F7A-9B2C-3D6E8F1A4B5C}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisherURL=http://localhost
AppSupportURL=http://localhost
AppUpdatesURL=http://localhost
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
AllowNoIcons=yes
; Output installer ke folder installer/
OutputDir=installer
OutputBaseFilename=NioChatSetup
; Kompresi LZMA maksimal (seperti 7-Zip)
Compression=lzma2/ultra64
SolidCompression=yes
LZMAUseSeparateProcess=yes
; Ikon installer
SetupIconFile={#SourceDir}\app.ico
; Tampilan wizard
WizardStyle=modern
; Minimum Windows 10
MinVersion=10.0.17763
; 64-bit only
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
; Uninstaller
UninstallDisplayIcon={app}\{#AppExeName}
UninstallDisplayName={#AppName}
; Shortcut di start menu dan desktop
DisableProgramGroupPage=yes
; Tidak perlu restart
RestartIfNeededByRun=no
; Tutup aplikasi otomatis jika sedang berjalan saat install / update
CloseApplications=yes
CloseApplicationsFilter=*NioChat.exe*
UsedUserAreasWarning=no
; Tampilkan lisensi (opsional)
; LicenseFile=LICENSE

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Salin semua file dari folder app_publish
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
; Start Menu
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExeName}"; WorkingDir: "{app}"; IconFilename: "{app}\app.ico"; AppUserModelID: "Nio.Chat.App"
; Desktop (opsional, user bisa pilih)
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; WorkingDir: "{app}"; IconFilename: "{app}\app.ico"; Tasks: desktopicon; AppUserModelID: "Nio.Chat.App"


[Run]
; Langsung buka aplikasi setelah install selesai
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; WorkingDir: "{app}"; Flags: nowait postinstall skipifsilent

[UninstallRun]
; Matikan aplikasi sebelum uninstall
Filename: "taskkill.exe"; Parameters: "/F /IM {#AppExeName}"; Flags: runhidden skipifdoesntexist; RunOnceId: "KillNioChat"

[UninstallDelete]
; Hapus data riwayat & profil agar fresh install benar-benar bersih
Type: filesandordirs; Name: "{userappdata}\NioChat"
Type: filesandordirs; Name: "{localappdata}\NioChat"

