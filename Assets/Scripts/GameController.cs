using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

// Reference: https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/ending-game

public class GameController : MonoBehaviour
{


    //public GUIText scoreText;

    public GameObject Player;
    public Rigidbody rb;
    public Button PauseButton;
    public Button ContinueButton;
    public Button RestartButton;
    public Button MenuButton;
    public Text GameOverText;
    private Image Background;

	//private int score;

	void Start ()
	{
        PauseButton.onClick.AddListener(PauseGame);

        RestartButton.gameObject.SetActive(false);
        RestartButton.onClick.AddListener(RestartGame);

        ContinueButton.gameObject.SetActive(false);
        ContinueButton.onClick.AddListener(ContinueGame);

        MenuButton.gameObject.SetActive(false);
        MenuButton.onClick.AddListener(LoadMenu);

        Background = GetComponent<Image>();
        Background.enabled = false;

        GameOverText.enabled = false;

	}

	public void GameOver ()
	{
        rb.isKinematic = true;
        GameOverText.enabled = true;
        RestartButton.gameObject.SetActive(true);
        MenuButton.gameObject.SetActive(true);
        PauseButton.enabled = false;
        Background.enabled = true;
	}

    public void PauseGame(){
        Time.timeScale = 0;
        ContinueButton.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(true);
        MenuButton.gameObject.SetActive(true);
        Background.enabled = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(1);
        Background.enabled = false;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        RestartButton.gameObject.SetActive(false);
        ContinueButton.gameObject.SetActive(false);
        MenuButton.gameObject.SetActive(false);
        Background.enabled = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
        Background.enabled = false;
    }

    // Pause game when player switches out of app
    void OnApplicationFocus(bool hasFocus)
    {
      if (!hasFocus)
      {
        PauseGame();
      }
    }
}
