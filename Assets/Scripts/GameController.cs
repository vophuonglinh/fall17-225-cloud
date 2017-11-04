﻿using UnityEngine;
using System.Collections;

// Reference: https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/ending-game

public class GameController : MonoBehaviour
{

	/* public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

*/

	//public GUIText scoreText;
	//public GUIText restartText;
	//public static GUIText gameOverText;

	private bool gameOver;
	//private bool restart;
	//private int score;

	void Start ()
	{
		gameOver = false;
		/*
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		*/
		// UpdateScore ();
		// StartCoroutine (SpawnWaves ());
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

	IEnumerator SpawnWaves ()
	{

		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);



			if (gameOver)
			{
				restartText.text = "Press 'R' for Restart";
				restart = true;
			//	break;
			}
		//}
	}

*/

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
		//gameOverText.text = "Game Over!";
		gameOver = true;
	} 

	public void DeleteAll(){ 		foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) { 			Destroy(o); 		} 	}
}






