using Cysharp.Threading.Tasks;

namespace _Game._Dev.Scripts.Runtime.Core.Movement
{
    public interface IMovementTracker
    {
        void RegisterMovement();
        void UnregisterMovement();
        UniTask WaitForAllMovementsToComplete();
        void Reset();
    }
}