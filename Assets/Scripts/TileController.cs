using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{

    public GameObject currentTile;
    public GameObject[] tilePrefabs;

    private static TileController instance;
    private const string tagCur = "CurrentTile";
    private const string tagNotCur = "NotCurrent";
    private Stack<GameObject> topTiles = new Stack<GameObject>();

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
        for (int i = 0; i < 5; i++)
        {
            SpawnTile();
        }
    }

    private void CreateTiles(int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            topTiles.Push(Instantiate(tilePrefabs[2]));
            topTiles.Peek().name = "TopTile";
            topTiles.Peek().SetActive(false);
        }


    }

    public Stack<GameObject> TopTiles
    {
        get { return topTiles; }
        set { topTiles = value; }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnTile()
    {
        RefillTilePool();


        int childIndex = 1;
        string direction = "top";

        RecycleTile(direction, childIndex);
    }

    public void RecycleTile(string direction, int childIndex)
    {
        GameObject temp;

        temp = topTiles.Pop();


        temp.SetActive(true);
        temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(childIndex).position;
        temp.gameObject.tag = tagCur;
        currentTile = temp;
    }

    public void checkHideCloud()
    {
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

    void RefillTilePool()
    {
        int poolSize = 10;
        CreateTiles(poolSize - topTiles.Count);
    }
}
