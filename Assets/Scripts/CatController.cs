using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class CatController : MonoBehaviour
{
	[SerializeField] Transform rotateMesh;
	[SerializeField] Text text;
	[SerializeField] Image dotImage;
	[SerializeField] float influenceIgnoreAmount = 1;

	CharacterController characterController;
	Transform cam;

	const float MoveSpeed = 2.5f;
	const float RotSmooth = 20;          // smoothing on the lerp to rotate towards stick direction

	Vector3 externInfluence = Vector3.zero;

	void Awake()
	{
		characterController = GetComponent<CharacterController>();
		cam = Camera.main.transform;
	}

	void Update()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 movement = AndUtils.InputWorldToCam(cam, input, Quaternion.identity);

		//Vector3 influenceInput = new Vector3(Input.GetAxisRaw("RightHorizontal"), 0, Input.GetAxisRaw("RightVertical"));
		//Vector3 influence = AndUtils.InputWorldToCam(cam, influenceInput, Quaternion.identity) + externInfluence;
		//influence *= 3;

		//HandleRunEffects(Vector3.Dot(movement, (influence.normalized * influenceInput.magnitude)));

		float normalizedDot = Vector3.Dot(movement.normalized, externInfluence.normalized);

		if (externInfluence != Vector3.zero && normalizedDot < 0)
			movement *= influenceIgnoreAmount;

		if (Input.GetKey(KeyCode.LeftShift))
			movement *= 5;

		Vector3 influence = externInfluence;
		normalizedDot = Vector3.Dot(movement.normalized, externInfluence.normalized);       // do again because change
		HandleRunEffects(Vector3.Dot(movement, externInfluence), normalizedDot);

		Vector3 avgMovement = ((movement + influence) / 2).normalized;
		RotateMesh(avgMovement);

		Debug.DrawRay(transform.position, movement, Color.blue);
		Debug.DrawRay(transform.position, influence, Color.green);

		Vector3 velocity = movement + influence;
		characterController.Move(velocity * MoveSpeed * Time.deltaTime);
	}

	void RotateMesh(Vector3 moveDir)
	{
		Vector3 targetRotation = Vector3.Slerp(rotateMesh.forward, moveDir, Time.deltaTime * RotSmooth);

		if (targetRotation != Vector3.zero)
			rotateMesh.rotation = Quaternion.LookRotation(targetRotation);
	}

	void HandleRunEffects(float dot, float normalizedDot)
	{
		if (normalizedDot > 0)
			return;

		// this is the maximum dot product when influence and movement are opposite. why 16? dunno yet
		const float MagicNumber = 16;

		float t = dot + Mathf.Clamp((1 - dot), 0, 1);
		t = Mathf.Abs(t - 1);
		t /= MagicNumber;

		GamePad.SetVibration(PlayerIndex.One, t, t);

		if (dotImage != null)
		{
			Color col = Color.Lerp(Color.gray, Color.red, t);
			dotImage.color = col;
		}

		if (text != null)
			text.text = t.ToString();
	}

	public void SetInfluence(Vector3 influence)
	{
		GetComponent<AudioSource>().Play();
		externInfluence = influence;
	}

	public IEnumerator SetInfluenceOverT(AnimationCurve curve, Vector3 direction, float speed)
	{
		GetComponent<AudioSource>().Play();
		//GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);

		for (float t = 0; t < 1; t += Time.deltaTime * speed)
		{
			externInfluence = direction * curve.Evaluate(t);
			yield return null;
		}

		GamePad.SetVibration(PlayerIndex.One, 0, 0);
		externInfluence = Vector3.zero;
	}

	void OnTriggerEnter(Collider col)
	{
		string nameLower = col.name.ToLower();

		if (nameLower.Contains("camerastateswitch"))
		{
			if (nameLower.Contains("point"))
			{
				Camera.main.GetComponent<CameraStateController>().SetPointCam();
				return;
			}

			Camera.main.GetComponent<CameraStateController>().SetHallwayCam();

			if (nameLower.Contains("forward"))
				Camera.main.GetComponent<CameraController>().SetState(CameraController.DirState.Forward);

			if (nameLower.Contains("left"))
				Camera.main.GetComponent<CameraController>().SetState(CameraController.DirState.Left);
		}
	}
}
