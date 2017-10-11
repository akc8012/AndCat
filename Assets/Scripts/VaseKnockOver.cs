using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseKnockOver : MonoBehaviour
{
	[SerializeField] float knockForce = 1;
	[SerializeField] float directionAmount = 1;
	[SerializeField] float influenceSpeed = 1;
	[SerializeField] AnimationCurve curve;

	Rigidbody rb;

	Vector3 startPos;
	bool gotKnocked = false;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
			Reset();
	}

	public void KnockOver()
	{
		if (gotKnocked) return;

		rb.AddForce(Vector3.left * knockForce, ForceMode.VelocityChange);
		gotKnocked = true;

		GetComponent<AudioSource>().Play();

		InfluenceCat();
	}

	void InfluenceCat()
	{
		GameObject cat = GameObject.FindWithTag("Player");
		Vector3 direction = cat.transform.position - transform.position;
		direction.y = 0; direction.Normalize();

		CatController catController = cat.GetComponent<CatController>();

		catController.StartCoroutine(catController.SetInfluenceOverT(curve, direction * directionAmount, influenceSpeed));
	}

	void Reset()
	{
		gotKnocked = false;
		transform.position = startPos;
		transform.rotation = Quaternion.identity;

		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}
}
