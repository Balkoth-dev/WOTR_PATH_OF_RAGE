@echo off
setlocal EnableDelayedExpansion

set "filePath=%~dp0Output\Info.json"

for /f "tokens=2 delims=:," %%a in ('findstr /i /c:"\"Version\"" "%filePath%"') do (
    set "currentVersion=%%~a"
)

for /f "tokens=1-3 delims=." %%a in ("%currentVersion%") do (
    set /a "major=%%a, minor=%%b, build=%%c+1"
)

if !build! equ 10 (
    set /a "build=0, minor+=1"
)

if !minor! equ 10 (
    set /a "minor=0, major+=1"
)

set "newVersion=!major!.!minor!.!build!"
echo New version: !newVersion!

powershell -Command "$json = Get-Content -Path '%filePath%' -Raw | ConvertFrom-Json; $json.Version = '%newVersion%'; $json | ConvertTo-Json -Depth 100 | Set-Content -Path '%filePath%'"

if not !errorlevel! gtr 0 (
    echo Failed to update version.
    exit /b !errorlevel!
)

echo Info.json file updated successfully.
