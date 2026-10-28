using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject gameOverPanel; 

    void Awake()
    {
        instance = this;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Cursor.visible = true; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenMarket()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MarketMenu"); 
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }

    public void GameScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene"); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyun Kapatıldı.");
    }

    public void LastOpenedScene()
    {        Time.timeScale = 1f;
        string lastScene = PlayerPrefs.GetString("LastOpenedScene", "SampleScene"); // Default olarak SampleScene
        SceneManager.LoadScene(lastScene);
    }
}