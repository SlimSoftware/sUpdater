mkdir "%appdata%\Slim Software\sUpdater"
copy /y "%appdata%\Slim Software\Slim Updater\Settings.xml" "%appdata%\Slim Software\sUpdater\settings.xml" 
rmdir "%appdata%\Slim Software\Slim Updater" /s /q
rmdir "%ProgramFiles%\Slim Software\Slim Updater" /s /q
pause