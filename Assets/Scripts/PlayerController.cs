using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public float speed;
    bool isStarted = false;

	// variables for swipe input
	public float maxTime; 	
	public float minSwipeDist;

	float startTime;
	float endTime;

	Vector3 startPos;
	Vector3 endPos;
	float swipeDist;
	float swipeTime;

	void Start(){
		rb = GetComponent<Rigidbody> ();
	}


	void Update ()
	{
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				if (!isStarted) {
					rb.AddForce (new Vector3 (0.0f, 0.0f, 100f) * speed);
					isStarted = true;
				} else {
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
					Swipe ();
				}
			}
		}       
    }
		

	void FixedUpdate()
	{
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");
//
//		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//		rb.AddForce (movement * speed);
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
}
