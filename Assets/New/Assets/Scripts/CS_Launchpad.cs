using UnityEngine;
using System.Collections;

public class CS_Launchpad : MonoBehaviour {

	[SerializeField] float myLaunchSpeed = 3000;

	[Header("Color")]
	[SerializeField] Color myColorNormal;
	[SerializeField] Color myColorHit;
	[SerializeField] float myColorSpeed = 1;

	void Start () {
		this.GetComponent<Renderer> ().material.color = myColorNormal;
	}

	void Update () {
		this.GetComponent<Renderer> ().material.color = Color.Lerp (this.GetComponent<Renderer> ().material.color, myColorNormal, Time.deltaTime * myColorSpeed);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "Player") {
			collision.gameObject.GetComponent<CS_PlayerControl> ().StartLaunchpad (this.transform.up * myLaunchSpeed);
		}

		this.GetComponent<Renderer> ().material.color = myColorHit;

	}
}

