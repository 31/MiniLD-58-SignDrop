using UnityEngine;
using System.Collections;

public class CHeliMovement : MonoBehaviour
{
	private float mainRotorRate = 0f;

	private float tailRotorRateFraction = 1f;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
	}

	void FixedUpdate()
	{
		float rollIn = Input.GetAxis("Roll");
		float pitchIn = Input.GetAxis("Pitch");

		//transform.Rotate(pitchIn, rollIn, 0f);
		rigidbody.AddRelativeTorque(-pitchIn, rollIn, 0f);

		// Relative wind speed squared.
		float windSpeed2 = rigidbody.velocity.sqrMagnitude;

		rigidbody.AddRelativeForce(9.81f * Vector3.up * 1.05f);
	}
}
