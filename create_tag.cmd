rem//  $LastChangedDate: 2011-03-31 08:48:24 +0200 (Cz, 31 mar 2011) $
rem//  $Rev: 5725 $
rem//  $LastChangedBy: mzbrzezny $
rem//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR21-CommServer/Scripts/create_tag.cmd $
rem//  $Id: create_tag.cmd 5725 2011-03-31 06:48:24Z mzbrzezny $
if "%1"=="" goto ERROR

./create_branch %1 tags

goto EXIT
:ERROR
echo Parametr must be set
:EXIT
