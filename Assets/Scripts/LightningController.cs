using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
	[SerializeField] float blackWaitTime = 1.5f;
	[SerializeField] float whiteWaitTime = 0.25f;

	Renderer rend;

	void Awake()
	{
		rend = GetComponent<Renderer>();
		StartCoroutine(LightningRoutine());
	}

	IEnumerator LightningRoutine()
	{
		bool isWhite = true;

		while (true)
		{
			isWhite = !isWhite;

			if (isWhite)
				rend.material.color = Color.white;
			else
				rend.material.color = Color.black;

			yield return new WaitForSeconds(isWhite ? whiteWaitTime : blackWaitTime);
		}
	}
}
