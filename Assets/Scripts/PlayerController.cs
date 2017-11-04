using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public float speed;

	bool isStarted = false;		// test if game has started

	// variables for swipe input
	public float maxTime;
	public float minSwipeDist;

	float startTime;
	float endTime;

	Vector3 startPos;
	Vector3 endPos;
	float swipeDist;
	float swipeTime;

	public GameController gamecontroller;
    private int count;
    public Text countText;

    void Start(){
		rb = GetComponent<Rigidbody> ();
<<<<<<< HEAD
        count = 0;
        SetCountText();
        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gamecontroller = gameControllerObject.GetComponent <GameController>();
		Debug.Log ("Start!");

=======

		// GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		// gamecontroller = gameControllerObject.GetComponent <GameController>();
		// Debug.Log ("Start!");
//Linh: I commented out 29:31
>>>>>>> 2162b6922e6841224fe2e2ca31923cd53aa6c3d4
	}


	void Update ()
	{
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				if (!isStarted) {					// if game hasn't started, player touch screen
					rb.AddForce (new Vector3 (0.0f, 0.0f, 100f) * speed);
					isStarted = true;
				} else {							// if game has started & player touch screen, start measuring to detect swipe
					startTime = Time.time;
					startPos = touch.position;
				}
			}

			else if (touch.phase == TouchPhase.Ended) {
				endTime = Time.time;
				endPos = touch.position;

				swipeDist = (endPos - startPos).magnitude;
				swipeTime = endTime - startTime;

				if (swipeTime < maxTime && swipeDist > minSwipeDist) {
					Swipe ();		// call method to move if player swipes
				}
			}
		}
  }


	void FixedUpdate()
	{
		// float moveHorizontal = Input.GetAxis ("Horizontal");
		// float moveVertical = Input.GetAxis ("Vertical");
		//
		// Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		// rb.AddForce (movement * speed);
	}

	void Swipe() {
		Vector2 distance = endPos - startPos;
		if (Mathf.Abs (distance.x) > Mathf.Abs (distance.y)) {			// check for horizontal swipes
			if (distance.x < 0) {
				Move ("Left");
			} else if (distance.x > 0) {
				Move ("Right");
			}
		}
	}

	void Move(string dir) {
		if (dir == "Left") {
			rb.AddForce (new Vector3 (rb.velocity.magnitude * -1, 0.0f, 0.0f) * speed);
		} else if (dir == "Right") {
			rb.AddForce (new Vector3 (rb.velocity.magnitude, 0.0f, 0.0f) * speed);
		}
	}

<<<<<<< HEAD
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
=======
	// collision with the clouds

// Linh: I commented out 98:103 + 106:107
	// void onTriggerEnter(Collider other) {
	// void OnParticleCollision(GameObject other){
	// 	if (other.gameObject.CompareTag("Cloud")) {
	// 		Debug.Log ("collided!");
	// 		gamecontroller.GameOver();
	// 		gamecontroller.DeleteAll ();
	// 		Debug.Log ("Game Over!");
	// 		//Destroy(other.gameObject);
	// 		//Destroy(gameObject);
		// }
	// }
>>>>>>> 2162b6922e6841224fe2e2ca31923cd53aa6c3d4


}
