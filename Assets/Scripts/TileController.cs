using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

	public GameObject currentTile;
	public GameObject[] tilePrefabs;

	private static TileController instance;
    private const string tagCur = "CurrentTile";
    private const string tagNotCur = "NotCurrent";
    private Stack<GameObject> leftTiles = new Stack<GameObject>();
	private Stack<GameObject> topTiles = new Stack<GameObject>();
	private Stack<GameObject> rightTiles = new Stack<GameObject>();

	private int minSegmentLength;
	private List<char> tileHistory;


    //Instance so that TileScript can access inside TileScript
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

	void Start () {
		minSegmentLength = 5;
		tileHistory = new List<char> (minSegmentLength);
		for (int i = 0; i < minSegmentLength; i++) {
            tileHistory.Add('t'); //initialize tile history with 't' for top tile
		}
        for (int i = 0; i < minSegmentLength; i++) {
            SpawnTile ("top");
        }
	}

    private void CreateTiles(int amount, char type)
	{
        if (type == 'l')
        {
            for (int i = 0; i < amount; i++)
            {
			    leftTiles.Push(Instantiate(tilePrefabs[0]));
                leftTiles.Peek().name = "LeftTile";
                leftTiles.Peek().SetActive(false);
            }
        }
        else if (type == 't')
        {
            for (int i = 0; i < amount; i++)
            {
                topTiles.Push(Instantiate(tilePrefabs[2]));
                topTiles.Peek().name = "TopTile";
                topTiles.Peek().SetActive(false);
            }
        }
        else if (type == 'r')
        {
            for (int i = 0; i < amount; i++)
            {
                rightTiles.Push(Instantiate(tilePrefabs[1]));
                rightTiles.Peek().name = "RightTile";
                rightTiles.Peek().SetActive(false);
            }
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

	public void SpawnTile(string preferredDirection="")
	{
        RefillTilePool();
        string direction = "top";
		bool foundTurn = false;
		int childIndex = 1;
        char lastMove = tileHistory[0];

        for (int i = 0; i < minSegmentLength; i++) 
		{
			if (tileHistory [i] != lastMove) 
			{
				foundTurn = true;
			}
		}

        if (foundTurn) {
            if (lastMove == 't')
            {
                childIndex = 1;
                direction = "top";
            }
            else if (lastMove == 'l')
            {
                childIndex = 0;
                direction = "left";
            }
            else if (lastMove == 'r'){
                childIndex = 2;
                direction = "right";
            }

		}
        else
        {
			int randomDirectionIndex = Random.Range (0, tilePrefabs.Length); 
			string randomDirectionTilePrefab = tilePrefabs [randomDirectionIndex].name;

			switch (randomDirectionTilePrefab) {
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
		}
        if (preferredDirection.Length != 0)
        {
            if (preferredDirection == "top")
            {
                direction = "top";
                childIndex = 1;
            }
        }

        RecycleTile(direction, childIndex);
    }

    public void RecycleTile(string direction, int childIndex)
    {
        if (direction == "left")
        {
            tileHistory.Insert(0, 'l');
            tileHistory.RemoveAt(tileHistory.Count - 1);
            GameObject temp = leftTiles.Pop();
            temp.SetActive(true);
            temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(childIndex).position;
            currentTile = temp;
            temp.gameObject.tag = tagCur;
        }
        else if (direction == "top")
        {
            tileHistory.Insert(0, 't');
            tileHistory.RemoveAt(tileHistory.Count - 1);
            GameObject temp = topTiles.Pop();
            temp.SetActive(true);
            temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(childIndex).position;
            currentTile = temp;
            temp.gameObject.tag = tagCur;
        }
        else if (direction == "right")
        {
            tileHistory.Insert(0, 'r');
            tileHistory.RemoveAt(tileHistory.Count - 1);
            GameObject temp = rightTiles.Pop();
            temp.SetActive(true);
            temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(childIndex).position;
            currentTile = temp;
            temp.gameObject.tag = tagCur;
        }
    }

    void RefillTilePool()
    {
        int poolSize = 10;
        CreateTiles(poolSize - leftTiles.Count, 'l');
        CreateTiles(poolSize - rightTiles.Count, 'r');
        CreateTiles(poolSize - topTiles.Count, 't');
    }
}
