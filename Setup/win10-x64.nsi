;---------------------------------------------------------------------------------------------------------------------
; Setup Script for nsis compiler
; See Examples\Modern UI\Basic.nsi

;----------------------------------------------------------
; Definitions

  !define APP_NAME "osFotoFix"
  !define MAIN_APP_EXE "osFotoFix.exe"
  ;!define VERSION "0.3.3"
  !define PROD_VERSION "${VERSION}"
  !define FILE_VERSION "${VERSION}"
  !define COMP_NAME "Seidel-IT"
  !define COPYRIGHT "OS, 2020"

  !define INSTALLER_NAME "win10-x64-setup/osFotoFix_v${VERSION}_setup.exe"
  !define LICENSE_TEXT "../osFotoFix/License.txt"
  !define REG_ROOT "HKLM"
  !define REG_APP_PATH "Software\Microsoft\Windows\CurrentVersion\App Paths\${MAIN_APP_EXE}"
  !define UNINSTALL_PATH "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"
  !define REG_START_MENU "Start Menu Folder" 

  var SM_Folder

;----------------------------------------------------------
; General

  
  ; Name and file
  Name "${APP_NAME}"
  Caption "${APP_NAME}"
  OutFile "${INSTALLER_NAME}"
  BrandingText "${APP_NAME}"

	; Default installer folder
  InstallDir "$PROGRAMFILES32\Seidel-IT\osFotoFix"

	; Get installation folder from registry if available from last installation
	InstallDirRegKey "${REG_ROOT}" "${REG_APP_PATH}" ""

;----------------------------------------------------------
; Version Information

  VIProductVersion "${VERSION}.0"
  VIAddVersionKey "ProductName" "${APP_NAME}"
  VIAddVersionKey "CompanyName" "${COMP_NAME}"
  VIAddVersionKey "ProductVersion" "${PROD_VERSION}"
  VIAddVersionKey "FileVersion" "${FILE_VERSION}"
  VIAddVersionKey "FileDescription" "${APP_NAME} Setup"
  VIAddVersionKey "LegalCopyright" "${COpYRIGHT}"
  ;VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "OS 2019"

;----------------------------------------------------------
; Include Modern UI

  !include "MUI2.nsh"
  !define MUI_ABORTWARNING
  !define MUI_UNABORTWARNING

;----------------------------------------------------------
; Pages

  !insertmacro MUI_PAGE_WELCOME

  !ifdef LICENSE_TEXT
    !insertmacro MUI_PAGE_LICENSE "${LICENSE_TEXT}"
  !endif

  ;!insertmacro MUI_PAGE_COMPONENTS

  !insertmacro MUI_PAGE_DIRECTORY
  
  !ifdef REG_START_MENU
    !define MUI_STARTMENUPAGE_DEFAULTFOLDER "${COMP_NAME}"
    !define MUI_STARTMENUPAGE_REGISTRY_ROOT "${REG_ROOT}"
    !define MUI_STARTMENUPAGE_REGISTRY_KEY "${UNINSTALL_PATH}"
    !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${REG_START_MENU}"
    !insertmacro MUI_PAGE_STARTMENU Application $SM_Folder    
  !endif

  !insertmacro MUI_PAGE_INSTFILES

  !insertmacro MUI_PAGE_FINISH
  
  !insertmacro MUI_UNPAGE_CONFIRM

  !insertmacro MUI_UNPAGE_INSTFILES

  !insertmacro MUI_UNPAGE_FINISH

  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section "Install"

  ;ADD YOUR OWN FILES HERE...
  ;SetOutPath "$INSTDIR\Help"	
  ;File "..\..\Deployment${SUFFIX}\Help\*.*"

  SetOutPath "$INSTDIR"
  File "win10-x64-deployment\*.*"

  !ifdef REG_START_MENU
    !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    CreateDirectory "$SMPROGRAMS\$SM_Folder"
    CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
    CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
    CreateShortCut "$SMPROGRAMS\$SM_Folder\Uninstall ${APP_NAME}.lnk" "$INSTDIR\uninstall.exe"

    !ifdef WEB_SITE
      WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
      CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
    !endif
    !insertmacro MUI_STARTMENU_WRITE_END
  !endif

  WriteRegStr ${REG_ROOT} "${REG_APP_PATH}" "" "$INSTDIR\${MAIN_APP_EXE}"
  WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayName" "${APP_NAME}"
  WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "UninstallString" "$INSTDIR\uninstall.exe"
  WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayIcon" "$INSTDIR\${MAIN_APP_EXE}"
  WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayVersion" "${VERSION}"
  WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "Publisher" "${COMP_NAME}"

  !ifdef WEB_SITE
    WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "URLInfoAbout" "${WEB_SITE}"
  !endif

  WriteUninstaller "$INSTDIR\uninstall.exe"

SectionEnd

;Section "Install Section" SecDummy

  ;Store installation folder
;  WriteRegStr HKCU "Software\as1ComaMan${SUFFIX}" "Install_Dir" $INSTDIR

  ; Write the uninstall keys for Windows
;  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\as1ComaMan${SUFFIX}" "DisplayName" "as1ComaMan"
;  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\as1ComaMan${SUFFIX}" "UninstallString" '"$INSTDIR\uninstall.exe"'
;  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\as1ComaMan${SUFFIX}" "NoModify" 1
;  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\as1ComaMan${SUFFIX}" "NoRepair" 1

  ;Create uninstaller
;  WriteUninstaller "$INSTDIR\Uninstall.exe"

;SectionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ;ADD YOUR OWN FILES HERE...
  Delete "$INSTDIR\*.*"

  !ifdef WEB_SITE
    Delete "$INSTDIR\${APP_NAME} website.url"
  !endif

  RMDir "$INSTDIR"

  !ifdef REG_START_MENU
    !insertmacro MUI_STARTMENU_GETFOLDER "Application" $SM_Folder
    Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk"
    Delete "$SMPROGRAMS\$SM_Folder\Uninstall ${APP_NAME}.lnk"

    !ifdef WEB_SITE
      Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk"
    !endif
    Delete "$DESKTOP\${APP_NAME}.lnk"

    RmDir "$SMPROGRAMS\$SM_Folder"
  !endif

  DeleteRegKey ${REG_ROOT} "${REG_APP_PATH}"
  DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}"

SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
;  LangString DESC_SecDummy ${LANG_ENGLISH} "A test section."

  ;Assign language strings to sections
;  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
;    !insertmacro MUI_DESCRIPTION_TEXT ${SecDummy} $(DESC_SecDummy)
;  !insertmacro MUI_FUNCTION_DESCRIPTION_END

