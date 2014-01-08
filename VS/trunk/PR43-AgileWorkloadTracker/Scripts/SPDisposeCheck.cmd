
set path="C:\Program Files (x86)\Microsoft\SharePoint Dispose Check"
set dashbordsPath="C:\vs\Projects\SmartFactory\PR42-SmartFactory\Shepherd\ShepherdDashboards\bin\Release"
set DataModelPath="C:\vs\Projects\SmartFactory\PR42-SmartFactory\Shepherd\DataModel\bin\Release"
set WorkflowsPath="C:\vs\Projects\SmartFactory\PR42-SmartFactory\Shepherd\Workflows\bin\Release"

%path%\SPDisposeCheck %dashbordsPath%\CAS.ShepherdDashboards.dll >%dashbordsPath%\CAS.ShepherdDashboards.dclog.txt
%path%\SPDisposeCheck %DataModelPath%\CAS.ShepherdDataModel.dll >%DataModelPath%\CAS.ShepherdDataModel.dclog.txt
%path%\SPDisposeCheck %WorkflowsPath%\CAS.SmartFactorySendNotification.dll >%WorkflowsPath%\CAS.SmartFactorySendNotification.dclog.txt

pause...