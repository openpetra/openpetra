[Setup]
AppName=NAnt
AppVerName=NAnt 0.91
DefaultDirName={pf}\NAnt
DefaultGroupName=NAnt
LicenseFile=nant-0.91\COPYING.txt
OutputBaseFilename=NAnt-Setup-0.91

[Files]
Source: nant-0.91\*.*; DestDir: {app}; Flags: recursesubdirs createallsubdirs

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep=ssPostInstall then
  begin
    SaveStringToFile(ExpandConstant('{win}/nant.bat'), ExpandConstant('@"{app}\bin\NAnt.exe" %*'), False);
  end;
end;

