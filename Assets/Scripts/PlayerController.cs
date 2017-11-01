using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public float speed;
	public GameController gamecontroller;

	void Start(){
		rb = GetComponent<Rigidbody> ();

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gamecontroller = gameControllerObject.GetComponent <GameController>();
		Debug.Log ("Start!");

	}
	void Update ()
	{
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.AddForce (movement * speed);
	}

	// collision with the clouds

	//void onTriggerEnter(Collider other) {
	void OnParticleCollision(GameObject other){
		if (other.gameObject.CompareTag("Cloud")) {
			Debug.Log ("collided!");
			gamecontroller.GameOver();
			gamecontroller.DeleteAll ();
			Debug.Log ("Game Over!");
			//Destroy(other.gameObject);
			//Destroy(gameObject);
		}
	}
		

}
