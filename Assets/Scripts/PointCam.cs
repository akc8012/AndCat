using UnityEngine;
using System.Collections;

public class PointCam : MonoBehaviour
{
	[Range(0, 20)] [SerializeField]
	float closeness = 1;

	Transform player;
	Vector3 startForward;

	void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		startForward = transform.forward;
	}

	void LateUpdate()
	{
		Vector3 relativePos = player.position - transform.position;
		Vector3 avgPos = (startForward.normalized * closeness + relativePos.normalized) / (2 + (closeness - 1));
		transform.rotation = Quaternion.LookRotation(closeness != 0 ? avgPos : relativePos);
	}
}
