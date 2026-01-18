namespace _Game._Dev.Scripts.Runtime.Core.GameplayState
{
    public class GameplayStateHolder : IGameplayStateHolder
    {
        public bool IsGameplayActive { get; private set; } = true;

        public void Pause()
        {
            IsGameplayActive = false;
        }

        public void Resume()
        {
            IsGameplayActive = true;
        }
    }
}