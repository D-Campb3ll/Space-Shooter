using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public bool _isMultiplayer = false;
    public bool _isGameOver = false;
    private bool _isPaused = false;
    public bool _isGameStarted = false;
    [SerializeField] private GameObject _pausePanel;
    private Animator _pauseAnimator;
    private LevelLoader _levelLoader;
    private UIManager _uiManager;

    private void Start()
    {
        _pauseAnimator = GameObject.Find("Canvas").transform.Find("Pause_Menu_Panel").GetComponent<Animator>();

        //allows animations to play when timescale is set to 0
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

        if (_pauseAnimator == null)
        {
            Debug.LogError("Pause Animator on Game Manager is NULL");
        }

        _levelLoader = GameObject.Find("Canvas").GetComponent<LevelLoader>();

        if (_levelLoader == null)
        {
            Debug.LogError("Level Loader on Game Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager on GameManager is NULL");
        }
    }

    private void Update()
    {

        //Restart
        if (_isMultiplayer == false)
        {
            if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
            {
                SceneManager.LoadScene(1); //Current Game Scene
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
            {
                SceneManager.LoadScene(2);
            }
        }

        //Quit application
        if (SceneManager.GetActiveScene().name == "Main_Menu" && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Pause game
        if (Input.GetKeyDown(KeyCode.P) && _isPaused == false)
        {
            _pausePanel.SetActive(true);
            _pauseAnimator.SetBool("IsGamePaused", true);
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.P) && _isPaused == true)
        {
            _pausePanel.SetActive(false);
            ResumeGame();
        }

        //Go back to menu when paused
        if (Input.GetKeyDown(KeyCode.Escape) && _isPaused == true)
        {
            BackToMenu();
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 && _uiManager._readOnlyPlayerOneScore == 60)
        {
            _levelLoader.LoadBossFight();
        }

       
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;
        _pausePanel.SetActive(false);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        _isPaused = false;
        _levelLoader.BackToMenu();
    }

    public void GameIsStarted()
    {
        _isGameStarted = true;
    }

}
