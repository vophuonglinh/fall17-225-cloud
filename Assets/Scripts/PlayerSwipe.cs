using UnityEngine;

namespace Lean.Touch
{
	// This script will push a rigidbody around when you swipe
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerSwipe : MonoBehaviour
	{

		protected virtual void OnEnable()
		{
			// Hook into the events we need
			LeanTouch.OnFingerSwipe += OnFingerSwipe;
		}

		protected virtual void OnDisable()
		{
			// Unhook the events
			LeanTouch.OnFingerSwipe -= OnFingerSwipe;
		}

		public void OnFingerSwipe(LeanFinger finger)
		{

			Vector2 distance = finger.SwipeScaledDelta;
      if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
      {           // check for horizontal swipes
          if (distance.x < 0)
          {
              Turn("Left");
          }
          else if (distance.x > 0)
          {
              Turn("Right");
          }
      }
      else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
      {
				Debug.Log("Vertical Swipe");
          if (distance.y > 0)
          {
              Turn("Up");
          }
          else
          {
              Turn("Down");
          }
      }
    }

		void Turn(string dir)
    {
			var rb = GetComponent<Rigidbody>();
			float angle = 90.0f;
			Vector3 axis = Vector3.up;
        if (dir == "Left")
        {
						transform.RotateAround(rb.position, Vector3.up, -90.0f);
						rb.velocity = Quaternion.Euler(0, -90, 0) * rb.velocity;
        }
        else if (dir == "Right")
        {
						transform.RotateAround(rb.position, Vector3.up, 90.0f);
						rb.velocity = Quaternion.Euler(0, 90, 0) * rb.velocity;
        }
        else if (dir == "Up")
        {
            transform.Translate(Vector3.up * 10, Space.World);
        }
        else if (dir == "Down")
        {
            transform.Translate(Vector3.down * 10, Space.World);
        }
    }
	}
}
