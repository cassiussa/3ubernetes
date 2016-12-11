using UnityEngine;
using System;
using System.Collections;
using Kubernetes;

public class ThisPod : MonoBehaviour {
	public KeepPodCurrent keepPodCurrent;
	public Items item;
	string baseURL;
	public string fullURL = "";
	public string apiText = "";

	public WWW www;
	public Hashtable headers;

	float startCoroutineTime = 0f;
	float coroutineWaitInterval = 1f;

	// Called by PodInstantiation.cs
	public void ReceiveData (Items passedItem) {
		item = passedItem;
		item.gameObject = this.gameObject;
		baseURL = item.baseURL;
		fullURL = baseURL + item.metadata.selfLink;
		www = new WWW(fullURL, null, headers);
	}


	void Update() {

		if (www != null) {
			if (www.isDone) {
				if (apiText != www.text) {
					apiText = www.text;
					//keepPodCurrent.BeginJSON (apiText);
				}
			}
		}

	}


	/*IEnumerator CheckForChange() {
		www = new WWW(fullURL, null, headers);
		yield return true; // May want to remove this later
	}*/


}
