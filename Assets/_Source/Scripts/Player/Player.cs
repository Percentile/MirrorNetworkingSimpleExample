using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using MirrorTest.Player.Controllers;
using UnityEngine;

namespace MirrorTest.Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] 
        private PlayerMovementController _movementController;
        
        [SerializeField] 
        private PlayerRotateController _rotateController;
        
        [SerializeField] 
        private PlayerDashController _dashController;

        [SerializeField] 
        private PlayerHitController _hitController;

        public void DisableControllers()
        {
            _dashController.IsEnabled = false;
            _movementController.IsEnabled = false;
            _rotateController.IsEnabled = false;
            _hitController.IsEnabled = false;
        }

        public void EnableControllers()
        {
            _dashController.IsEnabled = true;
            _movementController.IsEnabled = true;
            _rotateController.IsEnabled = true;
            _hitController.IsEnabled = true;
        }
    }
}