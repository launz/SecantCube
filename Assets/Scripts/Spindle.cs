using UnityEngine;
using System.Collections;

public class Spindle : MonoBehaviour {

	public AudioSource audio;

	public GameObject spindleMesh;

	//variables used for lerping color upon collision
	Color origColor;
	Color destColor;
	Color currentColor;
	Color emissiveColor;


	public float colorChangeTime;

	float colorChangeAmt = 0.0f;
	float emissiveChangeAmt = 0.0f;


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();	

		origColor = spindleMesh.GetComponent<Renderer> ().material.color;
	
	}
	
	// Update is called once per frame
	void Update () {
		colorChangeAmt += Time.deltaTime;
		emissiveChangeAmt += Time.deltaTime;

		currentColor = Color.Lerp(Color.white, origColor, Mathf.Clamp(colorChangeAmt,0f, 1f));
		emissiveColor = Color.Lerp(Color.white, Color.black, Mathf.Clamp(colorChangeAmt,0f, 1f));

		spindleMesh.GetComponent<Renderer> ().material.color = currentColor;

		spindleMesh.GetComponent<Renderer> ().material.SetColor("_EmissionColor",  emissiveColor);


	
	}

	void OnTriggerEnter(Collider collision) {
		
		audio.Play ();

		spindleMesh.GetComponent<Renderer>().material.color = Color.white;

		colorChangeAmt = 0;



	}
}
