using System.Threading;
using _Game._Dev.Scripts.Runtime.Features.Bus.Controllers;
using _Game._Dev.Scripts.Runtime.Level.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game._Dev.Scripts.Runtime.Core.Factories
{
    public interface IBusFactory
    {
        UniTask<IBusController> Create(Colors color, Vector3 arrivalPosition, CancellationToken cancellationToken);
        IBusController CreateAtPosition(Colors color, Vector3 position);
    }
}