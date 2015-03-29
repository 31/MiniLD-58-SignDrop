using UnityEngine;

public class CStick : MonoBehaviour
{
	public Vector3 fullLocalPosition;

	public float x, y;

	// Use this for initialization
	void Start()
	{
		fullLocalPosition = transform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		transform.localPosition = Vector3.Lerp(
			transform.localPosition,
			fullLocalPosition +
			Vector3.up * y * 0.3f / CHeloMovement.rollLessFraction +
			Vector3.right * x * 0.3f,
			Time.deltaTime * 10f);
	}
}
