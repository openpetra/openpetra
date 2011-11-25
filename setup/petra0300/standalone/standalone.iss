[Setup]
AppCopyright=by developers of OpenPetra.org
AppName=OpenPetra.org
AppVerName=OpenPetra.org {#PATCHVERSION}
DefaultDirName={pf}\OpenPetra.org
DefaultGroupName={cm:cmIconStandaloneLabel}
AppPublisherURL=http://www.openpetra.org
LicenseFile=..\..\..\LICENSE
VersionInfoVersion={#RELEASEID}
VersionInfoCompany=OM International
VersionInfoDescription=Administration Software for Charities
VersionInfoCopyright=2011 OM International
OutputBaseFilename=OpenPetraSetup-{#RELEASEVERSION}
OutputDir={#DELIVERY.DIR}
PrivilegesRequired=admin

[Languages]
Name: en; MessagesFile: compiler:Default.isl,..\language\lang-en.isl
Name: de; MessagesFile: compiler:Languages\German.isl,..\language\lang-de.isl

[Dirs]
Name: {app}/bin30
Name: {app}/manuals30
Name: {app}/db30
Name: {app}/reports30
Name: {app}/resources30
Name: {app}/sql30
Name: {app}/demo30

[Files]
Source: ..\..\..\delivery\bin\SourceGrid.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Mono.Data.Sqlite.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\sqlite3.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\ICSharpCode.SharpZipLib.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\GNU.Gettext.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Npgsql.dll; DestDir: {app}/bin30; Flags: ignoreversion

Source: ..\..\..\delivery\bin\Ict.Common*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.ClientPlugins*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.ServerPlugins*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Client*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Shared*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Server*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.ServerAdmin*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\PetraServerAdminConsole.exe; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\PetraClient.exe; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\PetraServerConsole.exe; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\tmp\UINavigation.yml; DestDir: {app}/bin30
Source: ..\..\..\delivery\bin\de-DE\OpenPetra.resources.dll; DestDir: {app}/bin30/de-DE
Source: ..\..\..\delivery\bin\es-ES\OpenPetra.resources.dll; DestDir: {app}/bin30/es-ES
Source: ..\..\..\delivery\bin\da-DK\OpenPetra.resources.dll; DestDir: {app}/bin30/da-DK
Source: ..\..\..\XmlReports\reports.dtd; DestDir: {app}/reports30
Source: ..\..\..\XmlReports\*.xml; DestDir: {app}/reports30; Flags: recursesubdirs createallsubdirs
Source: ..\..\..\csharp\ICT\Petra\Server\sql\*.sql; DestDir: {app}/sql30
Source: ..\..\..\csharp\ICT\Petra\Server\sql\*.yml; DestDir: {app}/sql30
Source: PetraServerAdminConsole.config; DestDir: {app}; DestName: PetraServerAdminConsole-3.0.config
Source: PetraClient.config; DestDir: {app}; DestName: PetraClient-3.0.config
Source: PetraServerConsole-Sqlite.config; DestDir: {app}; DestName: PetraServerConsole-3.0.config
Source: ..\releasenotes\releasenotes*html; DestDir: {app}/manuals30
; actual db will be copied to the user's userappdata directory
Source: ..\petra.db; DestDir: {app}/db30; DestName: demo.db
Source: ..\..\..\db\patches\*.sql; DestDir: {app}/db30
Source: ..\..\..\demodata\*.*; DestDir: {app}/demo30; Flags: recursesubdirs createallsubdirs
Source: ..\..\..\resources\petraico-big.ico; DestDir: {app}
Source: ..\..\..\resources\*.ico; DestDir: {app}/resources30
Source: ..\..\..\resources\*.png; DestDir: {app}/resources30
Source: ..\..\..\LICENSE; DestDir: {app}
Source: ..\..\..\tmp\version.txt; DestDir: {app}/bin30

[Icons]
Name: {group}\{cm:cmIconStandaloneLabel}; Filename: {app}\bin30\PetraClient.exe; WorkingDir: {app}/bin30; IconFilename: {app}\petraico-big.ico; Comment: {cm:cmIconStandaloneComment}; IconIndex: 0; Parameters: "-C:""{app}\PetraClient-3.0.config"" -AutoLogin:demo"
Name: {group}\{cm:cmIconReleaseNotesLabel}; Filename: {app}\manuals30\{cm:cmReleaseNotesFile}; WorkingDir: {app}/manuals30; Comment: {cm:cmIconReleaseNotesComment}
Name: {commondesktop}\{groupname}; Filename: {app}\bin30\PetraClient.exe; WorkingDir: {app}/bin30; IconFilename: {app}\petraico-big.ico; Comment: Start OpenPetra.org; IconIndex: 0; Parameters: "-C:""{app}\PetraClient-3.0.config"" -AutoLogin:demo"; Tasks: iconDesktop

[Tasks]
Name: iconDesktop; Description: {cm:cmIconTask}

[Run]
Filename: {app}\manuals30\{cm:cmReleaseNotesFile}; Description: {cm:cmViewReleaseNotes}; Flags: shellexec skipifdoesntexist postinstall skipifsilent

[Code]
#include "../utils/fileutils.iiss"
#include "../utils/DotNetFramework.iiss"

var
	DotNetPage: TOutputMsgMemoWizardPage;
procedure InitializeWizard;
begin
	DotNetPage := CreatePage_MissingNetFrameWork(wpPreparing);
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  { Skip pages that shouldn't be shown }
  if (PageID = DotNetPage.ID) and IsDotNetInstalled() then
  begin
    Result := true
  end;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;
  { Validate certain pages before allowing the user to proceed }
  if (CurPageID = DotNetPage.Id) then
  begin
    result :=  IsDotNetInstalled();
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
    ResultCode: Integer;
    Dirname: String;
begin
  if CurStep=ssPostInstall then
  begin
    Dirname := ExpandConstant('{app}');
    StringChangeEx(Dirname, ExpandConstant('{pf}') + '\', '', true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraServerConsole-3.0.config'), 'PETRA.DB', '{userappdata}/' + Dirname + '\db30\petra.db', true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraServerConsole-3.0.config'), 'BASE.DB', ExpandConstant('{app}\db30\demo.db'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraServerConsole-3.0.config'), 'REPORTS30', ExpandConstant('{app}\reports30'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraServerConsole-3.0.config'), 'SQL30', ExpandConstant('{app}\sql30'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraServerConsole-3.0.config'), 'TMP30', '{userappdata}/' + Dirname + '\tmp30', true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), 'PetraServerConsole.exe.config', ExpandConstant('{app}\PetraServerConsole-3.0.config'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), 'PetraServerAdminConsole.exe.config', ExpandConstant('{app}\PetraServerAdminConsole-3.0.config'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), 'TMP30', '{userappdata}/' + Dirname + '\tmp30', true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), 'REPORTSETTINGSPATH', ExpandConstant('{app}\reports30\Settings'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), 'REPORTUSERSETTINGSPATH', '{userappdata}\' + Dirname + '\reports30\Settings', true);
  end;

  // allow the .net remoting communication between client and server
  Exec(ExpandConstant('{sys}\cmd.exe'), '/C netsh firewall set allowedprogram program = '
    + ExpandConstant('"{app}\bin30\PetraClient.exe" name = PetraClient mode = DISABLE'),
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);

  Exec(ExpandConstant('{sys}\cmd.exe'), '/C netsh firewall set allowedprogram program = '
    + ExpandConstant('"{app}\bin30\PetraServerConsole.exe" name = PetraServer mode = DISABLE'),
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);

end;
