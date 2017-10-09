using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseTrigger : MonoBehaviour
{
	[SerializeField] VaseKnockOver vase;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
			vase.KnockOver();
	}
}