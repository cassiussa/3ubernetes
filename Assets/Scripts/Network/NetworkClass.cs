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
	bool doOnce = false;
	public PodInstantiation podInstantiation;
	WWW www;

	void Awake() {
		podInstantiation = GetComponent<PodInstantiation> () as PodInstantiation;
		podInfo = GetComponent<PodInfo> ();
	}

	void Start() {
		www = new WWW(url);
		podInfo.pods = new List<Items>();
		podInfo.BuildJSON (receivedText);  // To PodInfo.cs
		StartCoroutine(CheckForChange());
	}

	void Update() {
		if (www.isDone) {
			receivedText = www.text;	

			// Run it the first time but with the GameObject instantiations
			if (doOnce == true) {
				if (receivedText != _cachedReceivedText) {
					podInfo.pods = new List<Items> ();
					podInfo.BuildJSON (receivedText);  // To PodInfo.cs
					foreach (Items items in podInfo.pods) {
						podResourceVersions.Add (items.metadata.resourceVersion);
					}
					_cachedReceivedText = receivedText;
				}
			} else {
				podInfo.pods = new List<Items> ();
				podInfo.BuildJSON (receivedText);  // To PodInfo.cs
				foreach (Items items in podInfo.pods) {
					podInstantiation.CreatePod (items);
				}
				doOnce = true;
			}
			www = new WWW(url);
		}
	}

	IEnumerator CheckForChange() {
		Debug.LogWarning ("CheckForChange()");

		yield return new WaitForSeconds(1);
		Debug.Log ("Checking for update now " + Time.time);
		yield return www; // May want to remove this later
		StartCoroutine(CheckForChange());
	}
}