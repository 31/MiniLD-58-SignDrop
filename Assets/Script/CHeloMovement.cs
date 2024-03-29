﻿using System;
using UnityEngine;
using System.Collections;

public class CHeloMovement : MonoBehaviour
{
	public GameObject rotor;

	private float mainRotorRate = 0f;

	public float collective = 1f;
	public float rollIn;
	public float pitchIn;
	public float yawIn;

	private const float defaultCollective = 0.9f;

	private float tailRotorRateFraction = 1f;

	private const float mainRotorRadius = 8.3f * 0.5f;
	private readonly Vector3 mainRotorCenter = new Vector3(0f, 2.5f, 0f);
	private readonly Vector3 tailRotorCenter = new Vector3(0f, 0f, -4f);

	private const float floatingForce = 9.81f;
	//private const float floatingForce = 9.81f;

	private const float cyclicEffect = 0.1f;
	public const float rollLessFraction = 0.6f;

	private const float generalAirFriction = 0.0001f;
	private const float verticalAirFriction = 0.8f;
	public float verticalForceResult = 0f;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		rotor.transform.Rotate(0f, Time.deltaTime * 800f, 0f);
	}

	void FixedUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		Rigidbody rigidbody = GetComponent<Rigidbody>();
		var rotorForceCenter = rigidbody.centerOfMass + mainRotorCenter;

		rollIn = Input.GetAxis("Roll");
		pitchIn = Input.GetAxis("Pitch");
		yawIn = Input.GetAxis("Yaw");
		float collectiveIn = Input.GetAxis("Collective");

		rollIn = Mathf.Clamp(rollIn, -1f, 1f);
		pitchIn = Mathf.Clamp(pitchIn, -1f, 1f) * rollLessFraction;
		yawIn = Mathf.Clamp(yawIn, -1f, 1f);
		collectiveIn = Mathf.Clamp(collectiveIn, -1f, 1f);

		collective = defaultCollective * (1f + collectiveIn);

		//transform.Rotate(pitchIn, rollIn, 0f);
		//rigidbody.AddRelativeTorque(-pitchIn, rollIn, 0f);

		var mainFront = transform.TransformPoint(rotorForceCenter + new Vector3(0f, 0f, mainRotorRadius));
		var mainBack = transform.TransformPoint(rotorForceCenter + new Vector3(0f, 0f, -mainRotorRadius));
		var mainLeft = transform.TransformPoint(rotorForceCenter + new Vector3(mainRotorRadius, 0f, 0f));
		var mainRight = transform.TransformPoint(rotorForceCenter + new Vector3(-mainRotorRadius, 0f, 0f));

		//var mainUp = (transform.TransformPoint(rotorForceCenter) - transform.position).normalized;
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
		rigidbody.AddForce(-rigidbody.velocity * windSpeed2 * generalAirFriction);

		// Apply torque towards movement vector.
		var localVel = transform.InverseTransformVector(rigidbody.velocity);

		var localPlanarVel = new Vector3(localVel.x, 0f, localVel.z);

		Vector3 torqueAxis;
		float torqueAngle;

		Quaternion
			.FromToRotation(Vector3.forward, localPlanarVel)
			.ToAngleAxis(out torqueAngle, out torqueAxis);

		if (windSpeed2 > 0.5f)
		{
			rigidbody.AddRelativeTorque(torqueAxis * Mathf.Sqrt(torqueAngle) * windSpeed2 * 0.2f);
		}

		// Make blades keep plane from going up and down, friction-wise.
		var vertDampForce = -Mathf.Sign(localVel.y) * (localVel.y * localVel.y) *
			verticalAirFriction * mainUp * rigidbody.mass;
		verticalForceResult = vertDampForce.magnitude;
		rigidbody.AddForce(vertDampForce);

		// Apply tail rotor.
		rigidbody.AddForceAtPosition(
			yawIn * transform.TransformVector(Vector3.left) * rigidbody.mass,
			transform.TransformPoint(rigidbody.centerOfMass + tailRotorCenter));
	}

	private void ApplyBladeForce(Vector3 mainUp, float cyclicFrac, Vector3 bladePos)
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddForceAtPosition(
			floatingForce * mainUp * (collective + cyclicFrac * cyclicEffect) * 0.25f * rigidbody.mass,
			bladePos);
	}
}
