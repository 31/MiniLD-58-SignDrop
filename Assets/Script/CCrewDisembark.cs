using UnityEngine;
using System.Collections;

public class CCrewDisembark : MonoBehaviour
{
	public bool dropped = false;

	public Material dropMaterial;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnCollisionEnter(Collision collision)
	{
		if (dropped)
			return;

		var landZone = collision.collider.gameObject.GetComponent<CLandingZone>();
		if (landZone != null)
		{
			dropped = true;
			Destroy(GetComponent<ConfigurableJoint>());

			var renderer = GetComponent<MeshRenderer>();
			renderer.material = dropMaterial;
		}
	}
}
