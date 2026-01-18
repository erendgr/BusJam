namespace _Game._Dev.Scripts.Runtime.Core.GameplayState
{
    public interface IGameplayStateHolder
    {
        bool IsGameplayActive { get; }
        void Pause();
        void Resume();
    }
}