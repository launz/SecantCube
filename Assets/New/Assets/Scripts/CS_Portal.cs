using UnityEngine;
using System.Collections;

public class CS_Portal : MonoBehaviour {
	[SerializeField] GameObject myExit;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.transform.position = myExit.transform.position + myExit.transform.forward;
			Camera.main.transform.position = Camera.main.transform.position - this.transform.position + myExit.transform.position + myExit.transform.forward;
		}
	}
}
