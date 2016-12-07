using UnityEngine;
using System.Collections;
using Kubernetes;

public class PodInstantiation : MonoBehaviour {

	float pos = 0f;
	public void CreatePod (Items item) {
		pos += 10f;
		GameObject pod = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Material mat = new Material (Shader.Find("Diffuse"));
		pod.renderer.material = mat;
		mat.color = new Color (1, 1, 1);
		pod.name = item.name;
		pod.transform.position = new Vector3(pos, 0, 0);
		PodInfoForThisPod podInfoForThisPod = pod.gameObject.AddComponent<PodInfoForThisPod> ();
		podInfoForThisPod.ReceiveData (item);
		PodAPI podAPI = pod.gameObject.AddComponent<PodAPI> ();
		podAPI.keepPodCurrent = pod.gameObject.AddComponent<KeepPodCurrent> ();
		podAPI.keepPodCurrent.podMaterial = mat;
	}

}
