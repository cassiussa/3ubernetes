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

	// Called by PodInstantiation.cs
	public void ReceiveData (Items passedItem) {
		item = passedItem;
		item.gameObject = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
