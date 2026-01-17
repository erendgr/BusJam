using _Game._Dev.Scripts.Runtime.Core.Movement;
using Cysharp.Threading.Tasks;

namespace _Game._Dev.Scripts.Runtime.Core.MovementTracker
{
    public class MovementTracker : IMovementTracker
    {
        private int _activeMovements;

        public void RegisterMovement()
        {
            _activeMovements++;
        }

        public void UnregisterMovement()
        {
            if (_activeMovements > 0)
            {
                _activeMovements--;
            }
        }

        public async UniTask WaitForAllMovementsToComplete()
        {
            await UniTask.WaitUntil(() => _activeMovements == 0);
        }
        
        public void Reset()
        {
            _activeMovements = 0;
        }
    }
}