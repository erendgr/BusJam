using System;
using _Game._Dev.Scripts.Runtime.Core.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.UI.Views
{
    public class GameplayUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI timerText;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<LevelReadySignal>(OnLevelReady);
            _signalBus.Subscribe<TimerUpdatedSignal>(OnTimerUpdated);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<LevelReadySignal>(OnLevelReady);
            _signalBus.TryUnsubscribe<TimerUpdatedSignal>(OnTimerUpdated);
        }

        private void OnLevelReady(LevelReadySignal signal)
        {
            levelText.text = $"LEVEL {signal.LevelData.LevelIndex + 1}";
        }

        private void OnTimerUpdated(TimerUpdatedSignal signal)
        {
            var time = TimeSpan.FromSeconds(signal.RemainingTime);
            timerText.text = $"{time.Minutes:00}:{time.Seconds:00}";
        }
    }
}