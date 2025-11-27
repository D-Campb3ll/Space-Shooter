using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 19.5f;
    [SerializeField] private GameObject _explosion;
    private float _speed = 6f;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private Player _playerScript;
    [TextArea(3, 10)]
    public string _developerNotes;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager on Asteroid is NULL");
        }


       GameObject[] players= GameObject.FindGameObjectsWithTag("Player"); // find with tag

        foreach (GameObject _player in players)
        {
            Player playerScript = _player.GetComponent<Player>();
            _playerScript = playerScript;
        }

        if (players == null)
        {
            Debug.LogError("Player on Asteroid is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (_gameManager._isGameStarted == false)
        {
            transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        }
        else
        {
            _rotateSpeed = 25f;
            transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
            transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {

            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            _speed = 0;
            Destroy(this.gameObject, 1f);

            if (_gameManager._isGameStarted == false)
            {
                _gameManager.GameIsStarted();
                _spawnManager.StartSpawning();
                Debug.Log("Game has started");
            }
        }

        if (other.CompareTag("Player"))
        {
            Destroy(GetComponent<Collider2D>());
            _playerScript.AsteroidDamage();
            GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            _speed = 0;
            Destroy(this.gameObject, 1f);

        }
    }
}
