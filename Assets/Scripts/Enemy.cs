using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private Animator _anim;
    private Player _player;
    private bool _isDying = false;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.Log("Player game object not found - something is very wrong...");
        }

        _anim = GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.Log("Animator is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6f && _isDying == false)
        {
            transform.position = new Vector3(Random.Range(-5f, 5f), 12f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _isDying = true;

            Player _player = other.gameObject.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");

            Destroy(this.gameObject, 2.8f);

        }
        else if(other.tag == "Laser")
        {
            _isDying = true;

            Destroy(other.gameObject);

            _player.AddScore();

            _speed = 0f;

            _anim.SetTrigger("OnEnemyDeath");

            Destroy(this.gameObject, 2.8f);
        }
    }
}
