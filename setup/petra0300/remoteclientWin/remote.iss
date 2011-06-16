[Setup]
AppCopyright=by developers of OpenPetra.org
AppName=OpenPetra.org {#ORGNAME}
AppVerName=OpenPetra.org {#ORGNAME} {#PATCHVERSION}
DefaultDirName={pf}\OpenPetra{#ORGNAME}
DefaultGroupName=OpenPetra.org {#ORGNAME}
AppPublisherURL=http://{#PUBLISHERURL}
LicenseFile=..\..\..\LICENSE
VersionInfoVersion={#RELEASEID}
VersionInfoCompany=OM International
VersionInfoDescription=Administration Software for Charities
VersionInfoCopyright=2010 OM International
OutputBaseFilename=OpenPetraRemoteSetup-{#ORGNAME}-{#RELEASEVERSION}
OutputDir={#DELIVERY.DIR}
PrivilegesRequired=admin

[Languages]
Name: en; MessagesFile: compiler:Default.isl,..\language\lang-en.isl
Name: de; MessagesFile: compiler:Languages\German.isl,..\language\lang-de.isl

[Dirs]
Name: {app}/bin30; permissions: users-full
Name: {app}/patches30; permissions: users-full
Name: {app}/manuals30; permissions: users-full
Name: {app}/resources30; permissions: users-full
Name: {app}/reports30/Settings; permissions: users-full
Name: {app}/etc30; permissions: users-full

[Files]
Source: ..\..\..\csharp\ThirdParty\DevAge\SourceGrid.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\SQLite\System.Data.SQLite.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\ICSharpCode\ICSharpCode.SharpZipLib.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\GNU\GNU.Gettext.dll; DestDir: {app}/bin30; Flags: ignoreversion
	Source: ..\..\..\csharp\ThirdParty\Npgsql\Npgsql.dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Common*dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Client*dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Shared*dll; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\PetraClient.exe; DestDir: {app}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ICT\Petra\Definitions\{#UINAVIGATION}; DestDir: {app}/bin30
Source: ..\..\..\delivery\bin\de-DE\OpenPetra.resources.dll; DestDir: {app}/bin30/de-DE
Source: ..\..\..\delivery\bin\es-ES\OpenPetra.resources.dll; DestDir: {app}/bin30/es-ES
Source: ..\..\..\delivery\bin\da-DK\OpenPetra.resources.dll; DestDir: {app}/bin30/da-DK
Source: ..\..\..\XmlReports\Settings\*.xml; DestDir: {app}/reports30/Settings; Flags: recursesubdirs createallsubdirs
Source: {#REMOTECONFIG}; DestDir: {app}/etc30
Source: ..\releasenotes\releasenotes*html; DestDir: {app}/manuals30
Source: ..\..\..\resources\petraico-big.ico; DestDir: {app}
Source: ..\..\..\resources\*.ico; DestDir: {app}/resources30
Source: ..\..\..\resources\*.png; DestDir: {app}/resources30
Source: ..\..\..\LICENSE; DestDir: {app}
Source: version.txt; DestDir: {app}/bin30

[Icons]
Name: {group}\{cm:cmIconRemoteLabel}; Filename: {app}\bin30\PetraClient.exe; WorkingDir: {app}/bin30; IconFilename: {app}\petraico-big.ico; Comment: {cm:cmIconRemoteComment}; IconIndex: 0; Parameters: "-C:""{app}\etc30\PetraClientRemote.config"""
Name: {group}\{cm:cmIconReleaseNotesLabel}; Filename: {app}\manuals30\{cm:cmReleaseNotesFile}; WorkingDir: {app}/manuals30; Comment: {cm:cmIconReleaseNotesComment}
Name: {commondesktop}\{groupname}; Filename: {app}\bin30\PetraClient.exe; WorkingDir: {app}/bin30; IconFilename: {app}\petraico-big.ico; Comment: Start OpenPetra.org; IconIndex: 0; Parameters: "-C:""{app}\etc30\PetraClientRemote.config"""; Tasks: iconDesktop

[Tasks]
Name: iconDesktop; Description: {cm:cmIconTask}

[Run]
Filename: {app}\manuals30\{cm:cmReleaseNotesFile}; Description: {cm:cmViewReleaseNotes}; Flags: shellexec skipifdoesntexist postinstall skipifsilent

[Code]
#include "../utils/fileutils.iiss"
#include "../utils/DotNetFramework.iiss"

var
    DotNetPage: TOutputMsgMemoWizardPage;
    PetraServerConnectionPage: TWizardPage;
    ctrlPetraServerConnectionHostName, ctrlPetraServerConnectionNET:  TEdit;
    strServer : string;
    NetPort: Integer;

// This page will ask the user for the Server and Port to find Petra listening on.
function CreatePage_PetraServerConnection(const afterId: Integer; AStrServer: String; ANETPort: Integer): TWizardPage;
var
  currentY: integer;
  lblHeader, lblHostName, lblNET: TNewStaticText;
begin
  Result := CreateCustomPage(afterId,
    ExpandConstant('{cm:cmPetraServerConnectionTitle}'), ExpandConstant('{cm:cmPetraServerConnectionSubTitle}'));

  lblHeader := TNewStaticText.Create(Result);
  with lblHeader do begin
    AutoSize := False;
    Width := Result.SurfaceWidth - Left;
    WordWrap := True;
    Caption := ExpandConstant('{cm:cmExplanationPetraServerDetails}');
    Parent := Result.Surface;
  end;
  WizardForm.AdjustLabelHeight(lblHeader);

  currentY := lblHeader.Top + lblHeader.Height + ScaleY(8);

  lblHostName := TNewStaticText.Create(Result);
  lblHostName.Width := ScaleX(10);
  lblHostName.Top := currentY;
  lblHostName.Caption := ExpandConstant('{cm:cmPetraServerConnectionHostName}') + ':';
  lblHostName.Parent := Result.Surface;
  ctrlPetraServerConnectionHostName := TEdit.Create(Result);
  ctrlPetraServerConnectionHostName.Top := currentY;
  ctrlPetraServerConnectionHostName.Width := Result.SurfaceWidth div 2 - ScaleX(8);
  ctrlPetraServerConnectionHostName.Left := lblHostName.Left + ScaleX(100) + ScaleX(8);
  ctrlPetraServerConnectionHostName.Text := AStrServer;
  ctrlPetraServerConnectionHostName.Parent := Result.Surface;

  currentY := currentY + ctrlPetraServerConnectionHostName.Height + ScaleY(8);
  lblNET := TNewStaticText.Create(Result);
  lblNET.Width := ScaleX(10);
  lblNET.Top := currentY;
  lblNET.Caption := ExpandConstant('{cm:cmPetraServerConnectionNetPort}') + ':';
  lblNET.Parent := Result.Surface;
  ctrlPetraServerConnectionNET := TEdit.Create(Result);
  ctrlPetraServerConnectionNET.Top := currentY;
  ctrlPetraServerConnectionNET.Width := Result.SurfaceWidth div 6;
  ctrlPetraServerConnectionNET.Left := lblNET.Left + ScaleX(100) + ScaleX(8);
  ctrlPetraServerConnectionNET.Text := IntToStr(ANETPort);
  ctrlPetraServerConnectionNET.Parent := Result.Surface;
end;

procedure InitializeWizard;
begin
    strServer := '{#SERVERHOST}';
    NetPort := {#SERVERPORT};
    // PetraServerConnectionPage := CreatePage_PetraServerConnection(wpPreparing, strServer, NETPort);
	PetraServerConnectionPage := nil;
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
  end
  else if ((PetraServerConnectionPage <> nil) and (CurPageID = PetraServerConnectionPage.ID)) then
  begin
    strServer := ctrlPetraServerConnectionHostName.Text;
    NetPort := StrToInt(ctrlPetraServerConnectionNET.Text);
    result := (Length(strServer) > 0) and (NETPort > 0);
    if (not result) then
    begin
      MsgBox( ExpandConstant('{cm:cmPleaseEnterValidValues}'), mbError, MB_OK);
    end;
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
	ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClientRemote.config'), 'TMP30', '{userappdata}/' + Dirname + '/tmp30', true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClientRemote.config'), 'PATCHES30', ExpandConstant('{app}/patches30'), true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClientRemote.config'), 'REMOTEPATCHESPATH', 'https://{#SERVERHOST}/patches/{#ORGNAME}/', true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClientRemote.config'), 'REPORTUSERSETTINGSPATH', '{userappdata}/' + Dirname + '/reports30/Settings', true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClientRemote.config'), 'REPORTSETTINGSPATH', ExpandConstant('{app}/reports30/Settings'), true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClientRemote.config'), 'PETRAHOST', strServer, true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClientRemote.config'), 'PETRAPORT', IntToStr(NetPort), true);
  end;

  // allow the .net remoting communication between client and server
  Exec(ExpandConstant('{sys}\cmd.exe'), '/C netsh firewall set allowedprogram program = '
    + ExpandConstant('"{app}\bin30\PetraClient.exe" name = PetraClient mode = DISABLE'),
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;
