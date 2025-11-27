using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private int _bossHealth = 50;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private AudioClip _fireLaser;
    [SerializeField] private AudioClip _explosionSoundClip;
    [SerializeField] private GameObject _finishGamePanel;
    private UIManager _uiManager;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _targetYPos = 2.7f;
    private bool _isBossAlive = true;
    private bool _isFightStarted = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager on EnemyBoss is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on EnemyBoss is NULL");
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator on EnemyBoss is NULL");
        }

        transform.position = new Vector2(0, 8);

        while (transform.position.y > _targetYPos)
        {
            Debug.Log("Boss is moving");
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
            yield return null;
        }

        _uiManager.UpdateBossHealth(_bossHealth);

        StartCoroutine(MoveBoss());
        yield return new WaitForSeconds(1f);
        StartCoroutine(BossFireRoutine());
        _isFightStarted = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (_bossHealth <= 0)
        {
            _isBossAlive = false;
            Destroy(GetComponent<Collider2D>());
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.PlayOneShot(_explosionSoundClip, 0.2f);
            _speed = 0;
            Destroy(this.gameObject, 2.8f);

            _finishGamePanel.SetActive(true);
        }


    }

    private void BossDamage()
    {
        _bossHealth = _bossHealth - 2;
    }

    IEnumerator MoveBoss()
    {
        while (_isBossAlive == true)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector2 targetPosition = new Vector2(randomX, transform.position.y);

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

            while (Mathf.Abs(transform.position.x - randomX) > 0.01f && _isBossAlive == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }

    IEnumerator BossFireRoutine()
    {
        while (Mathf.Abs(transform.position.y - _targetYPos) > 0.05f && _isBossAlive == true)
        {
            yield return null;
        }

        while (_isBossAlive == true)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_fireLaser);

                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                for (int l = 0; l < lasers.Length; l++)
                {
                    lasers[l].AssignEnemyLaser();
                }

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser") && _isFightStarted == true)
        {
            Destroy(other.gameObject);
            BossDamage();
            _animator.SetTrigger("BossDamaged");
            StartCoroutine(ShakeBoss(0.06f, 0.1f));
            _uiManager.UpdateBossHealth(_bossHealth);
        }
    }

    IEnumerator ShakeBoss(float duration, float magnitude)
    {
        Vector2 originalPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xShake = Random.Range(-1, 1f) * magnitude;
            float yShake = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPos + new Vector2(xShake, yShake);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }
}
