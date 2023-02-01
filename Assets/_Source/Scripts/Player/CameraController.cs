using UnityEngine;

namespace MirrorTest.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] 
        private Camera _camera;

        [SerializeField] 
        private float _minVerticalAngle = 0;

        [SerializeField] 
        private float _maxVerticalAngle = 0;

        public void SetActive(bool isActive)
            => _camera.enabled = isActive;

        public void RotateVertical(float rotateX)
        {
            var localEulerAngles = transform.localEulerAngles;
            
            localEulerAngles = new(
                Mathf.Clamp(localEulerAngles.x - rotateX, _minVerticalAngle, _maxVerticalAngle) * Time.deltaTime,
                0, 
                0);
            
            transform.localEulerAngles = localEulerAngles;
        }
    }
}