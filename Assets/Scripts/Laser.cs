using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed variable of 8
    [SerializeField] private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    private Player _shooterPlayer;

    // Update is called once per frame
    void Update()
    {

        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                player.Damage();
                Debug.Log("Player got hit");
            }
            else
            {
                Debug.LogError("Player on Laser is NULL");
            }

        }
    }

    public void AssignShooter(Player player)
    {
        _shooterPlayer = player;
        Debug.Log("Assigned Shooter is: " + _shooterPlayer);
    }



}
