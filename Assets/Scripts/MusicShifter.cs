using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicShifter : MonoBehaviour {
	public AudioClip otherClip;

	public IEnumerator NextClip() {
		AudioSource audio = GetComponent<AudioSource>();

		//audio.Play();
		yield return new WaitForSeconds(audio.clip.length - audio.time);
		audio.clip = otherClip;
		audio.Play();
	}
}
