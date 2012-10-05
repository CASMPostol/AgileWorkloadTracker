#rem
Echo on
call "c:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x64
Echo on
signtool sign /a /n CAS %1 >sign.log
xcopy /y %1 %2 >sign.log


