using System;

namespace CAS.SharePoint.Web
{
  public abstract class GenericStateMachineEngine
  {
    #region public
    public GenericStateMachineEngine() { }
    [Flags]
    public enum ControlsSet
    {
      SaveOn = 0x01, EditOn = 0x02, CancelOn = 0x04, NewOn = 0x08,
      DeleteOn = 0x10, EditModeOn = 0x20
    }
    public enum InterfaceEvent { SaveClick, EditClick, CancelClick, NewClick, NewData };
    public enum InterfaceState { ViewState, EditState, NewState }
    public class ActionResult
    {
      #region public
      public enum Result { Success, NotValidated, Exception }
      public Result LastActionResult { get; private set; }
      public Exception ActionException { get; private set; }
      public bool ActionSucceeded { get { return LastActionResult == Result.Success; } }
      public static ActionResult Exception(Exception _excptn, string _src)
      {
        _excptn.Source += " at " + _src;
        return new ActionResult(Result.Exception) { ActionException = _excptn };
      }
      public static ActionResult Success { get { return new ActionResult(Result.Success); } }
      public static ActionResult NotValidated(string _msg)
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
    public void NewButton_Click(object sender, EventArgs e)
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
    public void SaveButton_Click(object sender, EventArgs e)
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
    public void CancelButton_Click(object sender, EventArgs e)
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
    public void EditButton_Click(object sender, EventArgs e)
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
    public void DeleteButton_Click(object sender, EventArgs e)
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
    protected abstract void ShowActionResult(ActionResult _rslt);
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
