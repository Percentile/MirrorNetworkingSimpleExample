using System;
using System.Threading;
using System.Threading.Tasks;
using _Source.Scripts.Util;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace MirrorTest.Player.Controllers
{
    public class PlayerDashController : PlayerBaseController
    {
        [SerializeField] 
        private CharacterController _characterController;
        
        [SerializeField] 
        private float _dashVelocity = 100f;

        [SerializeField] 
        private float _dashDistance = 100f;

        private CancellationTokenSource _moveCts = new CancellationTokenSource();

        [SerializeField]
        public UnityEvent OnDashStartEvent;
        
        [SerializeField]
        public UnityEvent OnDashEndEvent;

        private void Dash()
        {
            _moveCts = new CancellationTokenSource();
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task.Factory.StartNew(async () => await DashAsync(), CancellationToken.None, TaskCreationOptions.None, taskScheduler);
        }

        private async Task DashAsync()
        {
            OnDashStartEvent.Invoke();

            var direction = transform.rotation * Vector3.forward;

            var target = transform.localPosition + direction * _dashDistance;

            var distanceVector = target - transform.localPosition;

            var velocityVector = direction * _dashVelocity;

            while (distanceVector.magnitude > .01f)
            {
                if (_moveCts.Token.IsCancellationRequested)
                {
                    Debug.LogWarning($"Cancellation Requested");
                    break;
                }

                if(Math.Sign(distanceVector.x) * Math.Sign(direction.x) < 0 || Math.Sign(distanceVector.y) * Math.Sign(direction.y) < 0 )
                    break;

                _characterController.Move(velocityVector * Time.deltaTime);
                
                distanceVector = target - transform.localPosition;
                
                await Task.Delay(10);
            }
            
            Debug.LogWarning($"OnDashEndEvent");

            OnDashEndEvent.Invoke(); 
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if(hit.collider.gameObject.layer == 7)
                _moveCts.Cancel();
            
            
        }

        private void Update()
        {
            if(!isLocalPlayer || !Input.GetMouseButtonDown(0))
                return;
            
            Dash();
        }
    }
}