using System.Collections;
using UnityEngine;

//recycle tiles 20:20
public class TileScript : MonoBehaviour
{

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
