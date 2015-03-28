using UnityEngine;
using System.Collections;

public class CFollowCam : MonoBehaviour
{
	public enum CameraMode
	{
		Follow,
		Cockpit,
		Overhead,
	}

	public CHeloLinkage helo;

	public Vector3 followOffset;
	public Vector3 cockpitOffset;

	public CameraMode mode;

	public GameObject oceanBlocker;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		var heliMovement = helo.GetComponent<CHeloMovement>();

		switch (mode)
		{
			case CameraMode.Follow:
				Vector3 from = helo.transform.TransformPoint(followOffset);
				Vector3 look = helo.transform.position;

				transform.position = Vector3.Lerp(transform.position, from, Time.deltaTime * 8f);
				transform.LookAt(look);
				break;

			case CameraMode.Overhead:
				transform.position = Vector3.Lerp(
					transform.position,
					helo.transform.position + new Vector3(0, 20, 0),
					Time.deltaTime * 8f);
				transform.LookAt(helo.transform.position, helo.transform.forward);
				break;

			case CameraMode.Cockpit:
				transform.position = helo.transform.TransformPoint(cockpitOffset);
				transform.rotation = Quaternion.Lerp(
					transform.rotation,
					helo.transform.rotation *
						Quaternion.Euler(0, Input.GetAxis("CockpitLookHorizontal") * 75, 0) *
						Quaternion.Euler(Input.GetAxis("CockpitLookVertical") * 45, 0, 0),
					Time.deltaTime * 6f);
				break;
		}

		helo.instrumentation.SetActive(mode == CameraMode.Cockpit);

		if (Input.GetButtonDown("CycleViews"))
		{
			mode++;
			mode = (CameraMode)((int)mode % 3);
		}

		if (transform.position.y < 1.2f)
		{
			oceanBlocker.SetActive(true);
		}
		else
		{
			oceanBlocker.SetActive(false);
		}
	}
}
