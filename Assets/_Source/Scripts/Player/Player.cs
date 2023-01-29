using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace MirrorTest.Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] 
        private CameraController _camera;
        
        [SerializeField] 
        private float _movementMultiplier = .1f;
        
        [SerializeField] 
        private float _mouseSensivity = 1f;


        public void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
            _camera.SetActive(isLocalPlayer);
        }
        
        private void HandleMovement()
        {
            if (!isLocalPlayer) return;

            var moveHorizontal = Input.GetAxis("Horizontal");
            var moveVertical = Input.GetAxis("Vertical");
                
            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical) * _movementMultiplier;
            transform.localPosition += movement;
        }

        private void HandleRotate()
        {
            if(!isLocalPlayer)
                return;

            var rotateX = Input.GetAxis("Mouse Y") * _mouseSensivity;
            var rotateY = Input.GetAxis("Mouse X") * _mouseSensivity;

            _camera.Rotate(rotateX, 0);
            
            Rotate(rotateY);
        }


        private void Rotate(float rotateY)
        {
            var localEulerAngles = transform.localEulerAngles;
            
            localEulerAngles = new(
                0,
                localEulerAngles.y + rotateY, 
                0);
            
            transform.localEulerAngles = localEulerAngles;
        }

        public void Update()
        {
            HandleMovement();
            HandleRotate();
        }
    }
}