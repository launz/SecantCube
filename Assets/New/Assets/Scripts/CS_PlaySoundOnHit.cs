using UnityEngine;
using System.Collections;

public class CS_PlaySoundOnHit : MonoBehaviour {
	[SerializeField] AudioClip mySFX;
	[SerializeField] float myVolume = 0;
	[SerializeField] float myPitch = 1;

	void OnCollisionEnter (Collision g_collision) {
		if (g_collision.transform.tag == CS_Global.TAG_BOOST)
			PlaySFX ();
	}

	void OnTriggerEnter (Collider g_other) {
		if (g_other.tag == CS_Global.TAG_BOOST)
			PlaySFX ();
	}

	public void PlaySFX () {
		if (myVolume == 0) {
			CS_AudioManager.Instance.PlaySFX_Pitch (mySFX, myPitch);
		} else
			CS_AudioManager.Instance.PlaySFX_Pitch (mySFX, myVolume, myPitch);
	}
}
