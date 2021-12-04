using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.Pong
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private TextMeshProUGUI _oponentNameText;
        [SerializeField] private TextMeshProUGUI _playerScoreText;
        [SerializeField] private TextMeshProUGUI _oponentScoreText;
        [SerializeField] private GameObject _lossScreen;

        [SerializeField] private GameObject _winScreen;
        private int _score = 0;
        private int _oponentScore = 0;

        public void SetupPlayer(Player player)
        {
            if(player.hasAuthority)
            {
                _playerNameText.text = player.PlayerName;
            }
            else
            {
                _oponentNameText.text = player.PlayerName;
            }
        }

        private void Start()
        {
            PlayerConnection.OnPlayerDead += GameOver;
        }

        private void OnDestroy()
        {
            PlayerConnection.OnPlayerDead -= GameOver;
        }

        private void GameOver(Player player)
        {
            Debug.Log("GAME OVER");
            if(player.hasAuthority)
            {
                _score++;
                _playerScoreText.text = _score.ToString();

                if(_score > _oponentScore)
                {
                    _playerScoreText.color = Color.green;
                }
                else if(_score == _oponentScore)
                {
                    _playerScoreText.color = Color.yellow;
                }
                else
                {
                    _playerScoreText.color = Color.red;
                }
            }
            else
            {
                _oponentScore++;
                _oponentScoreText.text = _oponentScore.ToString();

                if(_score < _oponentScore)
                {
                    _oponentScoreText.color = Color.green;
                }
                else if(_score == _oponentScore)
                {
                    _oponentScoreText.color = Color.yellow;
                }
                else
                {
                    _oponentScoreText.color = Color.red;
                }
            }

            if(_score == 10)
            {
                _winScreen.SetActive(true);
                StartCoroutine(CloseGame());
            }
            else if(_oponentScore == 10)
            {
                _lossScreen.SetActive(true);
                StartCoroutine(CloseGame());
            }

            player.SpawnPosition();
        }

        private IEnumerator CloseGame()
        {
            yield return new WaitForSeconds(5);
            Application.Quit();
        }
    }
}