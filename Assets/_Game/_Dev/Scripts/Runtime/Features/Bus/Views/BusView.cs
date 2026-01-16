using System.Collections.Generic;
using System.Threading;
using _Game._Dev.Scripts.Runtime.Features.Bus.Models;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Features.Bus.Views
{
    public class BusView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MeshRenderer busBodyRenderer;
        [SerializeField] private List<Transform> passengerSlots;
        [SerializeField] private BusSettingsSO busSettings;

        private MaterialPropertyBlock _materialPropertyBlock;
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        public void SetColor(Color color)
        {
            _materialPropertyBlock.SetColor(ColorProperty, color);
            busBodyRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        public Transform GetSlotTransform(int index)
        {
            return (index >= 0 && index < passengerSlots.Count) ? passengerSlots[index] : null;
        }

        public async UniTask AnimateDeparture(CancellationToken cancellationToken)
        {
            await transform.DOMove(transform.position + Vector3.right * busSettings.MoveOffset, busSettings.MoveDuration)
                .SetEase(Ease.InCubic)
                .ToUniTask(cancellationToken: cancellationToken); 
        }
        
        public async UniTask AnimateToStopPosition(Vector3 targetPosition, CancellationToken cancellationToken)
        {
            await transform.DOMove(targetPosition, busSettings.MoveDuration)
                .SetEase(Ease.OutBack)
                .ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask AnimateArrival(Vector3 targetPosition, CancellationToken cancellationToken)
        {
            transform.position = targetPosition + Vector3.left * busSettings.MoveOffset;
            await transform.DOMove(targetPosition, busSettings.MoveDuration)
                .SetEase(Ease.OutCubic)
                .ToUniTask(cancellationToken: cancellationToken);
        }
    }
}