rem//  $LastChangedDate$
rem//  $Rev$
rem//  $LastChangedBy$
rem//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR21-CommServer/Scripts/create_branch.cmd $
rem//  $Id$
if "%1"=="" goto ERROR
set branchtype=%2
if "%branchtype%"=="" goto setbranch

:dothejob
svn mkdir svn://svnserver.hq.cas.com.pl/VS/%branchtype%/AgileWorkloadTracker/%1  -m "created new AgileWorkloadTracker tag  %1 (create tag folder)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR43-AgileWorkloadTracker svn://svnserver.hq.cas.com.pl/VS/%branchtype%/AgileWorkloadTracker/%1/PR43-AgileWorkloadTracker -m "created new Agile Workload Tracker tag %1 (project PR43-AgileWorkloadTracker)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR44-SharePoint svn://svnserver.hq.cas.com.pl/VS/%branchtype%/AgileWorkloadTracker/%1/PR44-SharePoint -m "created new Agile Workload Tracker tag %1 (project PR44-SharePoint)"

goto EXIT

:setbranch
set branchtype=branches
goto dothejob
:ERROR
echo Parametr must be set
:EXIT
