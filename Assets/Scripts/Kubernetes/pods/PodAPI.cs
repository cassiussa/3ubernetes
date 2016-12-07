using UnityEngine;
using System;
using System.Collections;
using Kubernetes;

public class PodAPI : MonoBehaviour {
	public PodInfoForThisPod podInfoForThisPod;
	public BuildPodVariable buildPodVariable;
	public Items item;
	public string baseURL;
	string fullURL = "";
	public string apiText = "";
	public string cachedApiText = "";

	public WWW www;

	float startCoroutineTime = 0f;
	float coroutineWaitInterval = 1f;

	void Awake () {
		podInfoForThisPod = GetComponent<PodInfoForThisPod> ();
		buildPodVariable = GetComponent<BuildPodVariable> ();
		item = podInfoForThisPod.item;
		baseURL = item.baseURL;
		fullURL = baseURL + item.metadata.selfLink;
		Debug.Log (fullURL);
		www = new WWW(fullURL);
	}

	void Update() {
		// Run the API check every 'coroutineWaitInterval' seconds
		if (Time.time >= startCoroutineTime) {
			startCoroutineTime += coroutineWaitInterval;
			StartCoroutine (CheckForChange ());
		}

		if (www.isDone) {
			cachedApiText = apiText;
			apiText = www.text;
			if (apiText != cachedApiText) {
				buildPodVariable.BeginJSON (apiText);
				//Debug.LogError ("Pod has changed.", gameObject);
			} else {

			}
		}

	}


	IEnumerator CheckForChange() {
		www = new WWW(fullURL);
		Debug.Log ("Checking for update at " + Time.time);
		yield return www; // May want to remove this later
	}
}
