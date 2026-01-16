using _Game._Dev.Scripts.Runtime.Features.Bus.Controllers;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers;
using Cysharp.Threading.Tasks;

namespace _Game._Dev.Scripts.Runtime.Core.BoardingSystem
{
    public interface IBoardingSystem
    {
        UniTask<bool> TryBoard(IPassengerController passenger, IBusController bus);
    }
}