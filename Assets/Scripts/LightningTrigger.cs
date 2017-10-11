using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrigger : MonoBehaviour
{
	[SerializeField] float directionAmount = 1;
	[SerializeField] float influenceSpeed = 1;
	[SerializeField] AnimationCurve curve;

	bool inTrigger = false;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
			inTrigger = true;
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
			inTrigger = false;
	}

	public void InfluenceCat()
	{
		if (!inTrigger) return;

		GameObject cat = GameObject.FindWithTag("Player");
		CatController catController = cat.GetComponent<CatController>();

		catController.StartCoroutine(catController.SetInfluenceOverT(curve, Vector3.left * directionAmount, influenceSpeed));
	}
}
