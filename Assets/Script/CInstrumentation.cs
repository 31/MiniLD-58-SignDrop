using UnityEngine;
using System.Collections;

public class CInstrumentation : MonoBehaviour
{
	public CBar throttle;
	public CStick stick;
	public CBar rudder;

	public CHeloMovement helo;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		throttle.input = helo.collective * 0.5f - 0.5f;

		rudder.input = helo.yawIn * 0.5f;

		stick.y = -helo.pitchIn;
		stick.x = helo.rollIn;
	}
}
