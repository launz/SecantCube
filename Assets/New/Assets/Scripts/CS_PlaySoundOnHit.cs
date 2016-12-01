using UnityEngine;
using System.Collections;

public class CS_PlaySoundOnHit : MonoBehaviour {
	[SerializeField] AudioClip mySFX;
	[SerializeField] float myVolume = 0;
	[SerializeField] float myPitch = 1;

	[Header("Particle")]
	[SerializeField] GameObject myParticleEffect;

	void Start () {
		myParticleEffect = Instantiate (myParticleEffect, this.transform) as GameObject; 
	}

	void OnCollisionEnter (Collision g_collision) {
		if (g_collision.transform.tag == CS_Global.TAG_PLAYER)
			return;
		PlaySFX ();

		myParticleEffect.GetComponent<ParticleSystem> ().Play ();
	}

	void OnTriggerEnter (Collider g_other) {
		if (g_other.tag == CS_Global.TAG_PLAYER)
			return;
		PlaySFX ();

		myParticleEffect.GetComponent<ParticleSystem> ().Play ();
	}

	public void PlaySFX () {
		if (myVolume == 0) {
			CS_AudioManager.Instance.PlaySFX_Pitch (mySFX, myPitch);
		} else
			CS_AudioManager.Instance.PlaySFX_Pitch (mySFX, myVolume, myPitch);
	}
		
}
