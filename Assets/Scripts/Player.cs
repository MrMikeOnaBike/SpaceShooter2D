using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedBoost = 1f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.2f;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shields;

    private bool _isTripleShotActive = false;
    private bool _areShieldsActive = false;
    int _lives = 3;
    private SpawnManager _spawnManager;
    private UIManager _UImanager;
    private int _score;

    private float _canFire = -1f;

    void Start()
    {
        transform.position = new Vector3(0f, -3.6f, 0f);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("The SpawnManager is null.");
        }

        _UImanager = GameObject.Find("UI Canvas").GetComponent<UIManager>();
        if (_UImanager == null)
        {
            Debug.Log("The UIManager is null.");
        }
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

        transform.Translate(direction * _speed * _speedBoost * Time.deltaTime);

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

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }
    }

    public void Damage()
    {
        if(_areShieldsActive == true)
        {
            //shields are on so they take the hit
            _areShieldsActive = false;
            _shields.SetActive(false);
        }
        else
        {
            _lives--;

            _UImanager.UpdateLives(_lives);

            if (_lives <= 0)
            {
                //call the function in the spawn manager script and let it know we died
                _spawnManager.GetComponent<SpawnManager>().OnPlayerDeath();

                Destroy(this.gameObject);
            }
        }
    }

    public void TurnOnTripleShot(float lifeSpan)
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotCoolDown(lifeSpan));
    }

    IEnumerator TripleShotCoolDown(float lifeSpan)
    {
        yield return new WaitForSeconds(lifeSpan);
        _isTripleShotActive = false;
    }

    public void TurnOnSpeedPowerup(float lifeSpan)
    {
        _speedBoost = 2.0f;
        StartCoroutine(SpeedBoostCoolDown(lifeSpan));
    }

    IEnumerator SpeedBoostCoolDown(float lifeSpan)
    {
        yield return new WaitForSeconds(lifeSpan);
        _speedBoost = 1.0f;
    }

    public void TurnOnShields(float lifeSpan)
    {
        _areShieldsActive = true;
        _shields.SetActive(true);
        StartCoroutine(ShieldsCoolDown(lifeSpan));
    } 
 
    IEnumerator ShieldsCoolDown(float lifeSpan)
    {
        yield return new WaitForSeconds(lifeSpan);
        _shields.SetActive(false);
        _areShieldsActive = false;
    }

    public void AddScore()
    {
        _score += 10;

        _UImanager.UpdateScoreText(_score);

    }
}