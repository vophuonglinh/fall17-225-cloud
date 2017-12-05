using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Reference: https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/ending-game

public class GameController : MonoBehaviour
{
    public Canvas Canvas;

    //public GUIText scoreText;

    public CanvasGroup GameOverPanel;
    public Button RestartButton;
    public GameObject Player;
    public Rigidbody rb;

	//private int score;

	void Start ()
	{
        Canvas = GetComponent<Canvas>();

        GameObject GameOverPanelObject = GameObject.FindWithTag("GameOverPanel");
        GameOverPanel = GameOverPanelObject.GetComponent<CanvasGroup>();

        RestartButton = GameObject.FindWithTag("Restart").GetComponent<Button>();
        RestartButton.onClick.AddListener(RestartGame);

        GameObject Player = GameObject.FindWithTag("Player");
        Rigidbody rb = Player.GetComponent<Rigidbody>();

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
        GameOverPanel.alpha = 1;
        GameOverPanel.interactable = true;
        rb.isKinematic = true;
	} 


    public void RestartGame() {
        Application.LoadLevel(Application.loadedLevel);
    }


}






