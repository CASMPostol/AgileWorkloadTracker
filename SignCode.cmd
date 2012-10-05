Echo on
call "c:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x64
Echo on
Echo signing \Release\CAS.SmartFactorySetup.msi
signtool sign /a /n CAS ..\Release\CAS.SmartFactorySetup.msi
Echo signing \Release\Setup.exe
signtool sign /a /n CAS ..\Release\Setup.exe
pause ...