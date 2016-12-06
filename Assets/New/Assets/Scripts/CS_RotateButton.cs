using UnityEngine;
using System.Collections;

public class CS_RotateButton : MonoBehaviour {

	[SerializeField] GameObject myRotateCenter;
	[SerializeField] GameObject myRotateTarget;

	[Header("Color")]
	[SerializeField] Color myColorNormal;
	[SerializeField] Color myColorHit;
	[SerializeField] float myColorSpeed = 1;

	[SerializeField] AudioClip mySFX;

	void Start () {
		this.GetComponent<Renderer> ().material.color = myColorNormal;
	}

	void Update () {
		this.GetComponent<Renderer> ().material.color = Color.Lerp (this.GetComponent<Renderer> ().material.color, myColorNormal, Time.deltaTime * myColorSpeed);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "Player") {
			myRotateTarget.GetComponent<CS_RotateLevel> ().SetRotation (Quaternion.Euler(this.transform.localRotation.eulerAngles * -1), myRotateCenter.transform);
			if (mySFX != null)
				CS_AudioManager.Instance.PlaySFX (mySFX);
		}

		this.GetComponent<Renderer> ().material.color = myColorHit;

	}
}
