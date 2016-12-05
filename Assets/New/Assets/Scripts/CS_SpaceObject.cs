using UnityEngine;
using System.Collections;

public class CS_SpaceObject : MonoBehaviour {
	[SerializeField] float myFurthestDistance = 800;
	private Transform myPlayerTransform;
	// Use this for initialization
	void Start () {
		myPlayerTransform = GameObject.FindWithTag (CS_Global.TAG_PLAYER).transform;
	}
	
	// Update is called once per frame
	void Update () {
		float t_x = this.transform.position.x;
		float t_y = this.transform.position.y;
		float t_z = this.transform.position.z;
		if (Mathf.Abs (this.transform.position.x - myPlayerTransform.position.x) > myFurthestDistance) {
			t_x = this.transform.position.x +
				Mathf.Sign (this.transform.position.x - myPlayerTransform.position.x) * -2 * myFurthestDistance;
		}
		if (Mathf.Abs (this.transform.position.y - myPlayerTransform.position.y) > myFurthestDistance) {
			t_y = this.transform.position.y +
				Mathf.Sign (this.transform.position.y - myPlayerTransform.position.y) * -2 * myFurthestDistance;
		}
		if (Mathf.Abs (this.transform.position.z - myPlayerTransform.position.z) > myFurthestDistance) {
			t_z = this.transform.position.z +
				Mathf.Sign (this.transform.position.z - myPlayerTransform.position.z) * -2 * myFurthestDistance;
		}

		this.transform.position = new Vector3 (t_x, t_y, t_z);
	}
}
