using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Reference: https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/ending-game

public class GameController : MonoBehaviour
{
    public Canvas Canvas;

    //public GUIText scoreText;

	private bool gameOver;
	private bool restart;


    public CanvasGroup GameOverPanel;
	//private int score;

	void Start ()
	{
		gameOver = false;
		restart = false;
        Canvas = GetComponent<Canvas>();

        GameObject GameOverPanelObject = GameObject.FindWithTag("GameOverPanel");
        GameOverPanel = GameOverPanelObject.GetComponent<CanvasGroup>();
       
      

		//score = 0;
	
		// UpdateScore ();

	}

	void Update ()
	{
        
		/*
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}
*/
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
		gameOver = true;
        GameOverPanel.alpha = 1;
        GameOverPanel.interactable = true;
	} 


    public void RestartGame() {
        restart = true;
        
    }


}






