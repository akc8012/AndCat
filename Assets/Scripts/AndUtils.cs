using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndUtils : MonoBehaviour
{
	public static Vector3 InputWorldToCam(Transform cam, Vector3 stickDir, Quaternion floorRotation)
	{
		Vector3 cameraDir = cam.forward; cameraDir.y = 0.0f;
		Vector3 moveDir = Quaternion.FromToRotation(Vector3.forward, cameraDir) * stickDir;     // referential shift
		moveDir = floorRotation * moveDir;

		// fixes bug by flipping movement x around
		if (cam.eulerAngles.y >= 179.920f && cam.eulerAngles.y <= 180.08f)
			moveDir = new Vector3(-moveDir.x, moveDir.y, moveDir.z);

		//Debug.DrawRay(transform.position, moveDir.normalized * 3, Color.blue);

		return moveDir;
	}
}
