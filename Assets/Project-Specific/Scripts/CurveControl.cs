using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveControl : MonoBehaviour
{
    [Range(-1, 1)] public float curveX = 0f;
    [Range(-1, 1)] public float curveY = 0f;

    public Material[] materials;

    void Update()
    {
        foreach (var mat in materials)
        {
            mat.SetFloat(Shader.PropertyToID("_CurveX"), curveX);
            mat.SetFloat(Shader.PropertyToID("_CurveY"), curveY);
        }
    }
}