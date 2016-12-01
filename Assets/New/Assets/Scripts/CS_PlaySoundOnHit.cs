using UnityEngine;
using System.Collections;

public class CS_PlaySoundOnHit : MonoBehaviour {
	[SerializeField] AudioClip mySFX;
	[SerializeField] float myVolume = 0;
	[SerializeField] float myPitch = 1;
	[SerializeField] GameObject myParticleEffect;

	void OnCollisionEnter (Collision g_collision) {
		if (g_collision.transform.tag == CS_Global.TAG_PLAYER)
			return;
		PlaySFX ();
		Instantiate(myParticleEffect, transform.position, Quaternion.identity);
//		Invoke ("DestroyParticle", myParticleEffect, 3f);
	}

	void OnTriggerEnter (Collider g_other) {
		if (g_other.tag == CS_Global.TAG_PLAYER)
			return;
		PlaySFX ();
		Instantiate(myParticleEffect, transform.position, Quaternion.identity);
//		Invoke ("DestroyParticle", myParticleEffect, 3f);
	}

	public void PlaySFX () {
		if (myVolume == 0) {
			CS_AudioManager.Instance.PlaySFX_Pitch (mySFX, myPitch);
		} else
			CS_AudioManager.Instance.PlaySFX_Pitch (mySFX, myVolume, myPitch);
	}

//	void DestroyParticle (GameObject particleEffectToDestroy) {
//		Destroy (particleEffectToDestroy);
//	}
}
