namespace _Game._Dev.Scripts.Runtime.Core.Events
{
    public struct RestartLevelRequestedSignal{ }

    public struct StartGameRequestedSignal { }

    public struct NextLevelRequestedSignal { }

    public struct LevelReadySignal
    {
        public readonly LevelSO LevelData;
        public LevelReadySignal(LevelSO levelData) => LevelData = levelData;
    }

    public struct TimerUpdatedSignal
    {
        public readonly float RemainingTime;
        public TimerUpdatedSignal(float remainingTime) => RemainingTime = remainingTime;
    }
}