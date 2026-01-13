using _Game._Dev.Scripts.Runtime.Level.Models;

namespace _Game._Dev.Scripts.Runtime.Core.Grid
{
    public interface IGridSystemManager
    {
        IGrid MainGrid { get; }
        IGrid WaitingAreaGrid { get; }
        void CreateGrids(LevelSO levelData); 
        void Reset();
    }
}