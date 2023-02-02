﻿using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MirrorTest.Player.Controllers
{
    public class PlayerScoreController : NetworkBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _playerNameText;
        
        
        [SerializeField] 
        private TextMeshProUGUI _scoreText;
        
        
        public int WinScore = 3;
        
        
        public string PlayerName { get; private set; }


        [SyncVar(hook = nameof(OnScoreChange))] 
        private int _score;
        

        public int Score => _score;

        
        public UnityAction<PlayerScoreController> OnWinAction;


        public void SetName(string name)
        {
            PlayerName = name;
            _playerNameText.text = name;
        }

        
        public void IncrementScore()
        {
            _score++;
        }


        public void DropScore()
        {
            _score = 0;
        }


        public void OnScoreChange(int oldVal, int newVal)
        {
            _scoreText.text = $"{_score}";
            
            if(_score >= WinScore)
                OnWinAction?.Invoke(this);
        }
    }
}