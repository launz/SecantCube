using UnityEngine;
using System.Collections;

public class ResetRotation : MonoBehaviour {
	public Transform player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Rigidbody> ().MovePosition (player.position);
		transform.rotation = Quaternion.identity;
	}

	void OnCollisionEnter(Collision thisCollision){
		Debug.Log("collided with cam");
	}
}
