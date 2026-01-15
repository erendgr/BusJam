using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Core.Movement;
using _Game._Dev.Scripts.Runtime.Features.Passenger.Models;
using _Game._Dev.Scripts.Runtime.Level.Models;
using _Game._Dev.Scripts.Runtime.Misc;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.Features.Passenger.Views
{
    [RequireComponent(typeof(Collider))]
    public class PassengerView : MonoBehaviour
    {
        private bool _interactionEnabled;
        public Colors PassengerColor { get; private set; }
        
        [Header("References")] 
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private PassengerSettingsSO passengerSettings;
        
        private SignalBus _signalBus;
        private IMovementTracker _movementTracker;
        private MaterialPropertyBlock _materialPropertyBlock;
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        [Inject]
        public void Construct(SignalBus signalBus, IMovementTracker movementTracker)
        {
            _signalBus = signalBus;
            _movementTracker = movementTracker;
        }

        public void Initialize(Colors color)
        {
            PassengerColor = color;
            SetVisualColor(color);
        }

        private void OnMouseDown()
        {
            if(_interactionEnabled) return;
            _signalBus.Fire(new PassengerClickedSignal(this));
        }

        private void SetVisualColor(Colors color)
        {
            _materialPropertyBlock.SetColor(ColorProperty, ColorMapper.GetColorFromEnum(color));
            meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        public async UniTask MoveAlongPath(List<Vector2Int> path)
        {
            _movementTracker.RegisterMovement();
            try
            {
                var sequence = DOTween.Sequence();
                float totalDuration = passengerSettings.MoveDuration;
                float durationPerSegment = path.Count > 1 ? totalDuration / (path.Count - 1) : totalDuration;

                for (var i = 1; i < path.Count; i++)
                {
                    var gridPos = path[i];
                    var worldPosition = new Vector3(gridPos.x, transform.position.y, gridPos.y);
                    sequence.Append(transform.DOMove(worldPosition, durationPerSegment).SetEase(Ease.Linear));
                }
                await sequence.Play().ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            finally
            {
                _movementTracker.UnregisterMovement();
            }
        }

        public async UniTask MoveToPoint(Vector3 worldPosition)
        {
            _movementTracker.RegisterMovement();
            try
            {
                await transform.DOMove(worldPosition, passengerSettings.MoveDuration)
                    .SetEase(Ease.OutQuad)
                    .ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            finally
            {
                _movementTracker.UnregisterMovement();
            }
        }
        
        public void SetInteractionEnabled(bool isEnabled)
        {
            _interactionEnabled = isEnabled;
        }
    }
}