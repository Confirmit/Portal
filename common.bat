call "%VS120COMNTOOLS%\..\..\VC\vcvarsall.bat" x86 
%FrameworkDir%\v4.0.30319\msbuild.exe /property:Configuration=Release MsBuild\ProjectBuilder.msbuild /target:Run
