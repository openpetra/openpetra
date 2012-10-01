[Setup]
AppName=NAnt
AppVerName=NAnt 0.92
DefaultDirName={pf}\NAnt
DefaultGroupName=NAnt
LicenseFile=nant-0.92\COPYING.txt
OutputBaseFilename=NAnt-Setup-0.92

[Files]
Source: nant-0.92\*.*; DestDir: {app}; Flags: recursesubdirs createallsubdirs
Source: nantcontrib-0.92\bin\*.*; DestDir: {app}\bin\extensions\common\neutral\nantcontrib; Flags: recursesubdirs createallsubdirs

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep=ssPostInstall then
  begin
    SaveStringToFile(ExpandConstant('{win}/nant.bat'), ExpandConstant('@"{app}\bin\NAnt.exe" %*'), False);
  end;
end;

