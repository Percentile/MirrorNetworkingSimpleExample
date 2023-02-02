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

        [SerializeField]
        public UnityEvent OnHitOtherPlayerEvent;

        private void Dash()
        {
            _moveCts.Cancel();
            _moveCts = new CancellationTokenSource();
            
            DashAsync().FireAndForget();
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
                    break;

                // Проверяем, не уехали ли слишком далеко
                if(Math.Sign(distanceVector.x) * Math.Sign(direction.x) < 0 || Math.Sign(distanceVector.y) * Math.Sign(direction.y) < 0 )
                    break;

                _characterController.Move(velocityVector * Time.deltaTime);

                var oldDistanceVector = distanceVector;
                
                distanceVector = target - transform.localPosition;
                
                if(distanceVector == oldDistanceVector)
                    break;
                
                await Task.Delay(10);
            }

            OnDashEndEvent.Invoke(); 
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            switch (hit.collider.gameObject.layer)
            {
                case 7:
                    _moveCts.Cancel();
                    break;
                case 6:
                {
                    if(!IsEnabled)
                        HitOtherPlayer(hit.collider.gameObject.GetComponent<PlayerHitController>());
                    break;
                }
            }
        }

        [Command]
        private void HitOtherPlayer(PlayerHitController hitController)
        {
            if(hitController.IsEnabled)
                OnHitOtherPlayerEvent.Invoke();
            
            hitController.OnHit();
        }

        private void Update()
        {
            if(!isLocalPlayer || !Input.GetMouseButtonDown(0))
                return;
            
            Dash();
        }
    }
}