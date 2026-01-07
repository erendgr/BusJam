using _Game._Dev.Scripts.Runtime.Core.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.UI.Views
{
    public class EndScreenView : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;
        
        [Header("Buttons")]
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button restartButton;
        
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            nextLevelButton.onClick.AddListener(OnNextLevelPressed);
            restartButton.onClick.AddListener(OnRestartPressed);
        }

        private void OnDisable()
        {
            nextLevelButton.onClick.RemoveListener(OnNextLevelPressed);
            restartButton.onClick.RemoveListener(OnRestartPressed);
        }

        public void Show(bool didWin)
        {
            winPanel.SetActive(didWin);
            losePanel.SetActive(!didWin);
            nextLevelButton.gameObject.SetActive(didWin);
            restartButton.gameObject.SetActive(!didWin);
        }
        
        private void OnNextLevelPressed()
        {
            Debug.Log("Next Level button pressed");
            _signalBus.Fire<NextLevelRequestedSignal>();
        }

        private void OnRestartPressed()
        {
            Debug.Log("Restart button pressed");
            _signalBus.Fire<RestartLevelRequestedSignal>();
        }
    }
}