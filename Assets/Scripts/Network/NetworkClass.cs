// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List types
using Kubernetes;

public class NetworkClass : MonoBehaviour {

	public PodInfo podInfo;
	public List<string> podResourceVersions = new List<string>();

	public string url = "http://supercass.com/bah.php";
	string receivedText;
	string _cachedReceivedText;

	WWW www;
	WWW cachedWWW;
	void Start() {
		podInfo = GetComponent<PodInfo> ();
		www = new WWW(url);
		//yield return www;
		//text = www.text;
		//Renderer renderer = GetComponent<Renderer>();
		//renderer.material.mainTexture = www.texture;
		StartCoroutine(CheckForChange());
	}

	void Update() {
		if (www.isDone) {
			receivedText = www.text;	
			if (receivedText != _cachedReceivedText) {
				//podInfo.BuildJSON(receivedText);  // To PodInfo.cs
				_cachedReceivedText = receivedText;
				//Debug.LogError (receivedText);
				Debug.LogError ("URL has been updated");
			}
			www = new WWW(url);
		}
	}

	IEnumerator CheckForChange() {
		//podResourceVersions = new List<string>();

		podInfo.pods = new List<Items>();
		podInfo.BuildJSON(receivedText);  // To PodInfo.cs
		foreach(Items items in podInfo.pods) {
			podResourceVersions.Add (items.metadata.resourceVersion);
		}

		yield return new WaitForSeconds(1);
		Debug.Log ("Checking for update now " + Time.time);

		cachedWWW = new WWW(url);
		yield return cachedWWW; // May want to remove this later

		for (int i = 0; i < podInfo.pods.Count; i++) {
			if (podResourceVersions [i] != podInfo.pods [i].metadata.resourceVersion) {
				Debug.LogError ("<resourceVersion> has changed");
			}
		}


		StartCoroutine(CheckForChange());
	}
}