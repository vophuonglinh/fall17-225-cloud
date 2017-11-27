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
              HorizontalTurn(-90.0f);
          }
          else if (distance.x > 0)
          {
              HorizontalTurn(90.0f);
          }
      }
      else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
      {						// check for vertical swipes
          if (distance.y > 0)
          {
              VerticalTurn(10);
          }
          else
          {
              VerticalTurn(-10);
          }
      }
    }

		private void HorizontalTurn(float angle)
		{
			var rb = GetComponent<Rigidbody>();

			transform.RotateAround(rb.position, Vector3.up, angle);
			rb.velocity = Quaternion.Euler(0, -90, 0) * rb.velocity;
		}

		private void VerticalTurn(int step)
		{
			transform.Translate(Vector3.up * step, Space.World);
		}

	}
}
