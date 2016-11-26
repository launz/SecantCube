//http://www.cnblogs.com/gameprogram/archive/2012/08/15/2640357.html
//http://www.blog.silentkraken.com/2010/04/06/audiomanager/
using UnityEngine;
using System.Collections;

public class CS_AudioManager : MonoBehaviour {
	
	private static CS_AudioManager instance = null;

	[SerializeField] GameObject myPrefabSFX;

	[SerializeField] AudioSource myAudioSource;

	//========================================================================
	public static CS_AudioManager Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	public void PlaySFX (AudioClip g_SFX) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	public void PlaySFX (AudioClip g_SFX, float g_Volume) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().volume = g_Volume;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	public void PlaySFX_Pitch (AudioClip g_SFX, float g_Pitch) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().pitch = g_Pitch;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	public void PlaySFX_Pitch (AudioClip g_SFX, float g_Volume, float g_Pitch) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().volume = g_Volume;
		t_SFX.GetComponent<AudioSource> ().pitch = g_Pitch;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	public void PlayBGM (AudioClip g_BGM) {
		if (myAudioSource.isPlaying == false) {
			myAudioSource.clip = g_BGM;
			myAudioSource.Play ();
			return;
		}

		if (g_BGM == myAudioSource.clip)
			return;

		myAudioSource.Stop ();
		myAudioSource.clip = g_BGM;
		myAudioSource.Play ();
	}

	public void PlayBGM (AudioClip g_BGM, float g_Volume) {
		if (myAudioSource.isPlaying == false) {
			myAudioSource.clip = g_BGM;
			myAudioSource.volume = g_Volume;
			myAudioSource.Play ();
			return;
		} else if (g_BGM == myAudioSource.clip) {
			myAudioSource.volume = g_Volume;
			return;
		}

		myAudioSource.Stop ();
		myAudioSource.clip = g_BGM;
		myAudioSource.volume = g_Volume;
		myAudioSource.Play ();
	}

	public void StopBGM () {
		myAudioSource.Stop ();
	}

}
