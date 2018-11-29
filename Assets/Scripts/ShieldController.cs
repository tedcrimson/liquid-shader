using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShieldController : MonoBehaviour {

	public Vector4[] points;
	public Material shieldMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		shieldMaterial.SetInt("_PointsSize", points.Length);
		// if(points.Length >0)
			shieldMaterial.SetVectorArray("_Points", points);
	}

    // internal void AddRipple(Vector4 vector4, float lifetime)
    // {
    //     points.Add(vector4);
	// 	Invoke("RemoveLast", lifetime);
    // }

	// private void RemoveLast()
	// {
	// 	if(points.Count > 0)
	// 	{
	// 		points.RemoveAt(points.Count-1);
	// 	}
	// }
}
