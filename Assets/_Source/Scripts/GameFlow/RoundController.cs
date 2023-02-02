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

        private bool _isRoundFinished;


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
        

        [ClientRpc]
        private void MovePlayerTo(PlayerController player, Transform spawnPlace)
        {
            if(spawnPlace == null)
                return;
            
            player.transform.position = spawnPlace.transform.position;
        }

        
        private void ResetRound()
        {
            ResetWinner();
            SetRoundFinished(false);
            _availableSpawnPlaces = new(_spawnPlaces);

            foreach (var player in _players)
                ResetPlayer(player);
        }
        
        
        private void ResetPlayer(PlayerController player)
        {
            var spawnPlace = _availableSpawnPlaces?[Random.Range(0, _availableSpawnPlaces.Count - 1)];
            MovePlayerTo(player, spawnPlace);
            _availableSpawnPlaces?.Remove(spawnPlace);
            
            player.ScoreController.DropScore();
            player.ScoreController.SetWinScore(_winScore);
        }
        
        
        [Command(requiresAuthority = false)]
        private void OnWinAction(PlayerScoreController playerScore)
        {
            if(_isRoundFinished)
                return;

            SetRoundFinished(true);
            
            ShowWinner(playerScore.PlayerName);
            
            ResetRoundAsync().FireAndForget();
        }

        
        [ClientRpc]
        private void SetRoundFinished(bool isFinished)
            => _isRoundFinished = isFinished;
        

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