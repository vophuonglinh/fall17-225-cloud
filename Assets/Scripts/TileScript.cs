using System.Collections;
using UnityEngine;

//recycle tiles 20:20
public class TileScript : MonoBehaviour {

	private float delay = 6;
    private GameObject player;
    private float cloudHeight;
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject cloud = GameObject.FindGameObjectWithTag("Cloud");
        float cloudHeight = cloud.transform.lossyScale.y;
        string message = "Cloud height is ";
        message = string.Format("{0} {1}", message, cloudHeight.ToString());
        Debug.Log(message);

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 playerPosition = gameObject.transform.position;
        Vector3 tilePosition = gameObject.transform.position;
        //compare these positions and if player is in the x,z position range of the tile, then recycle
        Vector3 tileWidth = gameObject.transform.lossyScale;

    }
		
	void OnTriggerExit(Collider other)
	{
        //TODO: ELENA: rewrite to instead use x and z position to detect recycle
        Debug.Log("on trigger exit happens");
		if (other.tag == "Player") {
            Debug.Log("on trigger exit happens");
            TileController.Instance.SpawnTile ();
            Recycle();
		}
	}

	void Recycle()
	{
		switch (gameObject.name)
		{
		case "LeftTile":
			TileController.Instance.LeftTiles.Push(gameObject);  // add it back to the tile stack to recycle it
			//Debug.Log("pushed left tile back to stack");
			break;
		case "TopTile":
			TileController.Instance.TopTiles.Push(gameObject);
			//Debug.Log("pushed top tile back to stack");
			break;
		case "RightTile":
			TileController.Instance.RightTiles.Push(gameObject);
			//Debug.Log("pushed right tile back to stack");
			break;
		}
    }
}