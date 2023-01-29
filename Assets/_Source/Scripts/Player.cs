using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace MirrorTest
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] 
        private float _movementMultiplier = .1f;
        
        public void HandleMovement()
        {
            if (isLocalPlayer)
            {
                var moveHorizontal = Input.GetAxis("Horizontal");
                var moveVertical = Input.GetAxis("Vertical");
                Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0) * _movementMultiplier;
                transform.position += movement;
            }
        }

        public void Update()
        {
            HandleMovement();
        }
    }
}