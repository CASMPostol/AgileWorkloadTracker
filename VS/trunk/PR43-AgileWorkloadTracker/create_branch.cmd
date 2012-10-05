rem//  $LastChangedDate: 2011-04-04 09:42:00 +0200 (Pn, 04 kwi 2011) $
rem//  $Rev: 5816 $
rem//  $LastChangedBy: mzbrzezny $
rem//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR21-CommServer/Scripts/create_branch.cmd $
rem//  $Id: create_branch.cmd 5816 2011-04-04 07:42:00Z mzbrzezny $
if "%1"=="" goto ERROR
set branchtype=%2
if "%branchtype%"=="" goto setbranch

:dothejob
svn mkdir svn://svnserver.hq.cas.com.pl/VS/%branchtype%/AgileWorkloadTracker/%1  -m "created new AgileWorkloadTracker tag  %1 (create tag folder)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR43-AgileWorkloadTracker svn://svnserver.hq.cas.com.pl/VS/%branchtype%/AgileWorkloadTracker/%1/PR43-AgileWorkloadTracker -m "created new Agile Workload Tracker tag %1 (project PR43-AgileWorkloadTracker)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR44-SharePoint svn://svnserver.hq.cas.com.pl/VS/%branchtype%/SmartFactory/%1/PR44-SharePoint -m "created new Agile Workload Tracker tag %1 (project PR44-SharePoint)"

goto EXIT

:setbranch
set branchtype=branches
goto dothejob
:ERROR
echo Parametr must be set
:EXIT
