using Mirror;
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
        
        
        private int _winScore = 3;


        [SyncVar(hook = nameof(SetNameText))]
        public string PlayerName;


        [SyncVar(hook = nameof(OnScoreChange))] 
        private int _score;
        

        public int Score => _score;

        
        public UnityAction<PlayerScoreController> OnWinAction;


        public void SetNameText(string _, string name)
        {
            _playerNameText.text = name;
        }

        
        public void IncrementScore()
        {
            _score++;
        }

        [ClientRpc]
        public void DropScore()
        {
            _score = 0;
        }


        [ClientRpc]
        public void SetWinScore(int winScore)
        {
            _winScore = winScore;
        }


        public void OnScoreChange(int oldVal, int newVal)
        {
            _scoreText.text = $"{_score}";
            
            if(_score >= _winScore)
                OnWinAction?.Invoke(this);
        }
    }
}