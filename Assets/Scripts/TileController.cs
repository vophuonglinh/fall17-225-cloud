using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{

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
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TileController>();
            }
            return instance;
        }
    }

    void Start()
    {
        minSegmentLength = 5;
        tileHistory = new List<char>(minSegmentLength);
        for (int i = 0; i < minSegmentLength; i++)
        {
            tileHistory.Add('t'); //initialize tile history with 't' for top tile
        }
        CreateTiles(minSegmentLength, 't');
        for (int i = 0; i < minSegmentLength; i++)
        {
            SpawnTile("top");
        }
    }

    public void CreateTiles(int amount, char type)
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
        get { return leftTiles; }
        set { leftTiles = value; }
    }
    public Stack<GameObject> TopTiles
    {
        get { return topTiles; }
        set { topTiles = value; }
    }
    public Stack<GameObject> RightTiles
    {
        get { return rightTiles; }
        set { rightTiles = value; }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnTile(string preferredDirection = "")
    {
        checkEmpty();
        string direction = "top";
        bool foundTurn = false;
        int childIndex = 1;
        char lastMove = tileHistory[0];

        for (int i = 0; i < minSegmentLength; i++)
        {
            if (tileHistory[i] != lastMove)
            {
                foundTurn = true;
            }
        }

        if (foundTurn)
        {
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
            else if (lastMove == 'r')
            {
                childIndex = 2;
                direction = "right";
            }

        }

        else
        {
            int randomDirectionIndex = Random.Range(0, tilePrefabs.Length);
            string randomDirectionTilePrefab = tilePrefabs[randomDirectionIndex].name;

            switch (randomDirectionTilePrefab)
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
        GameObject temp;
        if (direction == "left")
        {
            tileHistory.Insert(0, 'l');
            tileHistory.RemoveAt(tileHistory.Count - 1);
            temp = leftTiles.Pop();
        }
        else if (direction == "top")
        {
            tileHistory.Insert(0, 't');
            tileHistory.RemoveAt(tileHistory.Count - 1);
            temp = topTiles.Pop();
        }
        else
        {
            tileHistory.Insert(0, 'r');
            tileHistory.RemoveAt(tileHistory.Count - 1);
            temp = rightTiles.Pop();
        }
        temp.SetActive(true);
        temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(childIndex).position;


        currentTile = temp;
        temp.gameObject.tag = tagCur;


    }

    public void checkHideCloud() {
        GameObject[] clouds = GameObject.FindGameObjectsWithTag("Cloud");
        Vector3 tilePos = currentTile.transform.position;
        foreach (GameObject cloud in clouds)
        {
            Vector3 cloudPos = cloud.transform.position;

            var xDiff = Mathf.Abs(cloudPos.x - tilePos.x);
            var zDiff = Mathf.Abs(cloudPos.z - tilePos.z);
            //Debug.Log(string.Format("{0}, {1}", "xDiff ", xDiff.ToString()));
            //Debug.Log(string.Format("{0}, {1}", "zDiff ", zDiff.ToString()));

            //Debug.Log(string.Format("{0}, {1}", "cloud Pos ", cloudPos.ToString()));
            //Debug.Log(string.Format("{0}, {1}", "tile Pos ", tilePos.ToString()));


            if (xDiff < 70 && zDiff < 70)
            {
                //Debug.Log(string.Format("{0}, {1}", "xDiff hit ", xDiff.ToString()));
                //Debug.Log(string.Format("{0}, {1}", "zDiff hit ", zDiff.ToString()));

                cloud.transform.Rotate(new Vector3(45, 0, 45));
                cloud.SetActive(false);
                
            }
        }

}

    void checkEmpty()
    {
        if (leftTiles.Count == 0)
        {
            CreateTiles(10, 'l');
        }
        if (rightTiles.Count == 0)
        {
            CreateTiles(10, 'r');

        }
        if (topTiles.Count == 0)
        {
            CreateTiles(10, 't');
        }
    }
}
