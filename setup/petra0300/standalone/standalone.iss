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
Name: {app}/formletters30
Name: {app}/demo30

[Files]
Source: ..\..\..\delivery\bin\SourceGrid.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Mono.Data.Sqlite.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\sqlite3.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\ICSharpCode.SharpZipLib.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\GNU.Gettext.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Npgsql.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Owf.Controls.A1Panel.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\CustomControl.OrientedTextControls.dll; DestDir: {app}/bin30; Flags: ignoreversion

Source: ..\..\..\delivery\bin\Ict.Common*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Client*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Shared*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Server*.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\PetraClient.exe; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\tmp\UINavigation.yml; DestDir: {app}/bin30
Source: ..\..\..\delivery\bin\de-DE\OpenPetra.resources.dll; DestDir: {app}/bin30/de-DE
Source: ..\..\..\delivery\bin\es-ES\OpenPetra.resources.dll; DestDir: {app}/bin30/es-ES
Source: ..\..\..\delivery\bin\da-DK\OpenPetra.resources.dll; DestDir: {app}/bin30/da-DK
Source: ..\..\..\XmlReports\reports.dtd; DestDir: {app}/reports30
Source: ..\..\..\XmlReports\*.xml; DestDir: {app}/reports30; Flags: recursesubdirs createallsubdirs
Source: ..\..\..\csharp\ICT\Petra\Server\sql\*.sql; DestDir: {app}/sql30
Source: ..\..\..\csharp\ICT\Petra\Server\sql\*.yml; DestDir: {app}/sql30
Source: ..\..\..\demodata\formletters\*.html; DestDir: {app}/formletters30
Source: ..\..\..\demodata\formletters\*.png; DestDir: {app}/formletters30
Source: PetraClient.config; DestDir: {app}; DestName: PetraClient-3.0.config
Source: ..\releasenotes\releasenotes*html; DestDir: {app}/manuals30
; actual db will be copied to the user's userappdata directory
Source: ..\..\..\delivery\demo.db; DestDir: {app}/db30; DestName: demo.db
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
    SlashPosition: Integer;
begin
  if CurStep=ssPostInstall then
  begin
    Dirname := ExpandConstant('{app}');
    
    StringChangeEx(Dirname, ExpandConstant('{pf}') + '\', '', true);

    { this includes c:\myownprog\openpetra, if c:\myownprog is not the default directory for program files in windows }
    SlashPosition := Pos('\', DirName);
    while (SlashPosition <> 0)
    do begin
      { Remove the old text that was found }
      Delete(DirName, 1, SlashPosition);
      SlashPosition := Pos('\', DirName);
    end;

    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), '../db30/petra.db', '{userappdata}/' + Dirname + '\db30\petra.db', true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), '../db30/demo.db', ExpandConstant('{app}\db30\demo.db'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), '../reports30', ExpandConstant('{app}\reports30'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), '../sql30', ExpandConstant('{app}\sql30'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), '../tmp30', '{userappdata}\' + Dirname + '\tmp30', true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), '<add key="Reporting.PathReportSettings" value="../reports30/Settings" />', ExpandConstant('<add key="Reporting.PathReportSettings" value="{app}\reports30\Settings" />'), true);
    ReplaceInTextFile(ExpandConstant('{app}/PetraClient-3.0.config'), '<add key="Reporting.PathReportUserSettings" value="../reports30/Settings" />', '<add key="Reporting.PathReportUserSettings" value="{userappdata}\' + Dirname + '\reports30\Settings" />', true);
  end;
end;
