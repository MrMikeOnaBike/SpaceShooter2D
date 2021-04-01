using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _lifeSpan = 5.0f;

    [SerializeField] private int _powerupID;  //0 = triple shot, 1 = speed, 2 = shield, etc.

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -8.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null)
            {
                switch(_powerupID)
                {
                    case 0: //triple shot
                        player.TurnOnTripleShot(_lifeSpan);
                        break;
                    case 1: //speed boost
                        player.TurnOnSpeedPowerup(_lifeSpan);
                        break;
                    case 2: //shields 
                        player.TurnOnShields(_lifeSpan);
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }

}
