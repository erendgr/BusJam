using _Game._Dev.Scripts.Runtime.Features.Bus.Controllers;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using Cysharp.Threading.Tasks;

namespace _Game._Dev.Scripts.Runtime.Core.BoardingSystem
{
    public class BoardingSystem : IBoardingSystem
    {
        public async UniTask<bool> TryBoard(IPassengerController passenger, IBusController bus)
        {
            if (!bus.CanBoard(passenger.View.PassengerColor))
                return false;

            var slotIndex = bus.ReserveSlot();
            var slotTransform = bus.View.GetSlotTransform(slotIndex);

            try
            {
                await passenger.View.MoveToPoint(slotTransform.position);
            }
            catch (System.OperationCanceledException)
            {
                // do nothing because the game is already over
            }
            
            if (passenger.View != null)
            {
                passenger.View.transform.SetParent(slotTransform);
            }

            return true;
        }
    }
}