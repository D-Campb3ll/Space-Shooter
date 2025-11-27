using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _powerupID; //0 = triple Shot, 1 = Speed, 2 = Shields, 3 = Health
    [SerializeField] private AudioClip _powerupSound;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Communicate with player script using other
            Player player = other.transform.GetComponent<Player>();

            //Instantiating an audio clip
            AudioSource.PlayClipAtPoint(_powerupSound, new Vector3(transform.position.x, transform.position.y, -10f), 1000f); //positions on the camera location

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShields();                       
                        break;
                    case 3:
                        player.IncreaseHealth();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }

            }

            Destroy(this.gameObject);
        }
    }
}
