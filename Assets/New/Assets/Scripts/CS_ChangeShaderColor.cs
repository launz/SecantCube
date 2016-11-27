using UnityEngine;
using System.Collections;

public class CS_ChangeShaderColor : MonoBehaviour {

	ReplacementShaderEffect replacementShader;

	public Color newStartColor;
	public Color newMidColor;
	public Color newEndColor;


	void Start () {
	
//		replacementShader = GetComponents<ReplacementShaderEffect> ();


	}
	

	void Update () {

		if (Input.GetMouseButtonDown (1)) {

//			replacementShader.SendMessage ("ChangeStartColor", newStartColor);
		}
	
	}
}
