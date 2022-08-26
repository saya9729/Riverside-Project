using UnityEngine;

namespace GameUI
{
    public class PauseGame : MonoBehaviour
    {
        private Player.InputManager _inputManager;
        [SerializeField] private GameObject gameMenuCanvas;

        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if (_inputManager.menu)
            {
                PauseTheGame();
                _inputManager.menu = false;
            }
        }

        void Initialize()
        {
            _inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Player.InputManager>();
        }
        public void PauseTheGame()
        {
            if (_inputManager)
            {
                _inputManager._playerInput.DeactivateInput();
            }
            gameMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState =  CursorLockMode.None;
        }


        public void UnPauseTheGame()
        {
            if (_inputManager)
            {
                _inputManager._playerInput.ActivateInput();
            }
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
