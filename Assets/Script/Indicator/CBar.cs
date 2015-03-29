using UnityEngine;
using System.Collections;

public class CBar : MonoBehaviour
{
	public Vector3 fullLocalScale;
	public Vector3 fullLocalPosition;

	public float input;

	// Use this for initialization
	void Start()
	{
		fullLocalScale = transform.localScale;
		fullLocalPosition = transform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 newLocalScape = fullLocalScale;
		newLocalScape.Scale(new Vector3(1f, input * fullLocalScale.y, 1f));
		transform.localScale = newLocalScape;

		transform.localPosition = fullLocalPosition + Vector3.up * input * 0.5f;
	}
}
