using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ReplacementShaderEffect : MonoBehaviour
{
	[SerializeField] Shader ReplacementShader;
	[SerializeField] Color startColor;
	[SerializeField] Color midColor;
	[SerializeField] Color endColor;

	Camera mainCam;


	private Color startColorTarget;
	private Color midColorTarget;
	private Color endColorTarget;
	[SerializeField] float myColorChangeSpeed = 1;


	void Start() {	
		startColorTarget = startColor;
		midColorTarget = midColor;
		endColorTarget = endColor;

		mainCam = GetComponent<Camera> ();
		Shader.SetGlobalColor("_StartColor", startColor);
		Shader.SetGlobalColor("_MidColor", midColor);
		Shader.SetGlobalColor("_EndColor", endColor);

	}

	void Update() {		//	make background color always the same as end color in shader
		
		mainCam.backgroundColor = endColor;

		startColor = Color.Lerp (startColor, startColorTarget, Time.deltaTime * myColorChangeSpeed);
		midColor = Color.Lerp (midColor, midColorTarget, Time.deltaTime * myColorChangeSpeed);
		endColor = Color.Lerp (endColor, endColorTarget, Time.deltaTime * myColorChangeSpeed);

		Shader.SetGlobalColor("_StartColor", startColor);
		Shader.SetGlobalColor("_MidColor", midColor);
		Shader.SetGlobalColor("_EndColor", endColor);
	}

    void OnValidate()
    {
    	Shader.SetGlobalColor("_StartColor", startColor);
    	Shader.SetGlobalColor("_MidColor", midColor);
    	Shader.SetGlobalColor("_EndColor", endColor);

    }

    void OnEnable()
    {
        if (ReplacementShader != null)
            GetComponent<Camera>().SetReplacementShader(ReplacementShader, "RenderType");
    }

    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }

	void ChangeStartColor (Color newStartColor) {
		startColorTarget = newStartColor;
	}

	void ChangeMidColor (Color newMidColor) {
		midColorTarget = newMidColor;
	}

	void ChangeEndColor (Color newEndColor) {
		endColorTarget = newEndColor;
	}

	public void ChangeColors (Color g_start, Color g_mid, Color g_end) {
		ChangeStartColor (g_start);
		ChangeMidColor (g_mid);
		ChangeEndColor (g_end);
		Debug.Log ("ChangeColors");
	}

}