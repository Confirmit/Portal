:: Restoring database backup
:: Run it with file name without extension as an argument, ie:
::    dbrestore FluentMigration-2012-06-21-21-48-43
::    dbrestore CleanBackup
:: Note that only \backup folder files are supported
call "%VS120COMNTOOLS%\..\..\VC\vcvarsall.bat" x86 
%FrameworkDir%\v4.0.30319\msbuild.exe /property:Configuration=Release;RestoreFileName="%~1" MsBuild\ProjectBuilder.msbuild /target:Restore
