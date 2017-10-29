using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateController : MonoBehaviour
{
	enum CameraState { Hallway, Point }
	CameraState camState = CameraState.Hallway;

	CameraController hallwayCam;
	PointCam pointCam;

	void Awake()
	{
		hallwayCam = GetComponent<CameraController>();
		pointCam = GetComponent<PointCam>();

		SetHallwayCam();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
			ToggleState();
	}

	public void ToggleState()
	{
		if (camState == CameraState.Hallway)
			SetPointCam();
		else
			SetHallwayCam();
	}

	public void SetHallwayCam()
	{
		camState = CameraState.Hallway;

		pointCam.enabled = false;
		hallwayCam.enabled = true;
	}

	public void SetPointCam()
	{
		camState = CameraState.Point;

		hallwayCam.enabled = false;
		pointCam.enabled = true;
	}
}
