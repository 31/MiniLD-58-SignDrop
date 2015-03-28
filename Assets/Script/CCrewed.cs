using UnityEngine;
using System.Collections;

public class CCrewed : MonoBehaviour
{
	public GameObject crewPrefab;

	public Transform crewSlotHolder;

	public Transform[] crewSlots;
	public GameObject[] attachedCrew;

	// Use this for initialization
	void Start()
	{
		crewSlots = new Transform[crewSlotHolder.childCount];
		for (int i = 0; i < crewSlotHolder.childCount; i++)
		{
			crewSlots[i] = crewSlotHolder.GetChild(i);
		}

		attachedCrew = new GameObject[crewSlotHolder.childCount];
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position.y < -10f)
		{
			Destroy(this);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.name == "Home")
		{
			for (int i = 0; i < attachedCrew.Length; i++)
			{
				if (attachedCrew[i] == null || attachedCrew[i].GetComponent<CCrewDisembark>().dropped)
				{
					var slot = crewSlots[i];

					var go = Instantiate(crewPrefab);

					go.transform.position = crewSlots[i].transform.position;
					go.transform.rotation = crewSlots[i].transform.rotation * transform.rotation;

					var joint = go.GetComponent<ConfigurableJoint>();

					joint.connectedAnchor = transform.InverseTransformPoint(slot.TransformPoint(Vector3.zero));
					joint.connectedBody = GetComponent<Rigidbody>();

					attachedCrew[i] = go;
				}
			}
		}
	}
}
