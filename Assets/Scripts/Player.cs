using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool _isPlayerOne = false;
    public bool _isPlayerTwo = false;
    private float _boostedSpeed = 2f;
    private float _canFire = -1f;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private UIManager _uiManager;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    [SerializeField] private Animator _animator;
    [Header("Audio")]
    [SerializeField] private AudioClip _laserSoundClip;
    //[SerializeField] private AudioClip _playerExplosionSound;  -- keep for future use
    [Header("Game Objects")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _playerExplosion;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _rightWingFire, _leftWingFire;
    [Header("Player Attributes")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _lives = 3;



    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager on Player is NULL");
        }

        if (_gameManager._isMultiplayer == false)
        {
            transform.position = new Vector3(0, -2.7f, 0);
        }
        else
        {
            if (_isPlayerOne == true)
            {
                transform.position = new Vector3(4.5f, 0, 0);
            }

            if (_isPlayerTwo == true)
            {
                transform.position = new Vector3(-4.5f, 0, 0);
            }
        }

        //Find the Spawn_Manager object. Get the SpawnManager Script component
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_playerExplosion == null)
        {
            Debug.LogError("Player Explosion is NULL");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio source on player is NULL");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator on Player is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerOne == true)
        {
            CalculateMovement();

            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetMouseButtonDown(0))
            {
                FireLaser();
            }
        }

        if (_isPlayerTwo == true)
        {
            CalculateMovement();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireLaser();
            }
        }
    }

    void CalculateMovement()
    {

        if (_isPlayerOne == true)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            if (_isSpeedBoostActive == true)
            {
                float speedBoost = _speed * _boostedSpeed;
                transform.Translate(direction * speedBoost * Time.deltaTime);
            }
            else
            {
                transform.Translate(direction * _speed * Time.deltaTime);
            }

            if (horizontalInput < -0.01f)
            {
                _animator.SetBool("IsTurningLeft", true);
                _animator.SetBool("IsTurningRight", false);
                _animator.SetBool("IsIdle", false);
            }
            else if (horizontalInput > 0.01f)
            {
                _animator.SetBool("IsTurningLeft", false);
                _animator.SetBool("IsTurningRight", true);
                _animator.SetBool("IsIdle", false);
            }
            else
            {
                _animator.SetBool("IsTurningLeft", false);
                _animator.SetBool("IsTurningRight", false);
                _animator.SetBool("IsIdle", true);
            }
        }

        if (_isPlayerTwo == true)
        {
            float horizontalInput = Input.GetAxis("Horizontal_PlayerTwo");
            float verticalInput = Input.GetAxis("Vertical_PlayerTwo");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            if (_isSpeedBoostActive == true)
            {
                float speedBoost = _speed * _boostedSpeed;
                transform.Translate(direction * speedBoost * Time.deltaTime);
            }
            else
            {
                transform.Translate(direction * _speed * Time.deltaTime);
            }

            if (horizontalInput < -0.01f)
            {
                _animator.SetBool("IsTurningLeft", true);
                _animator.SetBool("IsTurningRight", false);
                _animator.SetBool("IsIdle", false);
            }
            else if (horizontalInput > 0.01f)
            {
                _animator.SetBool("IsTurningLeft", false);
                _animator.SetBool("IsTurningRight", true);
                _animator.SetBool("IsIdle", false);
            }
            else
            {
                _animator.SetBool("IsTurningLeft", false);
                _animator.SetBool("IsTurningRight", false);
                _animator.SetBool("IsIdle", true);
            }
        }


        //if (transform.position.y >= 0)
        //{
        //    transform.position = new Vector3(transform.position.x, 0, 0);
        //}
        //else if (transform.position.y <= -3.8f)
        //{
        //    transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}

        //Below is an optimized version of the above
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        else
        {
            if (Time.time > _canFire)
            {
                if (_isTripleShotActive == true)
                {
                    Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                    //Laser laser = GameObject.Find("Laser").GetComponent<Laser>();
                    //laser.AssignShooter(this);
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f), Quaternion.identity);
                    //Laser[] laser = GameObject.FindGameObjectsWithTag("Laser").GetComponent<Laser>();
                    //laser.AssignShooter(this);
                }

                _audioSource.PlayOneShot(_laserSoundClip);
                _canFire = Time.time + _fireRate;
            }
        }
    }

    public void Damage()
    {
        if (_isPlayerOne == true)
        {
            if (_isShieldsActive == true)
            {
                _isShieldsActive = false;
                _shield.SetActive(false);
                return;
            }

            _lives--;

            if (_lives == 2)
            {
                _leftWingFire.SetActive(true);
            }
            else if (_lives == 1)
            {
                _rightWingFire.SetActive(true);
            }

            _uiManager.PlayerOneUpdateLives(_lives);

            if (_lives < 1)
            {
                _uiManager.CheckBestScore();
                Instantiate(_playerExplosion, transform.position, Quaternion.identity);
                //audioSource.PlayOneShot(_playerExplosionSound); -- keep for future
                _speed = 0;
                _spawnManager.OnPlayerDeath();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 0.5f);
            }
        }

        if (_isPlayerTwo == true)
        {
            if (_isShieldsActive == true)
            {
                _isShieldsActive = false;
                _shield.SetActive(false);
                return;
            }

            _lives--;

            if (_lives == 2)
            {
                _leftWingFire.SetActive(true);
            }
            else if (_lives == 1)
            {
                _rightWingFire.SetActive(true);
            }

            _uiManager.PlayerTwoUpdateLives(_lives);

            if (_lives < 1)
            {
                _uiManager.CheckBestScore();
                Instantiate(_playerExplosion, transform.position, Quaternion.identity);
                //audioSource.PlayOneShot(_playerExplosionSound); -- keep for future
                _speed = 0;
                _spawnManager.OnPlayerDeath();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 0.5f);
            }
        }
    }

    public void AsteroidDamage()
    {

        if (_isPlayerOne == true)
        {
            if (_isShieldsActive == true)
            {
                _isShieldsActive = false;
                _shield.SetActive(false);
                return;
            }

            _lives = 0;

            _uiManager.PlayerOneUpdateLives(_lives);

            if (_lives < 1)
            {
                _uiManager.CheckBestScore();
                Instantiate(_playerExplosion, transform.position, Quaternion.identity);
                //audioSource.PlayOneShot(_playerExplosionSound); -- keep for future
                _speed = 0;
                _spawnManager.OnPlayerDeath();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 0.5f);
            }
        }

        if (_isPlayerTwo == true)
        {
            if (_isShieldsActive == true)
            {
                _isShieldsActive = false;
                _shield.SetActive(false);
                return;
            }

            _lives = 0;

            _uiManager.PlayerTwoUpdateLives(_lives);

            if (_lives < 1)
            {
                _uiManager.CheckBestScore();
                Instantiate(_playerExplosion, transform.position, Quaternion.identity);
                //audioSource.PlayOneShot(_playerExplosionSound); -- keep for future
                _speed = 0;
                _spawnManager.OnPlayerDeath();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 0.5f);
            }
        }
      
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void ActivateSpeedBoost()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    public void ActivateShields()
    {
        _isShieldsActive = true;
        _shield.SetActive(true);
    }

    public void IncreaseHealth()
    {
        _lives += 1;
        _uiManager.PlayerOneUpdateLives(_lives);

        if (_lives == 2)
        {
            _leftWingFire.SetActive(false);
        }
        else if (_lives == 3)
        {
            _rightWingFire.SetActive(false); 
            _leftWingFire.SetActive(false);
        }
    }


    IEnumerator TripleShotPowerDownRoutine() 
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
    }
}

