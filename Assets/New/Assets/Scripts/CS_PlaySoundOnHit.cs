using UnityEngine;
using System.Collections;

public class CS_PlaySoundOnHit : MonoBehaviour {
	[SerializeField] AudioClip mySFX;
	[SerializeField] float myVolume = 0;
	[SerializeField] float myPitch = 1;

	[Header("Particle")]
	[SerializeField] GameObject myParticleEffect;

	[Header("Color")]
	[SerializeField] Color myColorNormal;
	[SerializeField] Color myColorHit;
	[SerializeField] float myColorSpeed;
	private Color myColor;

	void Start () {
		myParticleEffect = Instantiate (myParticleEffect, this.transform) as GameObject; 
		myParticleEffect.transform.position = this.transform.position;
		this.GetComponent<Renderer> ().material.color = myColorNormal;
	}

	void Update () {
		this.GetComponent<Renderer> ().material.color = Color.Lerp (this.GetComponent<Renderer> ().material.color, myColorNormal, Time.deltaTime * myColorSpeed);
	}

	void OnCollisionEnter (Collision g_collision) {
		if (g_collision.transform.tag == CS_Global.TAG_PLAYER)
			return;
		PlaySFX ();

		myParticleEffect.GetComponent<ParticleSystem> ().Play ();
		this.GetComponent<Renderer> ().material.color = myColorHit;
	}

	void OnTriggerEnter (Collider g_other) {
		if (g_other.tag == CS_Global.TAG_PLAYER)
			return;
		PlaySFX ();

		myParticleEffect.GetComponent<ParticleSystem> ().Play ();
		this.GetComponent<Renderer> ().material.color = myColorHit;
	}

	public void PlaySFX () {
		if (myVolume == 0) {
			CS_AudioManager.Instance.PlaySFX_Pitch (mySFX, myPitch);
		} else
			CS_AudioManager.Instance.PlaySFX_Pitch (mySFX, myVolume, myPitch);
	}
		
}
