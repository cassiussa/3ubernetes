using UnityEngine;
using System.Collections;
using Kubernetes;

public class PodInstantiation : MonoBehaviour {

	float pos = 0f;
	public void CreatePod (Items item) {
		pos += 10f;
		GameObject pod = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pod.name = item.name;
		pod.transform.position = new Vector3(pos, 0, 0);
		PodInfoForThisPod podInfoForThisPod = pod.gameObject.AddComponent<PodInfoForThisPod> ();
		podInfoForThisPod.ReceiveData (item);
		PodAPI podAPI = pod.gameObject.AddComponent<PodAPI> ();
		podAPI.buildPodVariable = pod.gameObject.AddComponent<BuildPodVariable> ();
	}

}
