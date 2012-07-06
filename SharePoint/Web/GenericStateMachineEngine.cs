using System;

namespace CAS.SharePoint.Web
{
  internal abstract class GenericStateMachineEngine
  {
    #region public
    public GenericStateMachineEngine() { }
    [Flags]
    internal enum ControlsSet
    {
      SaveOn = 0x01, EditOn = 0x02, CancelOn = 0x04, NewOn = 0x08,
      DeleteOn = 0x10, EditModeOn = 0x20
    }
    internal enum InterfaceEvent { SaveClick, EditClick, CancelClick, NewClick, NewData };
    internal enum InterfaceState { ViewState, EditState, NewState }
    internal class ActionResult
    {
      #region public
      internal ActionResult(Exception _excptn, string _src)
      {
        _excptn.Source += " at " + _src;
        ActionException = _excptn;
        LastActionResult = Result.Exception;
      }
      internal enum Result { Success, NotValidated, Exception }
      internal Result LastActionResult { get; private set; }
      internal Exception ActionException { get; private set; }
      internal bool ActionSucceeded { get { return LastActionResult == Result.Success; } }
      internal static ActionResult Success { get { return new ActionResult(Result.Success); } }
      internal static ActionResult NotValidated(string _msg) 
      {
        return new ActionResult(Result.NotValidated) { ActionException = new ApplicationException(_msg) };  
      }
      #endregion

      #region private
      private ActionResult(Result _rslt)
      {
        LastActionResult = _rslt;
      }
      #endregion
    }
    #endregion

    #region Event Handlers
    internal void NewButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          CurrentMachineState = InterfaceState.NewState;
          ClearUserInterface();
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.NewClick);
          break;
      }
    }
    internal void SaveButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.EditState:
          ActionResult _ur = Update();
          switch (_ur.LastActionResult)
          {
            case ActionResult.Result.Success:
              CurrentMachineState = InterfaceState.ViewState;
              break;
            case ActionResult.Result.NotValidated:
            case ActionResult.Result.Exception:
              ShowActionResult(_ur);
              break;
          }
          break;
        case InterfaceState.NewState:
          ActionResult _cr = this.Create();
          switch (_cr.LastActionResult)
          {
            case ActionResult.Result.Success:
              CurrentMachineState = InterfaceState.ViewState;
              break;
            case ActionResult.Result.NotValidated:
              ShowActionResult(_cr);
              break;
            case ActionResult.Result.Exception:
              ClearUserInterface();
              ShowActionResult(_cr);
              CurrentMachineState = InterfaceState.ViewState;
              break;
            default:
              break;
          }
          break;
        case InterfaceState.ViewState:
        default:
          SMError(InterfaceEvent.SaveClick);
          break;
      };
    }
    internal void CancelButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.NewState:
        case InterfaceState.EditState:
          Show();
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.ViewState:
        default:
          SMError(InterfaceEvent.CancelClick);
          break;
      }
    }
    internal void EditButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          CurrentMachineState = InterfaceState.EditState;
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.EditClick);
          break;
      }
    }
    internal void DeleteButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          ActionResult _dr = Delete();
          switch (_dr.LastActionResult)
          {
            case ActionResult.Result.Success:
              ClearUserInterface();
              break;
            case ActionResult.Result.NotValidated:
            case ActionResult.Result.Exception:
              ShowActionResult(_dr);
              break;
          }
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.EditClick);
          break;
      }
    }
    #endregion

    #region private

    #region Actions
    protected abstract ActionResult Show();
    protected abstract ActionResult Update();
    protected abstract ActionResult Create();
    protected abstract ActionResult Delete();
    protected abstract void ClearUserInterface();
    protected abstract void SetEnabled(ControlsSet _buttons);
    protected abstract void SMError(InterfaceEvent interfaceEvent);
    protected virtual void ShowActionResult(ActionResult _rslt)
    {
      
      //if (_rslt.LastActionResult != ActionResult.Result.Exception)
      //  return;
      //Anons _entry = new Anons(_rslt.ActionException.Source, _rslt.ActionException.Message);
      //using (EntitiesDataContext _EDC = new EntitiesDataContext(SPContext.Current.Web.Url))
      //{
      //  _EDC.EventLogList.InsertOnSubmit(_entry);
      //  _EDC.SubmitChanges();
      //}
    }
    #endregion

    protected abstract InterfaceState CurrentMachineState { get; set; }
    protected void EnterState()
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          SetEnabled(ControlsSet.EditOn | ControlsSet.NewOn | ControlsSet.DeleteOn);
          break;
        case InterfaceState.EditState:
          SetEnabled(ControlsSet.CancelOn | ControlsSet.SaveOn | ControlsSet.EditModeOn);
          break;
        case InterfaceState.NewState:
          SetEnabled(ControlsSet.CancelOn | ControlsSet.SaveOn | ControlsSet.EditModeOn);
          break;
      }
    }
    #endregion
  }
}
