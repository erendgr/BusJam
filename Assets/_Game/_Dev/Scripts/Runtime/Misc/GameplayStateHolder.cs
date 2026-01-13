namespace _Game._Dev.Scripts.Runtime.Misc
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