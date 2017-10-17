using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//recycle tiles 20:20
public class TileScript : MonoBehaviour {

	private float delay = 6;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") {
			TileController.Instance.SpawnTile ();
			StartCoroutine (Recycle());
		}
	}

	IEnumerator Recycle()
	{
//		yield return new WaitForSeconds(fallDelay);
		//GetComponent<Rigidbody>.isKinematic = false;
		yield return new WaitForSeconds(delay);
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