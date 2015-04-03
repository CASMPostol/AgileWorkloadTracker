
rem//  $LastChangedDate: 2013-08-29 13:53:38 +0200 (Thu, 29 Aug 2013) $
rem//  $Rev: 9645 $
rem//  $LastChangedBy: mpostol $
rem//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR42-SmartFactory/Scripts/CreateTag.tt $
rem//  $Id: CreateTag.tt 9645 2013-08-29 11:53:38Z mpostol $


set branchtype=tags
set TagFolder=Rel_2_00_00
set TagPath=svn://svnserver.hq.cas.com.pl/VS/%branchtype%/SmartFactory/rel_2_00_00
set trunkPath=svn://svnserver.hq.cas.com.pl/VS/trunk

svn mkdir %TagPath%  -m "created new %TagPath% (in %branchtype% folder)"

svn copy %trunkPath%/PR43-AgileWorkloadTracker %TagPath%/PR43-AgileWorkloadTracker -m "created copy in %TagPath% of the PR43-AgileWorkloadTracker"
svn copy %trunkPath%/PR39-CommonResources %TagPath%/PR39-CommonResources -m "created copy in %TagPath% of the project PR39-CommonResources"
svn copy %trunkPath%/ImageLibrary %TagPath%/ImageLibrary -m "created copy in %TagPath% of the project ImageLibrary"
svn copy %trunkPath%/PR44-SharePoint %TagPath%/PR44-SharePoint -m "created copy in %TagPath% of the project PR44-SharePoint"

pause ....

