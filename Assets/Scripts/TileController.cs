using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

	public GameObject currentTile;
	public string lastDirection;
	public GameObject[] tilePrefabs;
	public bool nextTileStraight;

	private static TileController instance;

	private Stack<GameObject> leftTiles = new Stack<GameObject>();
	private Stack<GameObject> topTiles = new Stack<GameObject>();
	private Stack<GameObject> rightTiles = new Stack<GameObject>();


	public static TileController Instance
	{

		get 
		{ 
			if (instance == null) {
				instance = GameObject.FindObjectOfType<TileController> ();
			}
			return instance;
		}
	}

	// Use this for initialization
	void Start () {
		nextTileStraight = false;
		CreateTiles (80);
		for (int i = 0; i < 40; i++) {
			SpawnTile ();
		}
	}

	public void CreateTiles(int amount)
	{
		for (int i = 0; i<amount; i++)
		{
			leftTiles.Push(Instantiate(tilePrefabs[0]));
			topTiles.Push (Instantiate (tilePrefabs [2]));
			rightTiles.Push (Instantiate (tilePrefabs [1]));

			leftTiles.Peek().name = "LeftTile";
			leftTiles.Peek().SetActive (false);           //ensure they don't overlap each other!

			topTiles.Peek ().name = "TopTile";
			topTiles.Peek().SetActive (false);

			rightTiles.Peek ().name = "RightTile";
			rightTiles.Peek().SetActive (false);

		}
	}

	public Stack<GameObject> LeftTiles
	{
		get { return leftTiles;}
		set { leftTiles = value;}
	}
	public Stack<GameObject> TopTiles
	{
		get { return topTiles;}
		set { topTiles = value;}
	}
	public Stack<GameObject> RightTiles
	{
		get { return rightTiles;}
		set { rightTiles = value;}
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnTile()
	{
		if (leftTiles.Count == 0 || topTiles.Count == 0 || rightTiles.Count == 0) 
		{
			CreateTiles (10); 
		}
		int randomDirectionIndex = Random.Range (0, tilePrefabs.Length);
		int childIndex;
		string direction = "top";
		string randomDirectionTilePrefab = tilePrefabs [randomDirectionIndex].name;
		if (nextTileStraight == true) 
		{
			randomDirectionTilePrefab = "TopTile1";
			nextTileStraight = false;
		}
		else if (lastDirection != "top" && randomDirectionTilePrefab != "TopTile1") 
		{
			randomDirectionTilePrefab = "TopTile1";
			nextTileStraight = true;
		}
		switch(randomDirectionTilePrefab) 
		{
		case "LeftTile1":
			childIndex = 0;
			direction = "left";
			break;
		case "RightTile1":
			childIndex = 2;
			direction = "right";
			break;
		case "TopTile1":
			childIndex = 1;
			direction = "top";
			break;
		default:
			childIndex = 1;
			break;
		}
			
		if (direction == "left") 
		{
			GameObject temp = leftTiles.Pop ();
			temp.SetActive (true);
			temp.transform.position = currentTile.transform.GetChild (0).transform.GetChild (childIndex).position;
			currentTile = temp;
			lastDirection = "left";
		}
		else if (direction == "top")
		{
			GameObject temp = topTiles.Pop ();
			temp.SetActive (true);
			temp.transform.position = currentTile.transform.GetChild (0).transform.GetChild (childIndex).position;
			currentTile = temp;
			lastDirection = "top";
		}
		else if (direction == "right")
		{
			GameObject temp = rightTiles.Pop ();
			temp.SetActive (true);
			temp.transform.position = currentTile.transform.GetChild (0).transform.GetChild (childIndex).position;
			currentTile = temp;
			lastDirection = "right";
		}
		//currentTile = (GameObject)Instantiate (tilePrefabs[randomDirectionIndex], currentTile.transform.GetChild (0).transform.GetChild (childIndex).position, Quaternion.identity);
	}


}
