using Mirror;
using MirrorTest.GameFlow;
using MirrorTest.Player;
using UnityEngine;

namespace MirrorTest.Network
{
    public class NetworkManagerExpanded : NetworkManager
    {
        [SerializeField] 
        public RoundController RoundController;
    }
}