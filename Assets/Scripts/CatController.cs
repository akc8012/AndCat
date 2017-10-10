using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatController : MonoBehaviour
{
	[SerializeField] Transform rotateMesh;
	[SerializeField] Text text;
	[SerializeField] Image dotImage;

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

		Vector3 influence = externInfluence;
		HandleRunEffects(Vector3.Dot(movement, externInfluence));

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

	void HandleRunEffects(float dot)
	{
		float t = dot + Mathf.Clamp((1 - dot), 0, 1);
		t = Mathf.Abs(t - 1);

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

		for (float t = 0; t < 1; t += Time.deltaTime * speed)
		{
			externInfluence = direction * curve.Evaluate(t);
			yield return null;
		}

		externInfluence = Vector3.zero;
	}
}
