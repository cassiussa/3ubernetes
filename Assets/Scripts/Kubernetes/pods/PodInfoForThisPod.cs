using UnityEngine;
using System;
using System.Collections;
using Kubernetes;

public class PodInfoForThisPod : MonoBehaviour {

	public Items item;

	public void ReceiveData (Items passedItem) {
		item = passedItem;
		item.gameObject = this.gameObject;
	}
		
}
