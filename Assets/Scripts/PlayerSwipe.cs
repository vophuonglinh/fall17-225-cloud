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
              Move(Vector3.left);
          }
          else if (distance.x > 0)
          {
              Move(Vector3.right);
          }
      }
      else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
      {						// check for vertical swipes
          if (distance.y > 0)
          {
              Move(Vector3.up);
          }
          else
          {
              Move(Vector3.down);
          }
      }
    }

		private void Move(Vector3 axis)
		{
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(axis * 30);
            //transform.Translate(axis * 10, Space.World);
		}
	}
}
