using Mirror;
using UnityEngine;

namespace MirrorTest.Player.Controllers
{
    public abstract class PlayerBaseController : NetworkBehaviour
    {
        public bool IsEnabled = true;
    }
}