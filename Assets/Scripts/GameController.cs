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
    public Text GameOverText;

	//private int score;

	void Start ()
	{
        PauseButton.onClick.AddListener(PauseGame);

        RestartButton.gameObject.SetActive(false);
        RestartButton.onClick.AddListener(RestartGame);

        ContinueButton.gameObject.SetActive(false);
        ContinueButton.onClick.AddListener(ContinueGame);

        GameOverText.enabled = false;
		//score = 0;
	
		// UpdateScore ();

	}
	void Update ()
	{
        
        	
	}


	/*

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}
	*/

	public void GameOver ()
	{
        rb.isKinematic = true;
        GameOverText.enabled = true;
        RestartButton.gameObject.SetActive(true);
	} 

    public void PauseGame(){
        Time.timeScale = 0;
        ContinueButton.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(true);
    }

    public void RestartGame() 
    {
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        RestartButton.gameObject.SetActive(false);
        ContinueButton.gameObject.SetActive(false);
    }


}






