using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] Transform follow;
	[SerializeField] float ForwardLockValue;
	[SerializeField] float LeftLockValue;

	Vector3 offset;

	public enum DirState { Forward, Left };
	DirState m_dirState = DirState.Forward;
	DirState dirState { get { return m_dirState; } set { StartCoroutine(MoveThenSetNewState(value)); } }

	const float LerpSpeed = 10;
	bool allowCameraMovement = true;

	void Start()
	{
		offset = transform.position - follow.position;
	}

	void OnEnable()
	{
		transform.rotation = Quaternion.Euler(26.6f, 0, 0);
	}

	void LateUpdate()
	{
		if (allowCameraMovement)
			transform.position = GetCameraTarget();

		if (Input.GetKeyDown(KeyCode.P))
			ToggleState();
	}

	void ToggleState()
	{
		if (dirState == DirState.Forward)
			dirState = DirState.Left;
		else
			dirState = DirState.Forward;
	}

	public void SetState(DirState dirState)
	{
		if (this.enabled)
			this.dirState = dirState;
	}

	Vector3 GetCameraTarget()
	{
		Vector3 target = Vector3.zero;

		switch (dirState)
		{
			case DirState.Forward:
				target = GetMoveForwardTarget();
				break;

			case DirState.Left:
				target = GetMoveLeftTarget();
				break;
		}

		return target;
	}

	Vector3 GetMoveForwardTarget()
	{
		Vector3 target = follow.position + offset; target.x = ForwardLockValue;
		return target;
	}

	Vector3 GetMoveLeftTarget()
	{
		Vector3 target = follow.position + offset; target.z = LeftLockValue;
		return target;
	}

	IEnumerator MoveThenSetNewState(DirState newState)
	{
		allowCameraMovement = false;
		GameObject.FindWithTag("Player").GetComponent<CatController>().enabled = false;

		Vector3 target = newState == DirState.Forward ? GetMoveForwardTarget() : GetMoveLeftTarget();

		while (Vector3.Distance(transform.position, target) > 0.05f)
		{
			transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * LerpSpeed);
			yield return null;
		}

		m_dirState = newState;
		allowCameraMovement = true;
		GameObject.FindWithTag("Player").GetComponent<CatController>().enabled = true;
	}
}
