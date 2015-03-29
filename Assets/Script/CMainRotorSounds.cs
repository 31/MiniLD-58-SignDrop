using UnityEngine;
using System.Collections;

public class CMainRotorSounds : MonoBehaviour
{
	public CHeloMovement helo;

	public AudioSource fwah;
	public AudioSource fwoo;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		fwah.pitch = 1f + (helo.collective - 1f) * 0.15f;
		fwah.volume = Mathf.Min(0.7f, Mathf.Sqrt(helo.verticalForceResult) * 0.005f + 0.3f);

		var body = GetComponent<Rigidbody>();

		var sqVel = Mathf.Sqrt(body.velocity.magnitude);

		fwoo.volume = (body.velocity.magnitude + 8) * 0.01f;

		fwoo.pitch += UnityEngine.Random.Range(-1f, 1f) * sqVel * 0.01f;
		fwoo.pitch = Mathf.Lerp(fwoo.pitch, 1f, Time.deltaTime);
		fwoo.pitch = Mathf.Clamp(fwoo.pitch, 0.5f, 1.5f);
	}
}
