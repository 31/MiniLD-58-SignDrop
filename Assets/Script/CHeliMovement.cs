using System;
using UnityEngine;
using System.Collections;

public class CHeliMovement : MonoBehaviour
{
	private float mainRotorRate = 0f;

	private float collective = 1f;
	private const float defaultCollective = 0.9f;

	private float tailRotorRateFraction = 1f;

	private const float mainRotorRadius = 2f;
	private readonly Vector3 mainRotorCenter = new Vector3(0f, 1f, 0f);
	private const float floatingForce = 9.81f;

	private const float cyclicEffect = 0.1f;

	private const float generalAirFriction = 0.05f;

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
		float collectiveIn = Input.GetAxis("Collective");

		rollIn = Mathf.Clamp(rollIn, -1f, 1f);
		pitchIn = Mathf.Clamp(pitchIn, -1f, 1f);
		collectiveIn = Mathf.Clamp(collectiveIn, -1f, 1f);

		collective = defaultCollective + collectiveIn;

		//transform.Rotate(pitchIn, rollIn, 0f);
		//rigidbody.AddRelativeTorque(-pitchIn, rollIn, 0f);

		var mainFront = transform.TransformPoint(mainRotorCenter + new Vector3(0f, 0f, mainRotorRadius));
		var mainBack = transform.TransformPoint(mainRotorCenter + new Vector3(0f, 0f, -mainRotorRadius));
		var mainLeft = transform.TransformPoint(mainRotorCenter + new Vector3(mainRotorRadius, 0f, 0f));
		var mainRight = transform.TransformPoint(mainRotorCenter + new Vector3(-mainRotorRadius, 0f, 0f));

		var mainUp = transform.TransformPoint(mainRotorCenter) - transform.position;


		// Approximate a rotor by applying force to the four cardinal points on the rotor depending
		// on cyclic controls.

		ApplyBladeForce(mainUp, pitchIn, mainFront);
		ApplyBladeForce(mainUp, -pitchIn, mainBack);
		ApplyBladeForce(mainUp, -rollIn, mainLeft);
		ApplyBladeForce(mainUp, rollIn, mainRight);

		// Relative wind speed squared.
		float windSpeed2 = rigidbody.velocity.sqrMagnitude;

		// Generally oppose movement.
		rigidbody.AddForce(-rigidbody.velocity * windSpeed2 * generalAirFriction);

		// Apply torque towards movement vector.
		//var diff = Quaternion.Angle(rigidbody.rotation;

		//rigidbody.AddRelativeForce(9.81f * Vector3.up * mainRotorRate);
	}

	private void ApplyBladeForce(Vector3 mainUp, float cyclicFrac, Vector3 bladePos)
	{
		rigidbody.AddForceAtPosition(
			floatingForce * mainUp * (collective + cyclicFrac * cyclicEffect) * 0.25f,
			bladePos);
	}
}
