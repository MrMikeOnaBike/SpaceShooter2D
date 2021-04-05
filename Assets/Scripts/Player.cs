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
    [SerializeField] private GameObject _rightEngineFire;
    [SerializeField] private GameObject _leftEngineFire;
    [SerializeField] private GameObject _explosionPrefab;

    private bool _isTripleShotActive = false;
    private bool _areShieldsActive = false;
    int _lives = 3;
    private SpawnManager _spawnManager;
    private UIManager _UImanager;
    private int _score;
    private Animator _anim;


    private float _canFire = -1f;

    void Start()
    {
        transform.position = new Vector3(0f, -3f, 0f);

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

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.Log("Animator is null.");
        }
    }


    void Update()
    {
        if (_lives > 0)
        {
            CalculateMovement();

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
    }


    void CalculateMovement()
    {
        //X & Y SWITCHED FOR LANDSCAPE LAYOUT!!

        // float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");
        float verticalInput = Input.GetAxis("Horizontal");
        float horizontalInput = 0 - Input.GetAxis("Vertical");

        _anim.SetFloat("Direction", horizontalInput);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f);

        transform.Translate(direction * _speed * _speedBoost * Time.deltaTime);

        if (transform.position.y >= 3)
        {
            transform.position = new Vector3(transform.position.x, 3f, 0f);
        }
        else if (transform.position.y <= -7)
        {
            transform.position = new Vector3(transform.position.x, -7f, 0f);
        }

        if (transform.position.x > 6f)
        {
            transform.position = new Vector3(-6f, transform.position.y, 0f);
        }
        else if (transform.position.x < -6f)
        {
            transform.position = new Vector3(6f, transform.position.y, 0f);
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

            UpdatePlayerDamage();
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

    private void UpdatePlayerDamage()
    {
        //in a future update there will be health power-ups
        //so turn off both before checking to give the illusion
        //  of randomally fixing either the left or the right
        _rightEngineFire.SetActive(false);
        _leftEngineFire.SetActive(false);

        switch (_lives)
        {
            case 0:  //game over
                _rightEngineFire.SetActive(true);
                _leftEngineFire.SetActive(true);
                _spawnManager.GetComponent<SpawnManager>().OnPlayerDeath();
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(this.gameObject, 1f);
                break;
            case 1:  //both engines damages
                _rightEngineFire.SetActive(true);
                _leftEngineFire.SetActive(true);
                break;
            case 2:  //pick one engine to damage
                if (Random.Range(1, 3) == 1)
                {
                    _rightEngineFire.SetActive(true);
                }
                else
                {
                    _leftEngineFire.SetActive(true);
                }
                break;
        }
    }
}