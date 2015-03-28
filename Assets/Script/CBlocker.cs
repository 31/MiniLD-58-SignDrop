using System.Linq;
using UnityEngine;
using System.Collections;

public class CBlocker : MonoBehaviour
{
	public CFollowCam cam;

	public Transform goal;

	public CLandingZone[] zones;

	private float guardDistance;

	// Use this for initialization
	void Start()
	{
		var d = transform.position - goal.position;
		d.y = 0;
		guardDistance = d.magnitude;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		var d = (cam.helo.transform.position - goal.position).normalized * guardDistance;
		d.y = 0;
		var p = goal.position + d;

		var targetRot = Quaternion.LookRotation(d);

		int dropped = zones.Count(z => z.dropped);
		if (dropped == 0)
		{
			transform.rotation = targetRot;
		}
		else
		{
			dropped++;
			transform.rotation = Quaternion.Lerp(
				transform.rotation,
				targetRot,
				1f / (dropped * dropped * 5f));
		}
	}
}
