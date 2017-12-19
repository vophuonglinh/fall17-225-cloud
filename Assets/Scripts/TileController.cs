using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject currentTile;
    public GameObject tilePrefab;

    private static TileController instance;
    private const string tagCur = "CurrentTile";
    private const string tagNotCur = "NotCurrent";

    private Stack<GameObject> topTiles = new Stack<GameObject>();
    private const int poolSize = 5;

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
        for (int i = 0; i < poolSize; i++)
        {
            SpawnTile();
        }
    }

    private void CreateTiles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            topTiles.Push(Instantiate(tilePrefab));
            topTiles.Peek().name = "TopTile";
            topTiles.Peek().SetActive(false);
        }


    }

    public Stack<GameObject> TopTiles
    {
        get { return topTiles; }
        set { topTiles = value; }
    }

    public void SpawnTile()
    {
        RefillTilePool();
        RecycleTile();
    }

    public void RecycleTile()
    {
        GameObject temp;
        temp = topTiles.Pop();
        temp.SetActive(true);
        //reposition temp tile to the top attatch point of the current tile
        temp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(1).position;
        temp.gameObject.tag = tagCur;
        currentTile = temp;
    }

    void RefillTilePool()
    {
        if (topTiles.Count < poolSize)
        {
            CreateTiles(poolSize - topTiles.Count);
        }
    }
}
