using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Reference: https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/ending-game

public class GameController : MonoBehaviour
{
  

    //public GUIText scoreText;

    public GameObject Player;
    public Rigidbody rb;
    public Button RestartButton;
    public Text GameOverText;
    //public Text RestartText;

	//private int score;

	void Start ()
	{
        RestartButton.gameObject.SetActive(false);
        RestartButton.onClick.AddListener(RestartGame);

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
        //GameOverPanel.alpha = 1;
        //GameOverPanel.interactable = true;
        rb.isKinematic = true;
        GameOverText.enabled = true;
        RestartButton.gameObject.SetActive(true);
       // RestartText.enabled = true;
	} 


    public void RestartGame() {
        Application.LoadLevel(Application.loadedLevel);
    }


}






