using UnityEngine;
using System.Collections;

public class CDropIndicator : MonoBehaviour
{
	public Material afterDropMaterial;

	public CLandingZone shownZone;

	private bool showingDropped;

	public Light light;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!showingDropped && shownZone.dropped)
		{
			light.color = new Color(0, 1, 0);
			GetComponent<MeshRenderer>().material = afterDropMaterial;
			showingDropped = true;
		}
	}
}
