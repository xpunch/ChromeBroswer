!include 'logiclib.nsh'

Function GetNetFrameworkVersion
    ;获取.Net Framework版本支持
    Push $1
    Push $0
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Version"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client" "Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client" "Version"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "Version"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "InstallSuccess"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "Version"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Version"
    StrCmp $1 "" +1 +2
    StrCpy $1 "2.0.50727.832"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Version"
    StrCmp $1 "" +1 +2
    StrCpy $1 "1.1.4322.573"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0" "Install"
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0" "Version"
    StrCmp $1 "" +1 +2
    StrCpy $1 "1.0.3705.0"
    StrCmp $0 1 KnowNetFrameworkVersion +1
    StrCpy $1 "not .NetFramework"
    KnowNetFrameworkVersion:
    Pop $0
    Exch $1
FunctionEnd

Function InstallVCRedist
Push $R0
ClearErrors
ReadRegDword $R0 HKCR "Installer\Dependencies\{935adb67-500c-4c55-ad88-4fe31cfc4f91}" "Version"
${If} $R0 < '12'
  File "/oname=$TEMP\vcredist2013_x86.exe" "..\Dependencies\vcredist2013_x86.exe"
  ExecWait '"$TEMP\vcredist2013_x86.exe" /q' # silent install
  Delete "$TEMP\vcredist2013_x86.exe"
${EndIf}
Exch $R0
FunctionEnd

Function CheckNetFramework
RetryCheckNet:
  Call GetNetFrameworkVersion
  Pop $1
  ${If} $1 < '4.0.30319'
  SetDetailsPrint both
  DetailPrint "检测到未安装 .NET framework 4.0，请先下载安装.NET framework 4.0."
  MessageBox MB_RETRYCANCEL|MB_ICONSTOP "检测到未安装 .NET framework 4.0，请先下载安装.NET framework 4.0." IDRETRY RetryCheckNet
  DetailPrint "安装被取消."
  SetDetailsPrint listonly
  Abort "安装已取消."
  ${EndIf}
FunctionEnd
