using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic; // for List types

using Kubernetes;

public class PodInstantiation : MonoBehaviour {

	float pos = 0f;
	public void CreatePod (Items item, Dictionary<string,string> headers) {
		pos += 10f;
		GameObject pod = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Material mat = new Material (Shader.Find("Diffuse"));
		pod.renderer.material = mat;
		mat.color = new Color (1, 1, 1);
		pod.name = item.name;
		pod.transform.position = new Vector3(pos, 0, 0);

		ThisPod thisPod = pod.gameObject.AddComponent<ThisPod> ();
		thisPod.headers = headers;
		thisPod.ReceiveData (item);
		//thisPod.keepPodCurrent = pod.gameObject.AddComponent<KeepPodCurrent> ();
		//thisPod.keepPodCurrent.podMaterial = mat;
		UpdateThisPod updateThisPod = pod.gameObject.AddComponent<UpdateThisPod> ();
		updateThisPod.podMaterial = mat;
		updateThisPod.ReceiveData (item);  // Set it to be the same initially, so we have data
	}

}
