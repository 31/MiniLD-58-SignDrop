using UnityEngine;
using System.Collections;

public class CFollowCam : MonoBehaviour
{
	public GameObject helo;

	public Vector3 offset;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Vector3 from = helo.transform.TransformPoint(offset);
		Vector3 look = helo.transform.position;

		transform.position = Vector3.Lerp(transform.position, from, Time.deltaTime);
		transform.LookAt(look);
	}
}
