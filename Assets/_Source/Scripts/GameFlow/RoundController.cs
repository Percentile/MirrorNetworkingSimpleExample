using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using MirrorTest.Player;
using MirrorTest.Player.Controllers;
using UnityEngine;

namespace MirrorTest.GameFlow
{
    public class RoundController : NetworkBehaviour
    {
        [SerializeField] 
        private int _winScore = 3;

        [SerializeField] 
        private int _roundResetDelayMilliseconds = 5000;
        
        [SerializeField] 
        private List<Transform> _spawnPlaces;

        private List<Transform> _availableSpawnPlaces;

        private List<PlayerController> _players;


        public void SpawnPlayer(PlayerController playerController)
        {
            _players.Add(playerController);
            playerController.ScoreController.OnWinAction += OnWinAction;
            ResetPlayer(playerController);
        }


        public void DespawnPlayer(PlayerController playerController)
        {
            _players.Remove(playerController);
        }
        

        private void MovePlayerTo(PlayerController playerController, Transform spawnPlace)
        {
            _availableSpawnPlaces.Remove(spawnPlace);
            playerController.transform.position = spawnPlace.transform.position;
        }


        private void ResetRound()
        {
            _availableSpawnPlaces = _spawnPlaces;

            foreach (var player in _players)
                ResetPlayer(player);
        }

        private void ResetPlayer(PlayerController player)
        {
            MovePlayerTo(player, _availableSpawnPlaces[Random.Range(0, _availableSpawnPlaces.Count - 1)]);
            player.ScoreController.DropScore();
            player.ScoreController.WinScore = _winScore;
        }

        private void OnWinAction(PlayerScoreController playerScore)
        {
            
        }


        private async Task ResetRoundAsync()
        {
            await Task.Delay(_roundResetDelayMilliseconds);
            
            ResetRound();
        }
    }
}