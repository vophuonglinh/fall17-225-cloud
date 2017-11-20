using System.Collections;
using UnityEngine;

//recycle tiles 20:20
public class TileScript : MonoBehaviour {

	private float delay = 6;
    private float cloudHeight;
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

    }
		
	void OnTriggerExit(Collider other)
	{
        //Debug.Log("on trigger exit happens");
		if (other.tag == "Player") {
            //Debug.Log("on trigger exit happens");
            TileController.Instance.SpawnTile ();
			StartCoroutine (Recycle());
		}
	}



	IEnumerator Recycle()
	{
        Debug.Log("RECYCLING TILE");
//		yield return new WaitForSeconds(fallDelay);
		//GetComponent<Rigidbody>.isKinematic = false;
		yield return new WaitForSeconds(delay);
		TileController.Instance.checkHideCloud();
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