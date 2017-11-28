﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Reference: https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/ending-game

public class GameController : MonoBehaviour
{
    public Canvas Canvas;

    //public GUIText scoreText;

	private bool gameOver;

    public CanvasGroup GameOverPanel;
    public Button RestartButton;
	//private int score;

	void Start ()
	{
		gameOver = false;
        Canvas = GetComponent<Canvas>();

        GameObject GameOverPanelObject = GameObject.FindWithTag("GameOverPanel");
        GameOverPanel = GameOverPanelObject.GetComponent<CanvasGroup>();

        RestartButton = GameObject.FindWithTag("Restart").GetComponent<Button>();
        RestartButton.onClick.AddListener(RestartGame);
       
      

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
		gameOver = true;
        GameOverPanel.alpha = 1;
        GameOverPanel.interactable = true;
	} 


    public void RestartGame() {
        Application.LoadLevel(Application.loadedLevel);
        
    }

}






