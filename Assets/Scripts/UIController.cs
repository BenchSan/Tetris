using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private bool gameIsPaused = false;
    public bool gameisStarted = false;
    public GameObject pauseMenuUI;
    public GameObject mainMenuIU;
    public GameObject howToPlayUI;
    public GameObject gameOverUI;
    public GameObject score;
    private int startScorePosx = 480;
    private int startScorePosy = 670;
    private int endScorePosx = 1180;
    private int endScorePosy = 0;

    private void Start()
    {
        FindObjectOfType<Piece>().enabled = false;
        FindObjectOfType<Ghost>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&gameisStarted)
        {
            if (gameIsPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        FindObjectOfType<Piece>().enabled = true;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        FindObjectOfType<Piece>().enabled = false;
        
    }

    public void ShowHowToPlay()
    {
        mainMenuIU.SetActive(false);
        howToPlayUI.SetActive(true);
    }

    public void StartGame()
    {
        mainMenuIU.SetActive(false);
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        score.SetActive(true);
        Time.timeScale = 1f;
        score.transform.position = new Vector3(startScorePosx,startScorePosy,0);
        FindObjectOfType<Piece>().enabled = true;
        FindObjectOfType<Ghost>().enabled = true;
        FindObjectOfType<Board>().Spawn();
        FindObjectOfType<Score>().GetComponent<Text>().text = "Score: " + 0;
        gameisStarted = true;
        gameIsPaused = false;
    }

    public void ToMainMenu()
    {
        FindObjectOfType<Piece>().enabled = false;
        FindObjectOfType<Ghost>().enabled = false;
        FindObjectOfType<Board>().tilemap.ClearAllTiles();
        FindObjectOfType<Ghost>().tilemap.ClearAllTiles();
        gameisStarted = false;
        gameIsPaused = false;
        score.SetActive(false);
        mainMenuIU.SetActive(true);
        howToPlayUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameisStarted = false;
        gameOverUI.SetActive(true);
        FindObjectOfType<Board>().enabled = false;
        FindObjectOfType<Piece>().enabled = false;
        FindObjectOfType<Ghost>().enabled = false;
        Clear();
        score.transform.position = new Vector3(endScorePosx,endScorePosy,0);
    }
    
    public void Retry()
    {
        Time.timeScale = 1f;
        FindObjectOfType<Piece>().enabled = true;
        FindObjectOfType<Ghost>().enabled = true;
        gameOverUI.SetActive(false);
        gameisStarted = true;
        score.transform.position = new Vector3(startScorePosx,startScorePosy,0);
        FindObjectOfType<Score>().GetComponent<Text>().text = "Score: " + 0;
    }

    public void Clear()
    {
        FindObjectOfType<Board>().tilemap.ClearAllTiles();
        FindObjectOfType<Ghost>().tilemap.ClearAllTiles();
    }
    public void Quit()
    {
        Application.Quit(); 
    }
    
}
