using UnityEngine;
using System;
using System.Collections;
using Kubernetes;

public class PodInfoForThisPod : MonoBehaviour {

	public Items item;
	public string cachedResourceVersion;

	public void ReceiveData (Items passedItem) {
		item = passedItem;
		cachedResourceVersion = item.metadata.resourceVersion;
		item.gameObject = this.gameObject;
	}

	void Update() {
		if (item.metadata.resourceVersion == cachedResourceVersion) {
		} else {
			Debug.LogError ("The resource has changed.  Apply updates here");
		}
	}
		
}
