using System.Collections.Generic;
using _Game._Dev.Scripts.Runtime.Core.Events;
using _Game._Dev.Scripts.Runtime.Core.Movement;
using _Game._Dev.Scripts.Runtime.Level.Models;
using _Game._Dev.Scripts.Runtime.Misc;
using _Game._Dev.Scripts.Runtime.MVC.Passenger.Models;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Game._Dev.Scripts.Runtime.MVC.Passenger.Views
{
    [RequireComponent(typeof(Collider))]
    public class PassengerView : MonoBehaviour
    {
        public bool IsMoving { get; set; }
        public Colors Color { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        
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

        public void Initialize(Colors color, Vector2Int gridPosition)
        {
            Color = color;
            GridPosition = gridPosition;
            SetVisualColor(color);
        }

        private void OnMouseDown()
        {
            if(IsMoving) return;
            _signalBus.Fire(new CharacterClickedSignal(this));
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

        public void UpdateGridPosition(Vector2Int newPosition)
        {
            GridPosition = newPosition;
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
    }
}