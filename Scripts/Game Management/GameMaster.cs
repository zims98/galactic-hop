using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class GameMaster : MonoBehaviour
{

    [HideInInspector] public bool gameIsOver = false;
    [HideInInspector] public bool gameStarted = false;
    bool gamePaused = false;

    [Header("GameObjects")]
    [SerializeField] GameObject gameText;
    [SerializeField] GameObject gameOverObject;
    [SerializeField] GameObject scoreTextObject;
    [SerializeField] GameObject pauseIconObject;

    [Header("Component References")]
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;

    [Space]

    [SerializeField] Animator pauseAnim;

    [Header("Script References")]
    [SerializeField] LevelFader faderScript;
    [SerializeField] Score scoreScript;

    int highScore = 0;

    void Start()
    {
        Time.timeScale = 1;
        highScore = PlayerPrefs.GetInt("highscore");
    }

    void Update()
    {
        if (!gamePaused)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Mouse0)))
            {
                gameStarted = true;
                gameText.SetActive(false);
            }
        }
        

        if (!gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();               
            }
        }           
    }

    public void GameOver()
    {
        if (!gameIsOver)
        {
            gameIsOver = true;

            StartCoroutine(GameOverDelay());           

            if (scoreScript.GetScore() > highScore)
            {
                highScore = scoreScript.GetScore();               
                
            }

            PlayerPrefs.SetInt("highscore", highScore);
            PlayerPrefs.Save();

            finalScoreText.text = scoreScript.GetScore().ToString("0");
            highScoreText.text = highScore.ToString("0");

            HideOtherUI();
        }       
    }

    public void PauseGame()
    {
        if (!gameIsOver)
        {
            pauseIconObject.SetActive(false);
            pauseAnim.SetTrigger("ShowMenu");
            gamePaused = true;
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        pauseIconObject.SetActive(true);
        pauseAnim.SetTrigger("HideMenu");
        gamePaused = false;
        Time.timeScale = 1;       
    }
 
    public void PlayAgain()
    {
        faderScript.FadeToLevel(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(2.5f);

        gameOverObject.SetActive(true);
    }

    void HideOtherUI()
    {
        scoreTextObject.SetActive(false);
        pauseIconObject.SetActive(false);
    }
}
