using UnityEngine;
using System.Collections;

public class CDustFollow : MonoBehaviour
{
	public Transform source;

	private float originalRate;

	// Use this for initialization
	void Start()
	{
		originalRate = GetComponent<ParticleSystem>().emissionRate;
	}

	// Update is called once per frame
	void Update()
	{
		if (source == null)
			return;

		var r = new Ray(
			source.position + new Vector3(0, -2, 0),
			Vector3.down);

		RaycastHit hitInfo;
		if (Physics.Raycast(r, out hitInfo, 5))
		{
			transform.position = hitInfo.point;
			GetComponent<ParticleSystem>().emissionRate = originalRate;
		}
		else
		{
			GetComponent<ParticleSystem>().emissionRate = 0f;
		}
	}
}
