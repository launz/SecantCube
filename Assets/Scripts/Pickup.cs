using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	ParticleSystem thisParticle;

	public GameObject gameManager;

	bool _pickupEnabled;

	// Use this for initialization
	void Start () {

		gameManager = GameObject.FindGameObjectWithTag ("GameManager");

		thisParticle = gameObject.GetComponent<ParticleSystem> ();
		_pickupEnabled = true;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter() {

		gameObject.GetComponent<MeshRenderer>().enabled = false;

		gameObject.GetComponent<Collider> ().enabled = false;

		gameObject.GetComponent<AudioSource> ().Play ();

		thisParticle.startColor = Color.red;
		thisParticle.loop = false;
		thisParticle.gravityModifier = 0f;
		if (_pickupEnabled) {
			gameManager.SendMessage ("AddPickup");
			_pickupEnabled = false;
		}
		Destroy (gameObject, 1.0f);

	}

}
