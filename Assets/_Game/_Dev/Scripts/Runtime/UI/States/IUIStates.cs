namespace _Game._Dev.Scripts.Runtime.UI.States
{
    public interface IUIState
    {
        void Enter(object payload = null);
        void Exit();
    }
}