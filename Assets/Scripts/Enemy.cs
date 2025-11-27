using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //speed = 4
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private AudioClip _enemyExplosionSoundClip;
    [SerializeField] private AudioClip _enemyLaserFireSound;
    [SerializeField] private GameObject _laserPrefab;
    private UIManager _uiManager;
    private Player _player;
    private Animator _destroyAnimation;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canfire = -1;
    private bool _isEnemyDead = false;

    [TextArea(3, 10)]
    public string developerNote;
    


    private void Start()
    {

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager on Enemy is NULL");
        }

        _destroyAnimation = GetComponent<Animator>();

        if (_destroyAnimation == null)
        {
            Debug.LogError("_destroyAnimation is NULL");
        }

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("_player is NULL");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is NULL");
        }
        else
        {
            _audioSource.clip = _enemyExplosionSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (_isEnemyDead == false)
        {
            EnemyFireLaser();
        }
    }

    private void EnemyFireLaser()
    {
        if (Time.time > _canfire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canfire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

            _audioSource.PlayOneShot(_enemyLaserFireSound);
            // Because there are 2 children objects (The lasers), we need to use an Array to get the Laser Script component
            //and a for loop to cycle through them

            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        //Move the enemy down at 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.7f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6.3f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        //If tag contains "Laser"
        if (other.CompareTag("Laser"))
        {

            //Destroy Laser
            Destroy(other.gameObject);

            //Add to score
            _uiManager.UpdateScore();

            //Destroy Enemy
            _isEnemyDead = true;
            Destroy(GetComponent<Collider2D>());
            _destroyAnimation.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }
        
        //If tag contains "Player"
        if (other.CompareTag("Player"))
        {

            Player player = other.transform.GetComponent<Player>();
            //Damage player
            if (player != null)
            {
                player.Damage();
            }

            //Destroy Enemy
            _isEnemyDead = true;
            Destroy(GetComponent<Collider2D>());
            _destroyAnimation.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }

    }
}
