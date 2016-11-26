using UnityEngine;
using System.Collections;

public class DummyControl : MonoBehaviour {

	/// <summary>
	/// Currently, this script only updates position of the dummy to that of the player object
	/// </summary>

	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = player.transform.position;
	
	}
}
