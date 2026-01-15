using _Game._Dev.Scripts.Runtime.Features.Passenger.Models;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Views;

namespace _Game._Dev.Scripts.Runtime.Features.Passenger.Controllers
{
    public class PassengerController : IPassengerController
    {
        private readonly PassengerView _view;
        private readonly PassengerModel _model;

        public PassengerView View => _view;
        public PassengerModel Model => _model;

        public PassengerController(PassengerView view, PassengerModel model)
        {
            _view = view;
            _model = model;
        }
    }
}