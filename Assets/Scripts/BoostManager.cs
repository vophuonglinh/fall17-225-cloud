using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    public GameObject boostPrefab;
    public GameObject obstaclePrefab;
    private GameObject[] curTiles;
    // for recycling boosts that died out
    public Stack<GameObject> boosts = new Stack<GameObject>();
    public Stack<GameObject> obstacles = new Stack<GameObject>();
    private const int POOL_SIZE_BOOSTS = 18;
    private const int POOL_SIZE_OBSTACLES= 6;
    //private const int SPAWN_CHANCE = 4;
    private const int OBSTACLE_SPAWN_CHANCE = 2;
    private const int BOOST_SPREADING_SCALE = 7;
    private const int BOOST_GENERATE_DELAY = 1;
    private const string TAG_FOR_NONCURRENT = "NotCurrent";
    private const string TAG_FOR_CURRENT = "CurrentTile";
    private static BoostManager instance;


    public static BoostManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BoostManager>();
            }
            return instance;
        }
    }
    // Use this for initialization
    void Start()
    {
        CreatePool(POOL_SIZE_BOOSTS, true);
        CreatePool(POOL_SIZE_OBSTACLES, false);
        StartCoroutine(generate());
    }

    IEnumerator generate()
    {
        while (true)
        {
            curTiles = GameObject.FindGameObjectsWithTag(TAG_FOR_CURRENT);
            //loop over the array with objects that have the currenttile tag
            int i = 0;
            foreach (GameObject curTile in curTiles)
            {
                spawnBoostsOrObstacles(curTile,true);
                curTile.gameObject.tag = TAG_FOR_NONCURRENT;
                if (i % OBSTACLE_SPAWN_CHANCE == 0) {
                    spawnBoostsOrObstacles(curTile, false);
                }
                i++;
            }
            yield return new WaitForSeconds(BOOST_GENERATE_DELAY);
        }
    }


    // Update is called once per frame
    void Update()
    {


    }

    //generate a stack of boosts
    private void CreatePool(int amount, bool isBoost)
    {
        for (int i = 0; i < amount; i++)
        {
            if (isBoost)
            {
                boosts.Push(Instantiate(boostPrefab));
            }
            else {
                obstacles.Push(Instantiate(obstaclePrefab));
            }
            
        }
    }


    //spawn boosts using the position, and pop the boost out of the stack
    public void spawnBoostsOrObstacles(GameObject tile, bool isBoost)
    {
        if (boosts.Count == 0 && isBoost)
        {
            CreatePool(POOL_SIZE_BOOSTS, isBoost);
        }

        else if (obstacles.Count == 0 && !isBoost) {
            CreatePool(POOL_SIZE_OBSTACLES, isBoost);
        }
        Vector3 position = randomPositionOverTile(tile, BOOST_SPREADING_SCALE);
        GameObject temp = isBoost ? boosts.Pop() : obstacles.Pop();
        temp.transform.position = position;
        temp.SetActive(true);

    }

    //make a random position according to the position of the current tile, and based on the scale needed
    public Vector3 randomPositionOverTile(GameObject tile, int scatter)
    {
        float x = tile.transform.GetChild(0).position.x + Random.Range(-scatter, scatter) * scatter;
        float y= tile.transform.GetChild(0).position.y + Random.Range(scatter, 4*scatter) * scatter;
        float z = tile.transform.GetChild(0).position.z + Random.Range(-scatter, scatter) * scatter;
        return new Vector3(x, y, z);
    }

}
