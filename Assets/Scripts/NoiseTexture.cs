using UnityEngine;
using System.Collections;

public class NoiseTexture : MonoBehaviour {

	public Renderer rend;

	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer> ();
	}

	// Update is called once per frame
	void Update () {

		float scaleX = Random.Range(0.0f, 10.0f) * 50F + 10f;
		float scaleY = Random.Range(0.0f, 10.0f) * 50F + 10f;
	
		//rend.material.mainTextureScale = new Vector2(scaleX, scaleY);
		rend.material.SetTextureOffset ("_MainTex", new Vector2 (scaleX, scaleY));

	}
}
