namespace _Game._Dev.Scripts.Runtime.Misc
{
    public interface IGameplayStateHolder
    {
        bool IsGameplayActive { get; }
        void Pause();
        void Resume();
    }
}