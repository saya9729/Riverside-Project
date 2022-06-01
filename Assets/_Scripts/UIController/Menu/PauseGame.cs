using UnityEngine;
using System;

namespace GameUI
{
    public class PauseGame : MonoBehaviour
    {
        private Player.InputManager _inputManager;
        [SerializeField] private GameObject gameMenuCanvas;
        [SerializeField] private GameObject gameUICanvas;
        [SerializeField] private GameObject gameOptionMenuCanvas;
        [SerializeField] private GameObject GameMenuButtonCanvas;
        private bool _gameIsPause = false;
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if (_inputManager.menu)
            {
                TogglePauseGame();
                _inputManager.menu = false;
            }
        }

        void Initialize()
        {
            _inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Player.InputManager>();
        }
        public void TogglePauseGame()
        {
            _gameIsPause = !_gameIsPause;
            gameMenuCanvas.SetActive(_gameIsPause);
            gameUICanvas.SetActive(!_gameIsPause);
            gameOptionMenuCanvas.SetActive(false);
            GameMenuButtonCanvas.SetActive(_gameIsPause);
            Time.timeScale = _gameIsPause ? 0f : 1f;
            Cursor.lockState = _gameIsPause ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
