using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Source.Scripts.Util;
using Mirror;
using MirrorTest.Player;
using MirrorTest.Player.Controllers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MirrorTest.GameFlow
{
    public class RoundController : NetworkBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _winnerText;
        
        [SerializeField] 
        private int _winScore = 3;

        [SerializeField] 
        private int _roundResetDelayMilliseconds = 5000;
        
        [SerializeField] 
        private List<Transform> _spawnPlaces;

        private List<Transform> _availableSpawnPlaces;

        private readonly SyncList<PlayerController> _players = new();


        private void Start()
        {
            _availableSpawnPlaces = _spawnPlaces;
        }


        public void ApplySpawnPlayer(PlayerController playerController)
        {
            _players.Add(playerController);
            playerController.ScoreController.OnWinAction += OnWinAction;
            playerController.OnDestroyAction += ApplyDespawnPlayer;
            ResetPlayer(playerController);
        }


        public void ApplyDespawnPlayer(PlayerController playerController)
        {
            _players.Remove(playerController);
        }
        

        private void MovePlayerTo(PlayerController player, Transform spawnPlace)
        {
            if(spawnPlace == null)
                return;
            
            _availableSpawnPlaces.Remove(spawnPlace);
            
            player.transform.position = spawnPlace.transform.position;
        }

        
        private void ResetRound()
        {
            ResetWinner();
            _availableSpawnPlaces = _spawnPlaces;

            foreach (var player in _players)
                ResetPlayer(player);
        }
        
        
        [ClientRpc]
        private void ResetPlayer(PlayerController player)
        {
            MovePlayerTo(player, _availableSpawnPlaces?[Random.Range(0, _availableSpawnPlaces.Count - 1)]);
            player.ScoreController.DropScore();
            player.ScoreController.WinScore = _winScore;
        }
        
        
        private void OnWinAction(PlayerScoreController playerScore)
        {
            ShowWinner(playerScore.PlayerName);
            
            ResetRoundAsync().FireAndForget();
        }

        [ClientRpc]
        private void ShowWinner(string name)
        {
            _winnerText.text = $"{name} WINS";
        }


        [ClientRpc]
        private void ResetWinner()
        {
            _winnerText.text = "";
        }


        private async Task ResetRoundAsync()
        {
            await Task.Delay(_roundResetDelayMilliseconds);
            
            ResetRound();
        }
    }
}