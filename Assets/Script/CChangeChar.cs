using System;
using UnityEngine;
using System.Collections;

public class CChangeChar : MonoBehaviour
{
	public float until;

	// Use this for initialization
	void Start()
	{
		until = UnityEngine.Random.value;
	}

	// Update is called once per frame
	void Update()
	{
		until -= Time.deltaTime;
		if (until <= 0f)
		{
			until = UnityEngine.Random.value;
			var off = renderer.material.mainTextureOffset += new Vector2(
				UnityEngine.Random.value,
				UnityEngine.Random.value);

			off.x %= 1f;
			off.y %= 1f;

			renderer.material.mainTextureOffset = off;
		}
	}
}
