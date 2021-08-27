; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define AppName "sUpdater"
#define AppVersion "4.0.4"
#define AppPublisher "Slim Software"
#define AppURL "http://www.slimsoft.tk"
#define AppExeName "sUpdater.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{9016A0E6-C775-4FED-8C21-4A2E5456E1EB}}
AppName={#AppName}
AppVersion={#AppVersion}
;AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={commonpf}\Slim Software\sUpdater
DisableDirPage=yes
DefaultGroupName={#AppPublisher}\{#AppName}
DisableProgramGroupPage=yes
LicenseFile=license.txt
OutputBaseFilename=sUpdater-v{#AppVersion}-setup
SetupIconFile=..\Icons\sUpdater.ico
Compression=lzma
SolidCompression=yes
MinVersion=0,6.1
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
Source: "..\bin\Release\sUpdater.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\sUpdater.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\AutoUpdater.NET.dll"; DestDir: "{app}"
Source: "..\bin\Release\AutoUpdater.NET.xml"; DestDir: "{app}"
Source: "..\bin\Release\Hardcodet.Wpf.TaskbarNotification.dll"; DestDir: "{app}"
Source: "..\bin\Release\Hardcodet.Wpf.TaskbarNotification.xml"; DestDir: "{app}"
Source: "..\bin\Release\7-Zip\7z.dll"; DestDir: "{app}"
Source: "..\bin\Release\7-Zip\7z.exe"; DestDir: "{app}"
Source: "..\bin\Release\AsyncEnumerable.dll"; DestDir: "{app}"
Source: "..\bin\Release\AsyncEnumerable.xml"; DestDir: "{app}"
Source: "..\bin\Release\System.Runtime.CompilerServices.Unsafe.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\System.Threading.Tasks.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\System.Threading.Tasks.Extensions.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\System.Runtime.CompilerServices.Unsafe.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\Microsoft.Bcl.AsyncInterfaces.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\Microsoft.Bcl.AsyncInterfaces.dll"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExeName}"
Name: "{commondesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{app}\sUpdater.exe"; IconIndex: 0; Tasks: desktopicon

[Registry]
Root: "HKCU"; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "sUpdater"; ValueData: """{app}\{#AppExeName}"" /tray"; Flags: uninsdeletevalue; Tasks: autostart

[Run]
Filename: "{app}\{#AppExeName}"; Flags: nowait postinstall; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"
