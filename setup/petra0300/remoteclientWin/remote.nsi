;NSIS Modern User Interface
;Remote installer for OpenPetra which can be compiled on Linux
;Written by Timotheus Pokorra

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  ;Name and file
  Name "OpenPetra Remote"
  OutFile "OpenPetra-RemoteSetup.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\OpenPetra.Remote"

  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\OpenPetra.Remote" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Language Selection Dialog Settings

  ;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
  !define MUI_LANGDLL_REGISTRY_KEY "Software\OpenPetra.Remote" 
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"
  
;--------------------------------
;Pages

  !define MUI_WELCOMEPAGE_TITLE_3LINES
  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "..\..\..\LICENSE"
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH

  !define MUI_WELCOMEPAGE_TITLE_3LINES
  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages
!define MUI_TEXT_WELCOME_INFO_TITLE "Welcome to the $(^NameDA) Setup Assistant"
!define MUI_TEXT_WELCOME_INFO_TEXT "This assistant will guide you through the installation of $(^NameDA).$\r$\n$\r$\nIt is recommended that you close all other applications before starting Setup. This will make it possible to update relevant system files without having to reboot your computer.$\r$\n$\r$\n$_CLICK"

!define MUI_UNTEXT_WELCOME_INFO_TITLE "Welcome to the $(^NameDA) Uninstall Assistant"
!define MUI_UNTEXT_WELCOME_INFO_TEXT "This assistant will guide you through the uninstallation of $(^NameDA).$\r$\n$\r$\nBefore starting the uninstallation, make sure $(^NameDA) is not running.$\r$\n$\r$\n$_CLICK"

!define MUI_TEXT_FINISH_INFO_TITLE "Completing the $(^NameDA) Setup Assistant"
!define MUI_TEXT_FINISH_INFO_TEXT "$(^NameDA) has been installed on your computer.$\r$\n$\r$\nClick Finish to close this assistant."

!define MUI_UNTEXT_FINISH_INFO_TITLE "Completing the $(^NameDA) Uninstall Assistant"
!define MUI_UNTEXT_FINISH_INFO_TEXT "$(^NameDA) has been uninstalled from your computer.$\r$\n$\r$\nClick Finish to close this assistant."

  !insertmacro MUI_LANGUAGE "English"
  !insertmacro MUI_LANGUAGE "German"
  

;--------------------------------
;Installer Sections

Section "Main Section" SecInstallFiles

  CreateDirectory "$INSTDIR\bin30"
  SetOutPath "$INSTDIR\bin30"

  File ..\..\..\csharp\ICT\Petra\Client\_bin\Release\Ict.Common*dll

  ; give the patchtool a chance to update the files
  ; http://nsis.sourceforge.net/AccessControl_plug-in
  AccessControl::GrantOnFile "$INSTDIR\bin30" "(BU)" "FullAccess"
    
  ;Store installation folder
  WriteRegStr HKCU "Software\OpenPetra.Remote" "" $INSTDIR

  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

;--------------------------------
;Installer Functions

Function .onInit

  !insertmacro MUI_LANGDLL_DISPLAY

FunctionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  Delete "$INSTDIR\bin30\*.dll"
  
  RMDir "$INSTDIR\bin30"

  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\OpenPetra.Remote"

SectionEnd
