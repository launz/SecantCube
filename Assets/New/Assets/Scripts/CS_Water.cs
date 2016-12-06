using UnityEngine;
using System.Collections;

public class CS_Water : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame

	void OnTriggerEnter (Collider g_other) {
		if (g_other.tag == "MainCamera") {
			g_other.gameObject.GetComponent<CS_PlayerCamera> ().ShowWater ();
			Debug.Log ("in water");
		}
	}

	void OnTriggerExit (Collider g_other) {
		if (g_other.tag == "MainCamera")
			g_other.gameObject.GetComponent<CS_PlayerCamera> ().HideWater ();
	}
}
