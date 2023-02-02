using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace MirrorTest.Player.Controllers
{
    public abstract class PlayerBaseController : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnIsEnabledChange))]
        public bool IsEnabled = true;

        [SerializeField] 
        public UnityEvent OnEnableEvent;
        
        [SerializeField] 
        public UnityEvent OnDisableEvent;

        private void OnIsEnabledChange(bool oldVal, bool newVal)
        {
            if(newVal)
                OnEnableEvent.Invoke();
            else
                OnDisableEvent.Invoke();
        }
    }
}