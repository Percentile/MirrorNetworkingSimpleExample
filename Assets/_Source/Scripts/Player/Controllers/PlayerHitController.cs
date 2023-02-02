using System.Threading.Tasks;
using _Source.Scripts.Util;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace MirrorTest.Player.Controllers
{
    public class PlayerHitController : PlayerBaseController
    {
        [SerializeField] 
        private Renderer _renderer;
        
        [SerializeField] 
        private Material _materialIdle;

        [SerializeField] 
        private Material _materialHit;

        [SerializeField] 
        public int _millisecondsBeingHit;

        private async Task WaitAndDefault()
        {
            await Task.Delay(_millisecondsBeingHit);
            
            IsEnabled = true;
        }

        public void OnHit()
        {
            if(IsEnabled == false)
                return;
            
            IsEnabled = false;

            WaitAndDefault().FireAndForget();
        }

        public void ChangeMaterial()
        {
            _renderer.material = IsEnabled ? _materialIdle : _materialHit;
        }
    }
}