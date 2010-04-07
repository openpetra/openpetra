[Setup]
AppCopyright=by developers of OpenPetra.org
AppName=OpenPetra.org
AppVerName=OpenPetra.org {#PATCHVERSION}
DefaultDirName={pf}\OpenPetra.org
DefaultGroupName=OpenPetra.org
AppPublisherURL=http://www.openpetra.org
LicenseFile=..\..\..\LICENSE
VersionInfoVersion={#RELEASEID}
VersionInfoCompany=OM International
VersionInfoDescription=Administration Software for Charities
VersionInfoCopyright=2009 OM International
OutputBaseFilename=OpenPetraSetup-{#RELEASEVERSION}

[Languages]
Name: en; MessagesFile: compiler:Default.isl,..\language\lang-en.isl
Name: de; MessagesFile: compiler:Languages\German.isl,..\language\lang-de.isl

[Dirs]
Name: {app}/bin30
Name: {app}/bin30/locale/de/LC_MESSAGES
Name: {app}/manuals30
Name: {app}/db30
Name: {app}/reports30
Name: {app}/resources30
Name: {app}/sql30
Name: {userappdata}/OpenPetra.org/tmp30
Name: {userappdata}/OpenPetra.org/reports30
[Files]
Source: ..\..\..\csharp\ThirdParty\DevAge\SourceGrid.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\SQLite\System.Data.SQLite.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\ICSharpCode\ICSharpCode.SharpZipLib.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\Mono\intl.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\Mono\Mono.Posix.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\Mono\Mono.Security.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\Mono\MonoPosixHelper.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\Npgsql\Npgsql.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\gtk-sharp\iconv.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\gtk-sharp\libxml2.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\Client\_bin\Release\Ict.Common*dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\Client\_bin\Release\Ict.Petra.Client*dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\Shared\_bin\Server_Client\Release\Ict.Petra.Shared*dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\Shared\_bin\Server_ServerAdmin\Release\Ict.Petra.Shared*dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\Server\_bin\Release\Ict.Petra.Server*dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\ServerAdmin\_bin\Release\Ict.Petra.ServerAdmin*dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\ServerAdmin\_bin\Release\PetraServerAdminConsole.exe; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\Client\_bin\Release\PetraClient.exe; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\Server\_bin\Release\PetraServerConsole.exe; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\Definitions\UINavigation.yml; DestDir: {app}/bin30
Source: ..\i18n\*.mo; DestDir: {app}/bin30; Flags: recursesubdirs createallsubdirs
Source: ..\..\..\XmlReports\reports.dtd; DestDir: {app}/reports30
Source: ..\..\..\XmlReports\*.xml; DestDir: {app}/reports30; Flags: recursesubdirs createallsubdirs
; Source: ..\..\..\csharp\ICT\Petra\Server\sql\*.sql; DestDir: {app}/sql30
Source: PetraServerAdminConsole.config; DestDir: {app}; DestName: PetraServerAdminConsole-3.0.config
Source: PetraClient.config; DestDir: {app}; DestName: PetraClient-3.0.config
Source: PetraServerConsole-Sqlite.config; DestDir: {app}; DestName: PetraServerConsole-3.0.config
Source: ..\releasenotes\releasenotes*html; DestDir: {app}/manuals30
; actual db will be copied to the user's userappdata directory
Source: ..\petra.db; DestDir: {app}/db30; DestName: demo.db
Source: ..\..\..\resources\petraico-big.ico; DestDir: {app}
Source: ..\..\..\resources\*.ico; DestDir: {app}/resources30
Source: ..\..\..\resources\*.png; DestDir: {app}/resources30
Source: ..\..\..\LICENSE; DestDir: {app}
Source: version.txt; DestDir: {app}/bin30

[Icons]
Name: {group}\{cm:cmIconStandaloneLabel}; Filename: {app}\bin30\PetraClient.exe; WorkingDir: {app}/bin30; IconFilename: {app}\petraico-big.ico; Comment: {cm:cmIconStandaloneComment}; IconIndex: 0; Parameters: "-C:""{app}\PetraClient-3.0.config"" -AutoLogin:demo"
Name: {group}\{cm:cmIconReleaseNotesLabel}; Filename: {app}\manuals30\{cm:cmReleaseNotesFile}; WorkingDir: {app}/manuals30; Comment: {cm:cmIconReleaseNotesComment}
Name: {commondesktop}\{cm:cmIconStandaloneLabel}; Filename: {app}\bin30\PetraClient.exe; WorkingDir: {app}/bin30; IconFilename: {app}\petraico-big.ico; Comment: Start OpenPetra.org; IconIndex: 0; Parameters: "-C:""{app}\PetraClient-3.0.config"" -AutoLogin:demo"; Tasks: iconDesktop

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
begin
  if CurStep=ssPostInstall then
  begin
    ReplaceInTextFile(ExpandConstant('{app}/PetraServerConsole-3.0.config'), 'U:/OpenPetra/setup/petra0300/petra.db', '{userappdata}/OpenPetra.org/db30/petra.db', true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraServerConsole-3.0.config'), 'U:/OpenPetra/setup/petra0300/base.db', ExpandConstant('{app}/db30/demo.db'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraServerConsole-3.0.config'), 'reports30', ExpandConstant('{app}/reports30'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraServerConsole-3.0.config'), 'sql30', ExpandConstant('{app}/sql30'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), 'PetraServerConsole.exe.config', ExpandConstant('{app}/PetraServerConsole-3.0.config'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), 'PetraServerAdminConsole.exe.config', ExpandConstant('{app}/PetraServerAdminConsole-3.0.config'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), 'OpenPetra.PathTemp" value="TOREPLACE"', 'OpenPetra.PathTemp" value="{userappdata}/OpenPetra.org/tmp30"', true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), 'Reporting.PathReportSettings" value="TOREPLACE"', 'Reporting.PathReportSettings" value="{userappdata}/OpenPetra.org/reports30"', true);
  end;

  // allow the .net remoting communication between client and server
  Exec(ExpandConstant('{sys}\cmd.exe'), '/C netsh firewall set allowedprogram program = '
    + ExpandConstant('"{app}\bin30\PetraClient.exe" name = PetraClient mode = DISABLE'),
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);

  Exec(ExpandConstant('{sys}\cmd.exe'), '/C netsh firewall set allowedprogram program = '
    + ExpandConstant('"{app}\bin30\PetraServerConsole.exe" name = PetraServer mode = DISABLE'),
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);

end;
