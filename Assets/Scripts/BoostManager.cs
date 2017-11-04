using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    public GameObject boostPrefab;
    private GameObject[] curTiles;
    // for recycling boosts that died out
    public Stack<GameObject> boosts = new Stack<GameObject>();
    private const int POOL_SIZE = 50;
    private const int SPAWN_CHANCE = 4;
    private const int SCALE = 6;
    private const int Y_POSITION = 20;
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
        CreatePool(POOL_SIZE);
        StartCoroutine(generate());
    }

    IEnumerator generate()
    {
        while (true)
        {
            // yield return new WaitForSeconds(3);
            curTiles = GameObject.FindGameObjectsWithTag(TAG_FOR_CURRENT);
            //currentTile = GameObject.FindWithTag("CurrentTile");
            //loop over the array with objects that have the currenttile tag
            int i = 0;
            Debug.Log("NUmber of CurrentTile matches:" + curTiles.Length);
            foreach (GameObject curTile in curTiles)
            {
                //// spread the boosts out
                //if (i % 2 == 0) {
                //    i++;
                //    continue;
                //}
                spawnBoosts(curTile);
                curTile.gameObject.tag = TAG_FOR_NONCURRENT;
                i++;
                //TODO for further boosts, need longer life.
                //TODO recylce the boosts when it dies out or just get hit.
            }
            //CreatePool(POOL_SIZE);
            yield return new WaitForSeconds(3);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    //generate a stack of boosts
    private void CreatePool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            boosts.Push(Instantiate(boostPrefab));
            //boosts.Peek().SetActive(false);
        }
    }


    //spawn boosts using the position, and pop the boost out of the stack
    public void spawnBoosts(GameObject tile)
    {
        if (boosts.Count == 0)
        {
            CreatePool(POOL_SIZE);
        }
        //generate a bigger span if make multiple boosts
        Vector3 position = randomPositionOverTile(tile, SCALE);
        GameObject temp = boosts.Pop();
        temp.SetActive(true);

        //Boost.instance.boostLife += position.z * 1f;
        //Boost.Instance.life = Boost.Instance.life + position.z * 0.5f;
        //test
        //Debug.Log(Boost.instance.boostLife);
        temp.transform.position = position;
        //temp.SetActive(true);
    }

    //getter for boosts stack
    public Stack<GameObject> Boosts
    {
        get { return boosts; }
        set { boosts = value; }
    }


    //make a random position according to the position of the current tile, and based on the scale needed
    public Vector3 randomPositionOverTile(GameObject tile, int scatter)
    {
        float x = tile.transform.GetChild(0).position.x + Random.Range(-scatter, scatter) * scatter;
        float y = Y_POSITION;
        float z = tile.transform.GetChild(0).position.z + Random.Range(-scatter, scatter) * scatter;
        return new Vector3(x, y, z);
    }

}
