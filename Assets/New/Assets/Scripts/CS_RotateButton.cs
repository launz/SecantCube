using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CS_RotateButton : MonoBehaviour {

	[SerializeField] GameObject myRotateCenter;
	[SerializeField] GameObject myRotateTarget;

	[Header("Color")]
	[SerializeField] Color myColorNormal;
	[SerializeField] Color myColorOtherNormal;
	[SerializeField] Color myColorHit;

	[SerializeField] float myColorSpeed = 1;

	[SerializeField] AudioClip mySFX;

	[SerializeField] bool _wasHit;

	Renderer thisRenderer;

	AudioSource rumbleAudio;
	float rotateSFXTimer;

	void Start () {
		rotateSFXTimer = 40f;
		rumbleAudio = GetComponent<AudioSource> ();
		_wasHit = false;
		thisRenderer = this.GetComponent<Renderer> ();
		thisRenderer.material.color = myColorNormal;
	}

	void Update () {

		if (!_wasHit) {
			if (thisRenderer.material.color == myColorNormal) {
				thisRenderer.material.DOColor (myColorOtherNormal, 2.0f);
			} else if (thisRenderer.material.color == myColorOtherNormal) {
				thisRenderer.material.DOColor (myColorNormal, 2.0f);
			}
		}
		//this.GetComponent<Renderer> ().material.color = Color.Lerp (this.GetComponent<Renderer> ().material.color, myColorNormal, Time.deltaTime * myColorSpeed);
		rotateSFXTimer += Time.deltaTime;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "Player") {
			
			thisRenderer.material.DOKill ();
			this.GetComponent<Renderer> ().material.color = myColorHit;
			myRotateTarget.GetComponent<CS_RotateLevel> ().SetRotation (Quaternion.Euler(this.transform.localRotation.eulerAngles * -1), myRotateCenter.transform);
			if (mySFX != null) 
				CS_AudioManager.Instance.PlaySFX (mySFX);
			if (rotateSFXTimer >= 40f) {
				rumbleAudio.Play ();
				rotateSFXTimer = 0f;
			}
			
			

		}



	}
}
