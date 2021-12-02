using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.Pong
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private TextMeshProUGUI _gameOverText;
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
            _gameOverScreen.SetActive(true);

            if(player.hasAuthority)
            {
                _gameOverText.text = "YOU LOST!";
            }
            else
            {
                _gameOverText.text = "YOU WON!";
            }

            player.SpawnPosition();
            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(2);
            _gameOverScreen.SetActive(false);
        }
    }
}