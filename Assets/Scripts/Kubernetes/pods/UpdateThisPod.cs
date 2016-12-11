using UnityEngine;
using System;
using System.Collections;
using Kubernetes;

public class UpdateThisPod : MonoBehaviour {
	/*
	 * This script should receive a copy of the Item type whenever
	 * there's been a change to the master yaml file.  It will
	 * then compare itself to the one found in ThisPod.cs.  If
	 * it is different, it will send its values into ThisPod.cs'
	 * copy of the Item
	 */

	public Items item;
	ThisPod thisPod;
	KeepPodCurrent keepPodCurrent;

	void Awake() {
		thisPod = GetComponent<ThisPod> ();
		keepPodCurrent = GetComponent<KeepPodCurrent> ();
	}
	// Called by PodInstantiation.cs
	public void ReceiveData (Items passedItem) {
		item = passedItem;
		item.gameObject = this.gameObject;
	}
	
	// Update is called once per frame
	public void Copy () {
		Debug.Log ("would copy the Item from here to ThisPod.cs");
		thisPod.item = item;
		// We should also update the colours etc now
		ChangePodLook();
		Debug.Log ("ChangePodLook");
	}





	public Material podMaterial;
	Color startColor;
	Color endColor;
	float duration = 1f; // duration in seconds
	float t = 0f; // lerp control variable

	public void ChangePodLook() {
		//Debug.LogError ("item.status.phase = " + item.status.phase);
		if (thisPod.item.status.phase == "Starting") {
			startColor = podMaterial.color;
			endColor = new Color (190f / 255f, 237f / 255f, 249f / 255f);
			t = 0f;
		} else if (thisPod.item.status.phase == "Running") {
			Debug.Log ("*****************It's running");
			startColor = podMaterial.color;
			endColor = new Color (0f, 185f / 255f, 228f / 255f);
			t = 0f;
		} else if (thisPod.item.status.phase == "Canceled") {
			startColor = podMaterial.color;
			endColor = new Color (1f, 0f, 0f);
			t = 0f;
		} else {
			Debug.LogError ("Some other phase", gameObject);
		}
		//yield return null;
	}

	void Update() {
		//if (t < 1) {  // Only run it when there's a fade that needs happening
			//Debug.Log ("running");
			StartCoroutine(Fade ());
		//}
	}

	IEnumerator Fade() {
		podMaterial.color = Color.Lerp(startColor, endColor, t);
		if (t < 1){ // While t below the end limit
			t += Time.deltaTime/duration;  // Increment it at the desired rate every update
		}
		yield return null;
	}

}
