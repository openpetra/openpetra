[Setup]
AppCopyright=by developers of OpenPetra.org
AppName=OpenPetra.org {#ORGNAME}
AppVerName=OpenPetra.org {#ORGNAME} {#PATCHVERSION}
DefaultDirName={userappdata}/OpenPetra{#ORGNAME}
; for the moment, don't use the previous app name, otherwise it will try to install as admin
UsePreviousAppDir=no
DefaultGroupName=OpenPetra.org {#ORGNAME}
AppPublisherURL=http://{#PUBLISHERURL}
LicenseFile=..\..\..\LICENSE
VersionInfoVersion={#RELEASEID}
VersionInfoCompany=OM International
VersionInfoDescription=Administration Software for Charities
VersionInfoCopyright=2011 OM International
OutputBaseFilename=OpenPetraRemoteSetup-{#ORGNAME}-{#RELEASEVERSION}
OutputDir={#DELIVERY.DIR}
PrivilegesRequired=lowest

[Languages]
Name: en; MessagesFile: compiler:Default.isl,..\language\lang-en.isl
Name: de; MessagesFile: compiler:Languages\German.isl,..\language\lang-de.isl

[Dirs]
Name: {userappdata}/OpenPetra{#ORGNAME}/bin30; permissions: users-full
Name: {userappdata}/OpenPetra{#ORGNAME}/patches30; permissions: users-full
Name: {userappdata}/OpenPetra{#ORGNAME}/manuals30; permissions: users-full
Name: {userappdata}/OpenPetra{#ORGNAME}/resources30; permissions: users-full
Name: {userappdata}/OpenPetra{#ORGNAME}/reports30/Settings; permissions: users-full
Name: {userappdata}/OpenPetra{#ORGNAME}/etc30; permissions: users-full
Name: {userappdata}/OpenPetra{#ORGNAME}/tmp30/export; permissions: users-full

[Files]
Source: ..\..\..\csharp\ThirdParty\DevAge\SourceGrid.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\SQLite\Mono.Data.Sqlite.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\SQLite\sqlite3.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\ICSharpCode\ICSharpCode.SharpZipLib.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\GNU\GNU.Gettext.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\Npgsql\Npgsql.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\csharp\ThirdParty\A1Panel\Owf.Controls.A1Panel.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion

Source: ..\..\..\delivery\bin\Ict.Common*dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Client*dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Shared*dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Plugins.*.data.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Petra.Plugins.*.Client.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\PetraClient.exe; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\delivery\bin\Ict.Tools.PatchTool.exe; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; Flags: ignoreversion
Source: ..\..\..\tmp\UINavigation.yml; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30
Source: ..\..\..\delivery\bin\de-DE\OpenPetra.resources.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30/de-DE
Source: ..\..\..\delivery\bin\es-ES\OpenPetra.resources.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30/es-ES
Source: ..\..\..\delivery\bin\da-DK\OpenPetra.resources.dll; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30/da-DK
Source: ..\..\..\XmlReports\Settings\*.xml; DestDir: {userappdata}/OpenPetra{#ORGNAME}/reports30/Settings; Flags: recursesubdirs createallsubdirs
Source: ..\..\..\delivery\PetraClientRemote.config; DestDir: {userappdata}/OpenPetra{#ORGNAME}/etc30
Source: ..\releasenotes\releasenotes*html; DestDir: {userappdata}/OpenPetra{#ORGNAME}/manuals30
Source: ..\..\..\resources\petraico-big.ico; DestDir: {userappdata}/OpenPetra{#ORGNAME}
Source: ..\..\..\resources\*.ico; DestDir: {userappdata}/OpenPetra{#ORGNAME}/resources30
Source: ..\..\..\resources\*.png; DestDir: {userappdata}/OpenPetra{#ORGNAME}/resources30
Source: ..\..\..\LICENSE; DestDir: {userappdata}/OpenPetra{#ORGNAME}
Source: version.txt; DestDir: {userappdata}/OpenPetra{#ORGNAME}/bin30

[Icons]
Name: {group}\{cm:cmIconRemoteLabel}; Filename: {userappdata}/OpenPetra{#ORGNAME}\bin30\PetraClient.exe; WorkingDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; IconFilename: {userappdata}/OpenPetra{#ORGNAME}\petraico-big.ico; Comment: {cm:cmIconRemoteComment}; IconIndex: 0; Parameters: "-C:""{userappdata}/OpenPetra{#ORGNAME}\etc30\PetraClientRemote.config"""
Name: {group}\{cm:cmIconReleaseNotesLabel}; Filename: {userappdata}/OpenPetra{#ORGNAME}\manuals30\{cm:cmReleaseNotesFile}; WorkingDir: {userappdata}/OpenPetra{#ORGNAME}/manuals30; Comment: {cm:cmIconReleaseNotesComment}
Name: {commondesktop}\{groupname}; Filename: {userappdata}/OpenPetra{#ORGNAME}\bin30\PetraClient.exe; WorkingDir: {userappdata}/OpenPetra{#ORGNAME}/bin30; IconFilename: {userappdata}/OpenPetra{#ORGNAME}\petraico-big.ico; Comment: Start OpenPetra.org; IconIndex: 0; Parameters: "-C:""{userappdata}/OpenPetra{#ORGNAME}\etc30\PetraClientRemote.config"""; Tasks: iconDesktop

[Tasks]
Name: iconDesktop; Description: {cm:cmIconTask}

[Run]
Filename: {userappdata}\OpenPetra{#ORGNAME}\manuals30\{cm:cmReleaseNotesFile}; Description: {cm:cmViewReleaseNotes}; Flags: shellexec skipifdoesntexist postinstall skipifsilent

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
begin
  if CurStep=ssPostInstall then
  begin
    ReplaceInTextFile(ExpandConstant('{userappdata}/OpenPetra{#ORGNAME}/etc30/PetraClientRemote.config'), 'PETRAHOST', strServer, true);
    ReplaceInTextFile(ExpandConstant('{userappdata}/OpenPetra{#ORGNAME}/etc30/PetraClientRemote.config'), 'PETRAPORT', IntToStr(NetPort), true);
  end;
end;
