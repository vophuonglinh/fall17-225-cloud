using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public float speed;
	public GameController gamecontroller;
    private int count;
    public Text countText;

    void Start(){
		rb = GetComponent<Rigidbody> ();
        count = 0;
        SetCountText();
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boost"))
        {
            other.gameObject.SetActive(false);
            Debug.Log("count");
            count += 1;
            SetCountText();
        }
    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }


}
