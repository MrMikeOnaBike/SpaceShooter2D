using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 6f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player _player = other.gameObject.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();
            }

            Destroy(this.gameObject);

        }
        else if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            Destroy(this.gameObject);
        }
    }
}
