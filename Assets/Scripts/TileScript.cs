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
        //Debug.Log("on trigger exit happens");
        if (other.tag == "Player")
        {
            //Debug.Log("on trigger exit happens");
            TileController.Instance.SpawnTile();
            Recycle();
        }
    }

    void Recycle()
    {
        TileController.Instance.checkHideCloud();
        TileController.Instance.TopTiles.Push(gameObject);
        //Debug.Log("pushed top tile back to stack");
    }
}
