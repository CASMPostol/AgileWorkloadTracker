rem//  $LastChangedDate$
rem//  $Rev$
rem//  $LastChangedBy$
rem//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR21-CommServer/Scripts/create_tag.cmd $
rem//  $Id$
if "%1"=="" goto ERROR

./create_branch %1 tags

goto EXIT
:ERROR
echo Parametr must be set
:EXIT
