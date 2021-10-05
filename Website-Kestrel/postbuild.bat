@REM bash -c "rsync -rvuz 'bin/Release/net5.0/' 'debian@192.168.1.111:~/Website-Kestrel'"
@REM C:\Windows\System32\bash.exe -c "echo hello"








echo %PROCESSOR_ARCHITECTURE%

@REM if exist C:\Windows\Sysnative\bash.exe (
@REM   start "bash.exe" /D "%cd%" /B /wait "C:\Windows\Sysnative\bash.exe" %*
@REM ) else (
@REM   start "bash.exe" /D "%cd%" /B /wait "C:\Windows\System32\bash.exe" %*
@REM )

set outDir=%1
set "outDir=%outDir:\=/%"
echo outDir: %outDir%

echo ProjectName: %2
set ProjectName=%2

echo TargetFileName %3
set TargetFileName=%3

echo TargetName %4
set TargetName=%4

@REM Kill any process that are running
C:\Windows\Sysnative\bash.exe -c "ssh 'debian@192.168.1.111' -f 'pkill %TargetName%'"

@REM Send the files to the IP
C:\Windows\Sysnative\bash.exe -c "rsync -rvuz '%outDir%' 'debian@192.168.1.111:~/%2'"

@REM Run the process
C:\Windows\Sysnative\bash.exe -c "ssh 'debian@192.168.1.111' -f '/home/debian/%ProjectName%/linux-arm/%TargetName% </dev/null &>/dev/null &'"

@REM NOTES
@REM # Postbuild
@REM bash -c "rsync -rvuz $(OutDir) debian@192.168.1.111:~/$(ProjectName)"

@REM # Terminal output
@REM Executing task: cmd /c "dotnet publish -r linux-arm -o bin\linux-arm\publish C:\Workspace\BeagleBone-Test\Website-Kestrel\Website-Kestrel.csproj" && 
@REM bash -c "rsync -rvuz $(wslpath 'C:\Workspace\BeagleBone-Test\Website-Kestrel')/bin/linux-arm/publish/ debian@192.168.1.111:~/Website-Kestrel" <


@REM # Working in CMD
@REM bash -c "rsync -rvuz 'bin/Release/net5.0/' 'debian@192.168.1.111:~/Website-Kestrel'"

