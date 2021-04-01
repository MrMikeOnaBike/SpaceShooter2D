using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesDisplayImage;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Sprite[] _livesImages;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "SCORE: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        UpdateLives(3);

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.Log("Game Manager is null)");
        }
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "SCORE: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        _livesDisplayImage.sprite = _livesImages[currentLives];

        if(currentLives <=0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _restartText.gameObject.SetActive(true);
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlicker());

        _gameManager.GameOver();
    }

    IEnumerator GameOverTextFlicker()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                _gameOverText.text = "GAME OVER";
                yield return new WaitForSeconds(0.5f);
                _gameOverText.text = "";
                yield return new WaitForSeconds(0.5f);
            }

            _gameOverText.text = "G";
            yield return new WaitForSeconds(0.25f);
            _gameOverText.text = "GA";
            yield return new WaitForSeconds(0.25f);
            _gameOverText.text = "GAM";
            yield return new WaitForSeconds(0.25f);
            _gameOverText.text = "GAME";
            yield return new WaitForSeconds(0.25f);
            _gameOverText.text = "GAME O";
            yield return new WaitForSeconds(0.25f);
            _gameOverText.text = "GAME OV";
            yield return new WaitForSeconds(0.25f);
            _gameOverText.text = "GAME OVE";
            yield return new WaitForSeconds(0.25f);
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.25f);
        }
    }
}
