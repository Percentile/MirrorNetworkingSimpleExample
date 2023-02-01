using Mirror;
using UnityEngine;

namespace MirrorTest.Player.Controllers
{
    public class PlayerRotateController : PlayerBaseController
    {
        [SerializeField] 
        private CameraController _camera;
        
        [SerializeField] 
        private float _mouseSensivity = 1f;


        public void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            _camera.SetActive(isLocalPlayer);
        }

        public void Update()
        {
            if(IsEnabled)
                HandleRotate();
        }

        private void HandleRotate()
        {
            if(!isLocalPlayer)
                return;

            var rotateX = Input.GetAxis("Mouse Y") * _mouseSensivity;
            var rotateY = Input.GetAxis("Mouse X") * _mouseSensivity;

            _camera.RotateVertical(rotateX);
            
            RotateHorizontal(rotateY);
        }


        private void RotateHorizontal(float rotateY)
        {
            var localEulerAngles = transform.localEulerAngles;
            
            localEulerAngles = new(
                0,
                localEulerAngles.y + rotateY, 
                0);
            
            transform.localEulerAngles = localEulerAngles;
        }
    }
}