using UnityEngine;
using System;
using System.Collections;
using Kubernetes;

public class PodAPI : MonoBehaviour {
	public PodInfoForThisPod podInfoForThisPod;
	public KeepPodCurrent keepPodCurrent;
	public Items item;
	public string baseURL;
	string fullURL = "";
	public string apiText = "";
	public string cachedApiText = "";

	public WWW www;
	public WWWForm form;
	public Hashtable headers;

	float startCoroutineTime = 0f;
	float coroutineWaitInterval = 1f;

	void Awake () {
		headers = new Hashtable();
		headers.Add("Authorization", "Bearer <token>");
		podInfoForThisPod = GetComponent<PodInfoForThisPod> ();
		keepPodCurrent = GetComponent<KeepPodCurrent> ();
		item = podInfoForThisPod.item;
		baseURL = item.baseURL;
		fullURL = baseURL + item.metadata.selfLink;
		Debug.Log (fullURL);
		www = new WWW(fullURL, null, headers);
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
				keepPodCurrent.BeginJSON (apiText);
			} else {

			}
		}

	}


	IEnumerator CheckForChange() {
		www = new WWW(fullURL, null, headers);
		//Debug.Log ("Checking for update at " + Time.time);
		yield return www; // May want to remove this later
	}


}
