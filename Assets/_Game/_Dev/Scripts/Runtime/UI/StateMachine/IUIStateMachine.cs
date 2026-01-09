namespace _Game._Dev.Scripts.Runtime.UI.StateMachine
{
    public interface IUIStateMachine
    {
        void ChangeState<T>(object payload = null) where T : States.IUIState;
    }
}