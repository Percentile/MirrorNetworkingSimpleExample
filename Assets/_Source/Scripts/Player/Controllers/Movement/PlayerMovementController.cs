using Mirror;
using UnityEngine;

namespace MirrorTest.Player.Controllers
{
    public class PlayerMovementController : PlayerBaseController
    {
        [SerializeField] 
        private CharacterController _characterController;
        
        [SerializeField] 
        private float _velocity = 10f;

        [SerializeField] 
        private float _fallingSpeed = 10f;
        

        public void FixedUpdate()
        {
            if(!IsEnabled)
                return;
            
            HandleMovement();
            HandleFalling();
        }
        
        
        private void HandleMovement()
        {
            if (!isLocalPlayer) return;

            var inputHorizontal = Input.GetAxis("Horizontal");
            var inputVertical = Input.GetAxis("Vertical");
            
            if(inputHorizontal == 0 && inputVertical == 0)
                return;

            var moveVector = transform.rotation * new Vector3(inputHorizontal, 0, inputVertical).normalized * (_velocity * Time.deltaTime);
            
            _characterController.Move(moveVector);
        }


        private void HandleFalling()
        {
            if(!isLocalPlayer || _characterController.isGrounded)
                return;

            var fallVector = new Vector3(0, -_fallingSpeed, 0) * Time.deltaTime;

            _characterController.Move(fallVector);
        }
    }
}