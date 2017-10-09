using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] Transform follow;

	Vector3 offset;

	void Start()
	{
		offset = transform.position - follow.position;
	}

	void LateUpdate()
	{
		transform.position = follow.position + offset;
	}
}
