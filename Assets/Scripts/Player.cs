using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.2f;
    private float _canFire = -1f;
    [SerializeField] int _lives = 3;
    [SerializeField] private GameObject _spawner;

    void Start()
    {
        transform.position = new Vector3(0f, -3.6f, 0f);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0f);
        }
        else if (transform.position.y <= -3.8)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0f);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0f);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0f);
        }

    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
    }

    public void Damage()
    {
        _lives--;
        Debug.Log("You have " + _lives + " lives left. Be more careful.");

        if(_lives <= 0)
        {
            Debug.Log("You died.");

            //call the function in the spawn manager script and let it know we died
            _spawner.GetComponent<SpawnManager>().OnPlayerDeath();

            Destroy(this.gameObject);
        }
    }
}