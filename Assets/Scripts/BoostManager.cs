using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    public GameObject boostPrefab;
    public GameObject obstaclePrefab;
    public GameObject lightningPrefab;
    public GameObject[] boostOptions;

    private GameObject[] curTiles;
    // for recycling boosts that died out
    public Stack<GameObject> boosts = new Stack<GameObject>();
    public Stack<GameObject> obstacles = new Stack<GameObject>();
    public Stack<GameObject> lightnings = new Stack<GameObject>();

    //initial stack size for the game objects
    private const int POOL_SIZE_BOOSTS = 8;
    private const int POOL_SIZE_OBSTACLES = 9;
    private const int POOL_SIZE_LIGHTNINGS = 5;

    //constants for identifying different objects
    private const int BOOST_NUM = 0;
    private const int OBSTACLE_NUM = 1;
    private const int LIGHTNING_NUM = 2;

    private const int OBSTACLE_SPAWN_CHANCE = 6;
    private const int LIGHTNING_SPAWN_CHANCE = 6;
    private const int BOOST_SPAWN_CHANCE = 2;
    private const int BOOST_SPREADING_SCALE = 7;
    private const int BOOST_GENERATE_DELAY = 1;

    private const string TAG_FOR_NONCURRENT = "NotCurrent";
    private const string TAG_FOR_CURRENT = "CurrentTile";

    private const int FIXED_LIGHTNING_POSITION = 120;
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
        CreatePool(POOL_SIZE_BOOSTS, BOOST_NUM);
        CreatePool(POOL_SIZE_OBSTACLES, OBSTACLE_NUM);
        CreatePool(POOL_SIZE_LIGHTNINGS, LIGHTNING_NUM);
        StartCoroutine(generate());
    }

    IEnumerator generate()
    {
        while (true)
        {
            curTiles = GameObject.FindGameObjectsWithTag(TAG_FOR_CURRENT);
            //loop over the array with objects that have the CurrentTile tag
            //Starting with two to avoid generating lightnings in the beginning
            int i = 2;
            foreach (GameObject curTile in curTiles)
            {
                spawnBoostsOrObstacles(curTile, BOOST_NUM);
                spawnBoostsOrObstacles(curTile, BOOST_NUM);
                spawnBoostsOrObstacles(curTile, BOOST_NUM);
                curTile.gameObject.tag = TAG_FOR_NONCURRENT;
                if (i % OBSTACLE_SPAWN_CHANCE == 0)
                {
                    spawnBoostsOrObstacles(curTile, OBSTACLE_NUM);
                }
                if (i % LIGHTNING_SPAWN_CHANCE == Random.Range(0, 4))
                {
                    spawnBoostsOrObstacles(curTile, LIGHTNING_NUM);
                }
                i++;
            }
            yield return new WaitForSeconds(BOOST_GENERATE_DELAY);
        }
    }


    //generate an initial stack of objects to start with
    private void CreatePool(int amount, int objectNum)
    {
        for (int i = 0; i < amount; i++)
        {
            if (objectNum == BOOST_NUM)
            {
                boosts.Push(Instantiate(boostOptions[i % boostOptions.Length]));
            }
            else if (objectNum == OBSTACLE_NUM)
            {
                obstacles.Push(Instantiate(obstaclePrefab));
            }
            else if (objectNum == LIGHTNING_NUM)
            {
                lightnings.Push(Instantiate(lightningPrefab));
            }

        }
    }


    //spawn boosts using the position, and pop the boost out of the stack
    public void spawnBoostsOrObstacles(GameObject tile, int objectNum)
    {
        if (boosts.Count == 0 && objectNum == BOOST_NUM)
        {
            CreatePool(POOL_SIZE_BOOSTS, BOOST_NUM);
        }

        else if (obstacles.Count == 0 && objectNum == OBSTACLE_NUM)
        {
            CreatePool(POOL_SIZE_OBSTACLES, OBSTACLE_NUM);
        }
        else if (obstacles.Count == 0 && objectNum == LIGHTNING_NUM)
        {
            CreatePool(POOL_SIZE_LIGHTNINGS, LIGHTNING_NUM);
        }
        Vector3 position = randomPositionOverTile(tile, BOOST_SPREADING_SCALE, objectNum);
        GameObject temp = getRightObject(objectNum);
        temp.transform.position = position;
        temp.SetActive(true);

    }

    private GameObject getRightObject(int objectNum)
    {
        if (objectNum == BOOST_NUM)
        {
            return boosts.Pop();
        }

        else if (objectNum == OBSTACLE_NUM)
        {
            return obstacles.Pop();
        }
        else
        {
            return lightnings.Pop();
        }
    }

    //make a random position according to the position of the current tile, and based on the scale needed
    public Vector3 randomPositionOverTile(GameObject tile, int scatter, int objectNum)
    {
        float x = tile.transform.GetChild(0).position.x + Random.Range(-scatter, scatter) * scatter;
        //the y position for the lightning should be fixed
        float y = objectNum == LIGHTNING_NUM ? FIXED_LIGHTNING_POSITION : tile.transform.GetChild(0).position.y + Random.Range(scatter, 4 * scatter) * scatter;
        float z = tile.transform.GetChild(0).position.z + Random.Range(-scatter, scatter) * scatter;
        return new Vector3(x, y, z);
    }
}
