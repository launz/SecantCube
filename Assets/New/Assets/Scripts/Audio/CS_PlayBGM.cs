using UnityEngine;
using System.Collections;

public class CS_PlayBGM : MonoBehaviour {
	[SerializeField] AudioClip myBGM;
	[SerializeField] float myVolume;
	// Use this for initialization
	void Start () {
		if (myBGM == null)
			CS_AudioManager.Instance.StopBGM ();
		CS_AudioManager.Instance.PlayBGM (myBGM, myVolume);
	}
}
