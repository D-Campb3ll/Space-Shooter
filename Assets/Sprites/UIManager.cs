using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle to Text
    [Header("Text")]
    [SerializeField] private Text _controlsText;
    [SerializeField] private Text _playerOneScoreText;
    [SerializeField] private Text _playerTwoScoreText;
    [SerializeField] private Text _bestScoreText;
    [SerializeField] private Text _gameOver;
    [SerializeField] private Text _restartButton;
    [SerializeField] private Text _bossHealth;
    [Header("Images")]
    [SerializeField] private Image _playerOneLivesImage;
    [SerializeField] private Image _playerTwoLivesImage;
    [Header("Sprites")]
    [SerializeField] private Sprite[] _lifeSprites;
    [SerializeField] private Sprite[] _playerTwoLifeSprites;
    [Header("Audio")]
    [SerializeField] private AudioClip _buttonSoundClip;
    private GameManager _gameManager;
    private AudioSource _audioSource;
    private Player _playerScript;
    public int _playerOneScore;
    private int _playerTwoScore;
    public int _bestScore;
    public int _readOnlyPlayerOneScore => _playerOneScore;


    // Start is called before the first frame update
    void Start()
    {

        _gameOver.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _playerOneScoreText.text = "Score: " + 0; 

        if (_gameManager._isMultiplayer == true)
        {
            _playerTwoScoreText.text = "Score: " + 0;
        }

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on UI Manager is NULL");
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject _player in players)
        {
            Player playerScript = _player.GetComponent<Player>();
            _playerScript = playerScript;
        }
    }

    private void Update()
    {
        if (_gameManager._isGameStarted == true)
        {
            _controlsText.enabled = false;
        }

    }

    public void UpdateScore()
    {
        if (_playerScript._isPlayerOne == true)
        {
            Debug.Log("Updating Player 1 score");
            _playerOneScore += 10;
            _playerOneScoreText.text = "Score: " + _playerOneScore;
        }

        if (_playerScript._isPlayerTwo ==true)
        {
            Debug.Log("Updating Player 2 score");
            _playerTwoScore += 10;
            _playerTwoScoreText.text = "Score: " + _playerTwoScore;
        }

    }

    public void UpdateBestScore()
    {
        if (_gameManager._isMultiplayer == false)
        {
            _bestScoreText.text = "Best: " + _bestScore;
        }
    }

    public void CheckBestScore()
    {
        if (_gameManager._isMultiplayer == false)
        {
            if (_playerOneScore > _bestScore)
            {
                _bestScore = _playerOneScore;
                UpdateBestScore();
            }
        }
    }


    public void PlayerOneUpdateLives(int currentLives)
    {
        _playerOneLivesImage.sprite = _lifeSprites[currentLives];

        if (currentLives == 0)
        {
            GameOver();
        }

    }

    public void PlayerTwoUpdateLives(int currentLives)
    {
            _playerTwoLivesImage.sprite = _playerTwoLifeSprites[currentLives];

            if (currentLives == 0)
            {
                GameOver();
            }
    }

    public void UpdateBossHealth(int currentBossHealth)
    {
        _bossHealth.text = "Boss Health: " + currentBossHealth;
    }

    private void GameOver()
    {
        _gameManager.GameOver();
        StartCoroutine(GameOverFlashRoutine());
    }

    IEnumerator GameOverFlashRoutine()
    {
        while (true)
        {
            _gameOver.gameObject.SetActive(true);
            _restartButton.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOver.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

    }

    public void PlayButtonSound()
    {
        _audioSource.PlayOneShot(_buttonSoundClip);
    }

}
