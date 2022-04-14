using UnityEngine;
using System;

public class PauseGame : MonoBehaviour
{
    private Player.InputManager _inputManager;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameMenu gameMenu;
    private bool _gameIsPause = false;
    private GameMenu _gameMenu;
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputManager.menu)
        {
            //This not work with the current player input need to update in the future
            /*_gameIsPause = !_gameIsPause;
            Debug.Log(_gameIsPause);
            TogglePauseGame(_gameIsPause);
            */
            gameMenu.QuitGameToMainMenu();
            _inputManager.menu = false;
        }
    }

    void Initialize()
    {
        Debug.Log("asdasdas dasdasd");
        _inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Player.InputManager>();
    }
    void TogglePauseGame(bool p_toggleMenuButton)
    {
        canvas.SetActive(p_toggleMenuButton);
        Time.timeScale = p_toggleMenuButton ? 0f : 1f;
    }
}
