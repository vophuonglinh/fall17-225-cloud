using System.Collections;
using UnityEngine;

//recycle tiles 20:20
public class TileScript : MonoBehaviour
{

    private float delay = 6;
    private float cloudHeight;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            TileController.Instance.SpawnTile();
            Recycle();
        }
    }

    void Recycle()
    {
        TileController.Instance.TopTiles.Push(gameObject);
    }
}
