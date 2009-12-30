[Setup]
AppCopyright=by developers of OpenPetra.org
AppName=OpenPetra.org
AppVerName=OpenPetra.org Remote {#PATCHVERSION}
DefaultDirName={pf}\OpenPetra.org
DefaultGroupName=OpenPetra.org
AppPublisherURL=http://www.openpetra.org
LicenseFile=..\..\..\LICENSE
VersionInfoVersion={#RELEASEID}
VersionInfoCompany=OM International
VersionInfoDescription=Administration Software for Charities
VersionInfoCopyright=2009 OM International
OutputBaseFilename=OpenPetraRemoteSetup-{#RELEASEVERSION}

[Languages]
Name: en; MessagesFile: compiler:Default.isl,..\language\lang-en.isl
Name: de; MessagesFile: compiler:Languages\German.isl,..\language\lang-de.isl

[Dirs]
Name: {app}/bin30
Name: {app}/bin30/locale/de/LC_MESSAGES
Name: {app}/manuals30
Name: {app}/reports30
Name: {app}/resources30
Name: {userappdata}/OpenPetra.org/tmp30
Name: {userappdata}/OpenPetra.org/reports30
Name: {userappdata}/OpenPetra.org/etc30

[Files]
Source: ..\..\..\csharp\ThirdParty\DevAge\SourceGrid.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\DevAge\SourceGrid.Extensions.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\DevAge\DevAge.Core.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\DevAge\DevAge.Windows.Forms.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\SQLite\System.Data.SQLite.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\ICSharpCode\ICSharpCode.SharpZipLib.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\Mono\intl.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\Mono\Mono.Posix.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\Mono\Mono.Security.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\Mono\MonoPosixHelper.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\Npgsql\Npgsql.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\gtk-sharp\pango-sharp.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\gtk-sharp\atk-sharp.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\gtk-sharp\gdk-sharp.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\gtk-sharp\glib-sharp.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\gtk-sharp\gtk-sharp.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\gtk-sharp\iconv.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ThirdParty\gtk-sharp\libxml2.dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ICT\Petra\Client\_bin\Release\Ict.Common*dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ICT\Petra\Client\_bin\Release\Ict.Petra.Client*dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ICT\Petra\Shared\_bin\Server_Client\Release\Ict.Petra.Shared*dll; DestDir: {app}/bin30
Source: ..\..\..\csharp\ICT\Petra\Client\_bin\Release\PetraClient.exe; DestDir: {app}/bin30
Source: ..\..\..\csharp\ICT\Petra\Definitions\UINavigation.yml; DestDir: {app}/bin30
Source: ..\i18n\*.mo; DestDir: {app}/bin30; Flags: recursesubdirs createallsubdirs
Source: ..\..\..\XmlReports\reports.dtd; DestDir: {app}/reports30
Source: ..\..\..\XmlReports\*.xml; DestDir: {app}/reports30
Source: PetraClientRemote.config; DestDir: {app}/etc30; DestName: PetraClient-Remote.config
Source: ..\releasenotes\releasenotes*html; DestDir: {app}/manuals30
Source: ..\..\..\resources\petraico-big.ico; DestDir: {app}
Source: ..\..\..\resources\*.ico; DestDir: {app}/resources30
Source: ..\..\..\resources\*.png; DestDir: {app}/resources30
Source: ..\..\..\LICENSE; DestDir: {app}
Source: version.txt; DestDir: {app}/bin30
Source: ..\..\..\csharp\ICT\Testing\secretkey.txt; DestDir: {app}/etc30; DestName: secretkey.dat

[Icons]
Name: {group}\{cm:cmIconRemoteLabel}; Filename: {app}\bin30\PetraClient.exe; WorkingDir: {app}/bin30; IconFilename: {app}\petraico-big.ico; Comment: {cm:cmIconRemoteComment}; IconIndex: 0; Parameters: "-C:""{app}\etc30\PetraClient-Remote.config"" -AutoLogin:demo"
Name: {group}\{cm:cmIconReleaseNotesLabel}; Filename: {app}\manuals30\{cm:cmReleaseNotesFile}; WorkingDir: {app}/manuals30; Comment: {cm:cmIconReleaseNotesComment}
Name: {commondesktop}\{cm:cmIconRemoteLabel}; Filename: {app}\bin30\PetraClient.exe; WorkingDir: {app}/bin30; IconFilename: {app}\petraico-big.ico; Comment: Start OpenPetra.org; IconIndex: 0; Parameters: "-C:""{app}\etc30\PetraClient-Remote.config"" -AutoLogin:demo"; Tasks: iconDesktop

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
    strServer := 'LINUX';
    NetPort := 9000;
    PetraServerConnectionPage := CreatePage_PetraServerConnection(wpPreparing, strServer, NETPort);
    DotNetPage := CreatePage_MissingNetFrameWork(PetraServerConnectionPage.Id);
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
  else if (CurPageID = PetraServerConnectionPage.ID) then
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
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClient-Remote.config'), 'Petra.PathTemp" value="TOREPLACE"', 'Petra.PathTemp" value="{userappdata}/OpenPetra.org/tmp30"', true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClient-Remote.config'), 'Petra.Path.Patches" value="TOREPLACE"', 'Petra.Path.RemotePatches" value="{app}/bin30/patches"', true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClient-Remote.config'), 'Petra.Path.RemotePatches" value="TOREPLACE"', 'Petra.Path.RemotePatches" value="http://www.example.org/OpenPetraPatches/"', true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClient-Remote.config'), 'Reporting.PathReportSettings" value="TOREPLACE"', 'Reporting.PathReportSettings" value="{userappdata}/OpenPetra.org/reports30"', true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClient-Remote.config'), 'PETRAHOST', strServer, true);
    ReplaceInTextFile(ExpandConstant('{app}/etc30/PetraClient-Remote.config'), 'PETRAPORT', IntToStr(NetPort), true);
    ReplaceInTextFile(ExpandConstant('{app}/bin30/version.txt'), 'ReleaseVersion', '#RELEASEVERSION', true);
  end;

  // allow the .net remoting communication between client and server
  Exec(ExpandConstant('{sys}\cmd.exe'), '/C netsh firewall set allowedprogram program = '
    + ExpandConstant('"{app}\bin30\PetraClient.exe" name = PetraClient mode = DISABLE'),
    '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;
