using UnityEngine;
using System.Collections;

public class CWaterSlow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
	}

	void OnTriggerStay(Collider other)
	{
		// Slow down the intruder.
		var otherBody = other.GetComponentInParent<Rigidbody>();

		otherBody.AddForceAtPosition(
			-otherBody.velocity * 50f,
			other.transform.position);
	}
}
