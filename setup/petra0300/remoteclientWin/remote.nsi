;NSIS Modern User Interface
;Remote installer for OpenPetra which can be compiled on Linux
;Written by Timotheus Pokorra

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"
  !include LogicLib.nsh
;--------------------------------
;General

!define ORGNAME "{#ORGNAME}"
!define VERSION "{#RELEASEVERSION}"
!define PRODUCT_WEB_SITE "http://{#PUBLISHERURL}"

!define MUI_PRODUCT "OpenPetra.org ${ORGNAME}"

  ;Name and file
  Name "${MUI_PRODUCT}"
  OutFile "{#DELIVERY.DIR}/OpenPetraRemoteSetup-${ORGNAME}-${VERSION}.exe"
  BrandingText "by developers of OpenPetra.org"


  ;Default installation folder
  InstallDir "$APPDATA\OpenPetra${ORGNAME}"

  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\OpenPetra${ORGNAME}" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel user

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Language Selection Dialog Settings

  ;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
  !define MUI_LANGDLL_REGISTRY_KEY "Software\OpenPetra${ORGNAME}" 
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
; we do not want to use the word wizard, but prefer assistant!
!define MUI_TEXT_WELCOME_INFO_TITLE "Welcome to the $(^NameDA) Setup Assistant"
!define MUI_TEXT_WELCOME_INFO_TEXT "This assistant will guide you through the installation of $(^NameDA).$\r$\n$\r$\nIt is recommended that you close all other applications before starting Setup. This will make it possible to update relevant system files without having to reboot your computer.$\r$\n$\r$\n$_CLICK"

!define MUI_UNTEXT_WELCOME_INFO_TITLE "Welcome to the $(^NameDA) Uninstall Assistant"
!define MUI_UNTEXT_WELCOME_INFO_TEXT "This assistant will guide you through the uninstallation of $(^NameDA).$\r$\n$\r$\nBefore starting the uninstallation, make sure $(^NameDA) is not running.$\r$\n$\r$\n$_CLICK"

!define MUI_TEXT_FINISH_INFO_TITLE "Completing the $(^NameDA) Setup Assistant"
!define MUI_TEXT_FINISH_INFO_TEXT "$(^NameDA) has been installed on your computer.$\r$\n$\r$\nClick Finish to close this assistant."

!define MUI_UNTEXT_FINISH_INFO_TITLE "Completing the $(^NameDA) Uninstall Assistant"
!define MUI_UNTEXT_FINISH_INFO_TEXT "$(^NameDA) has been uninstalled from your computer.$\r$\n$\r$\nClick Finish to close this assistant."

; TODO: several languages
  !insertmacro MUI_LANGUAGE "English"
;  !insertmacro MUI_LANGUAGE "German"
 
;--------------------------------
;Installer Sections

Section "Main Section" SecInstallFiles
  Call CheckDotNETVersion

  CreateDirectory "$INSTDIR\tmp30"
  CreateDirectory "$INSTDIR\patches30"
  CreateDirectory "$INSTDIR\bin30"
  SetOutPath "$INSTDIR\bin30"
  File ..\..\..\csharp\ThirdParty\DevAge\SourceGrid.dll
  File ..\..\..\csharp\ThirdParty\SQLite\Mono.Data.Sqlite.dll
  File ..\..\..\csharp\ThirdParty\SQLite\sqlite3.dll
  File ..\..\..\csharp\ThirdParty\ICSharpCode\ICSharpCode.SharpZipLib.dll
  File ..\..\..\csharp\ThirdParty\GNU\GNU.Gettext.dll
  File ..\..\..\csharp\ThirdParty\Npgsql\Npgsql.dll
  File ..\..\..\csharp\ThirdParty\A1Panel\Owf.Controls.A1Panel.dll
  File ..\..\..\csharp\ThirdParty\OrientedTextControls\CustomControl.OrientedTextControls.dll
  File ..\..\..\delivery\bin\Ict.Common*dll
  File ..\..\..\delivery\bin\Ict.Petra.Client*dll
  File ..\..\..\delivery\bin\Ict.Petra.Plugins.*.data.dll
  File ..\..\..\delivery\bin\Ict.Petra.Plugins.*.Client.dll
  File ..\..\..\delivery\bin\Ict.Petra.Shared*dll
  File ..\..\..\delivery\bin\PetraClient.exe
  File ..\..\..\delivery\bin\Ict.Tools.PatchTool.exe
  File ..\..\..\delivery\bin\Ict.Tools.PatchTool.Library.dll
  File ..\..\..\tmp\UINavigation.yml
  SetOutPath "$INSTDIR\bin30\de-DE"
  File ..\..\..\delivery\bin\de-DE\OpenPetra.resources.dll
  SetOutPath "$INSTDIR\bin30\es-ES"
  File ..\..\..\delivery\bin\es-ES\OpenPetra.resources.dll
  SetOutPath "$INSTDIR\bin30\da-DK"
  File ..\..\..\delivery\bin\da-DK\OpenPetra.resources.dll
  SetOutPath "$INSTDIR\reports30\Settings"
  File ..\..\..\XmlReports\Settings\*.xml
  SetOutPath "$INSTDIR\etc30"
  File ..\..\..\tmp\PetraClientRemote.config
  SetOutPath "$INSTDIR\manuals30"
  File ..\releasenotes\releasenotes*html
  SetOutPath "$INSTDIR\resources30"
  File ..\..\..\resources\*.ico
  File ..\..\..\resources\*.png
  SetOutPath "$INSTDIR\bin30"
  File ..\..\..\tmp\version.txt
  SetOutPath "$INSTDIR"
  File ..\..\..\LICENSE
  File ..\..\..\resources\petraico-big.ico
  
  ;Store installation folder
  WriteRegStr HKCU "Software\OpenPetra${ORGNAME}" "" $INSTDIR

  ; Now create shortcuts
  CreateDirectory "$SMPROGRAMS\${MUI_PRODUCT}"
  SetOutPath "$INSTDIR\bin30"
  CreateShortCut "$SMPROGRAMS\${MUI_PRODUCT}\OpenPetra.org Client.lnk" "$INSTDIR\bin30\PetraClient.exe" '-C:"$INSTDIR\etc30\PetraClientRemote.config"' $INSTDIR\petraico-big.ico 0 SW_SHOWNORMAL
  ; avoid problems with empty hotkey. so no comment for the moment for the shortcut: "Start OpenPetra.org (connecting to your OpenPetra server)"
  CreateShortCut "$SMPROGRAMS\${MUI_PRODUCT}\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
 
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
SectionEnd

;--------------------------------
;Installer Functions

Function .onInit

  !insertmacro MUI_LANGDLL_DISPLAY

FunctionEnd
;--------------------------------

; see http://nsis.sourceforge.net/Check_for_a_Registry_Key
!macro IfKeyExists ROOT MAIN_KEY KEY
  Push $R0
  Push $R1
  Push $R2
 
  # XXX bug if ${ROOT}, ${MAIN_KEY} or ${KEY} use $R0 or $R1
 
  StrCpy $R1 "0" # loop index
  StrCpy $R2 "0" # not found
 
  ${Do}
    EnumRegKey $R0 ${ROOT} "${MAIN_KEY}" "$R1"
    ${If} $R0 == "${KEY}"
      StrCpy $R2 "1" # found
      ${Break}
    ${EndIf}
    IntOp $R1 $R1 + 1
  ${LoopWhile} $R0 != ""
 
  ClearErrors
 
  Exch 2
  Pop $R0
  Pop $R1
  Exch $R2
!macroend

; see also http://nsis.sourceforge.net/Get_.NET_Version and http://nsis.sourceforge.net/DotNET
; GetCORVersion always returns v2.0, but not the installed v3.5
; we don't want to install .Net directly, since the user might not have admin permissions
Function CheckDotNETVersion
    !insertmacro IfKeyExists "HKLM" "SOFTWARE\Microsoft\.NETFramework\Policy" "v4.0"
    Pop $R0
    ;$R0 contains 0 (not present) or 1 (present)

  ${If} $R0 == 0
    DetailPrint ".NET Framework v4.0 not installed."
    DetailPrint "Please first install this .NET Framework version from www.microsoft.com!"
    MessageBox MB_OK|MB_ICONSTOP \
    ".NET Framework v4.0 not installed.$\nPlease first install this .NET Framework version from www.microsoft.com!"
    Abort
  ${EndIf}
FunctionEnd

;Uninstaller Section

Section "Uninstall"

  Delete "$INSTDIR\bin30\*.dll"
  Delete "$INSTDIR\bin30\de-DE\*.dll"
  Delete "$INSTDIR\bin30\es-ES\*.dll"
  Delete "$INSTDIR\bin30\da-DK\*.dll"
  Delete "$INSTDIR\bin30\*.exe"
  Delete "$INSTDIR\bin30\*.yml"
  Delete "$INSTDIR\bin30\version.txt"
  Delete "$INSTDIR\etc30\*.config"
  Delete "$INSTDIR\reports30\Settings\*.xml"
  Delete "$INSTDIR\manuals30\*.html"
  Delete "$INSTDIR\resources30\*.png"
  Delete "$INSTDIR\resources30\*.ico"
  Delete "$INSTDIR\*.ico"
  Delete "$INSTDIR\LICENSE"
  
  RMDir "$INSTDIR\bin30\de-DE"
  RMDir "$INSTDIR\bin30\es-ES"
  RMDir "$INSTDIR\bin30\da-DK"
  RMDir "$INSTDIR\bin30"
  RMDir "$INSTDIR\reports30\Settings"  
  RMDir "$INSTDIR\reports30"
  RMDir "$INSTDIR\resources30"
  RMDir "$INSTDIR\etc30"
  RMDir "$INSTDIR\manuals30"
  RMDir "$INSTDIR\tmp30"
  RMDir "$INSTDIR\patches30"
  
  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"
  
  ;Delete Start Menu Shortcuts
  Delete "$DESKTOP\${MUI_PRODUCT}.lnk"
  Delete "$SMPROGRAMS\${MUI_PRODUCT}\*.*"
  RmDir  "$SMPROGRAMS\${MUI_PRODUCT}"  

  DeleteRegKey /ifempty HKCU "Software\OpenPetra${ORGNAME}"

SectionEnd
