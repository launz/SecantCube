using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ReplacementShaderEffect : MonoBehaviour
{
    public Shader ReplacementShader;
    public Color startColor;
    public Color midColor;
    public Color endColor;
	Camera mainCam;



	void Start() {	

		mainCam = GetComponent<Camera> ();
		Shader.SetGlobalColor("_StartColor", startColor);
		Shader.SetGlobalColor("_MidColor", midColor);
		Shader.SetGlobalColor("_EndColor", endColor);

	}

	void Update() {		//	make background color always the same as end color in shader
		
		mainCam.backgroundColor = endColor;
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

		startColor = newStartColor;

	}

	void ChangeMidColor (Color newMidColor) {

		midColor = newMidColor;

	}

	void ChangeEndColor (Color newEndColor) {

		endColor = newEndColor;

	}
}