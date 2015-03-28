using UnityEngine;
using System.Collections;

public class CHeloReset : MonoBehaviour
{
	public GameObject currentHelo;
	public GameObject heloPrefab;

	public CFollowCam mainCamera;

	// Use this for initialization
	void Start()
	{
		mainCamera = FindObjectOfType<CFollowCam>();

		currentHelo = FindObjectOfType<CHeloMovement>().gameObject;
		if (currentHelo == null)
		{
			CreateNewHelo();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Reset"))
		{
			Destroy(currentHelo);
			CreateNewHelo();
		}
	}

	void CreateNewHelo()
	{
		currentHelo = Instantiate(heloPrefab);
		currentHelo.transform.position = transform.position;
		currentHelo.transform.rotation = transform.rotation;

		mainCamera.helo = currentHelo.GetComponent<CHeloLinkage>();
	}
}
