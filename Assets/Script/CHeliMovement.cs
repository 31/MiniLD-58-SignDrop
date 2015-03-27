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
	//private const float floatingForce = 9.81f;

	private const float cyclicEffect = 0.05f;

	private const float generalAirFriction = 0.0005f;

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
		Rigidbody rigidbody = GetComponent<Rigidbody>();

		float rollIn = Input.GetAxis("Roll");
		float pitchIn = Input.GetAxis("Pitch");
		float collectiveIn = Input.GetAxis("Collective");

		rollIn = Mathf.Clamp(rollIn, -1f, 1f);
		pitchIn = Mathf.Clamp(pitchIn, -1f, 1f);
		collectiveIn = Mathf.Clamp(collectiveIn, -1f, 1f);

		collective = defaultCollective + collectiveIn * 0.4f;

		//transform.Rotate(pitchIn, rollIn, 0f);
		//rigidbody.AddRelativeTorque(-pitchIn, rollIn, 0f);

		var mainFront = transform.TransformPoint(mainRotorCenter + new Vector3(0f, 0f, mainRotorRadius));
		var mainBack = transform.TransformPoint(mainRotorCenter + new Vector3(0f, 0f, -mainRotorRadius));
		var mainLeft = transform.TransformPoint(mainRotorCenter + new Vector3(mainRotorRadius, 0f, 0f));
		var mainRight = transform.TransformPoint(mainRotorCenter + new Vector3(-mainRotorRadius, 0f, 0f));

		//var mainUp = (transform.TransformPoint(mainRotorCenter) - transform.position).normalized;
		var mainUp = transform.TransformVector(Vector3.up).normalized;


		// Approximate a rotor by applying force to the four cardinal points on the rotor depending
		// on cyclic controls.

		ApplyBladeForce(mainUp, pitchIn, mainFront);
		ApplyBladeForce(mainUp, -pitchIn, mainBack);
		ApplyBladeForce(mainUp, -rollIn, mainLeft);
		ApplyBladeForce(mainUp, rollIn, mainRight);

		// Relative wind speed squared.
		float windSpeed2 = rigidbody.velocity.sqrMagnitude;

		// Generally oppose movement.
		//rigidbody.AddForce(-rigidbody.velocity * windSpeed2 * generalAirFriction);

		// Apply torque towards movement vector.
		var localVel = transform.InverseTransformVector(rigidbody.velocity);
		var movementRot = Quaternion.LookRotation(localVel, Vector3.up);

		var localPlanarVel = new Vector3(localVel.x, 0f, localVel.z);

		Vector3 torqueAxis;
		float torqueAngle;

		Quaternion
			.FromToRotation(Vector3.forward, localPlanarVel)
			.ToAngleAxis(out torqueAngle, out torqueAxis);

		if (windSpeed2 > 0.5f)
		{
			rigidbody.AddRelativeTorque(torqueAxis * torqueAngle * windSpeed2 * 0.001f);
		}

		rigidbody.AddRelativeForce(9.81f * Vector3.up * mainRotorRate);
	}

	private void ApplyBladeForce(Vector3 mainUp, float cyclicFrac, Vector3 bladePos)
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddForceAtPosition(
			floatingForce * mainUp * (collective + cyclicFrac * cyclicEffect) * 0.25f * rigidbody.mass,
			bladePos);
	}
}
