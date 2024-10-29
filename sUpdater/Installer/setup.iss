#define AppName "sUpdater"
#define AppVersion "5.0"
#define AppPublisher "Slim Software"
#define AppURL "http://www.slimsoftware.dev"
#define AppExeName "sUpdater.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
AppId={{9016A0E6-C775-4FED-8C21-4A2E5456E1EB}}
AppName={#AppName}
AppVersion={#AppVersion}
;AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
ArchitecturesInstallIn64BitMode=x64compatible
DefaultDirName={commonpf}\Slim Software\sUpdater
DisableDirPage=yes
DefaultGroupName={#AppPublisher}\{#AppName}
DisableProgramGroupPage=yes
LicenseFile=license.txt
OutputBaseFilename=sUpdater-v{#AppVersion}-setup
SetupIconFile=..\Icons\sUpdater.ico
Compression=lzma
SolidCompression=yes
MinVersion=0,6.1sp1
VersionInfoVersion={#AppVersion}
VersionInfoCompany={#AppPublisher}
VersionInfoProductName={#AppName}
VersionInfoProductVersion={#AppVersion}
UninstallDisplayName=sUpdater
UninstallDisplayIcon={app}\sUpdater.exe
WizardStyle=modern
WizardSmallImageFile=wizardsmall.bmp,wizardsmall_hq.bmp
UsePreviousAppDir=False
UsePreviousGroup=False

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "autostart"; Description: "Auto-start sUpdater as a system tray icon"

[Files]
Source: "..\bin\Release\net472\*"; DestDir: "{app}"
Source: "..\bin\Release\net472\sUpdater.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExeName}"
Name: "{commondesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{app}\sUpdater.exe"; IconIndex: 0; Tasks: desktopicon

[Registry]
Root: "HKCU"; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "sUpdater"; ValueData: """{app}\{#AppExeName}"" /tray"; Flags: uninsdeletevalue; Tasks: autostart

[Run]
Filename: "{app}\{#AppExeName}"; Flags: nowait postinstall; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"

[InstallDelete]
Type: filesandordirs; Name: "{autopf32}\Slim Software\sUpdater"; Check: Is64BitInstallMode
Type: files; Name: "{app}\Hardcodet.Wpf.TaskbarNotification.xml"
Type: files; Name: "{app}\7z.dll"
Type: files; Name: "{app}\7z.exe"
Type: files; Name: "{app}\*.xml"
Type: files; Name: "{app}\System.Drawing.Common.dll"
Type: filesandordirs; Name: "{app}\runtimes"
Type: files; Name: "{app}\AutoUpdater.NET.dll"
Type: files; Name: "{app}\Microsoft.Web.WebView2*.dll"
