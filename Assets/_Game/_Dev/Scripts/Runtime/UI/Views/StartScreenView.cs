using _Game._Dev.Scripts.Runtime.Core.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.UI.Views
{
    public class StartScreenView : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            startButton.onClick.AddListener(OnStartButtonPressed);
        }

        private void OnStartButtonPressed()
        {
            _signalBus.Fire<StartGameRequestedSignal>();
        }
        
        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(OnStartButtonPressed);
        }
    }
}