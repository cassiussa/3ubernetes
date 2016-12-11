// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List types
using Kubernetes;

public class NetworkClass : MonoBehaviour {

	public PodInfo podInfo;
	public PodInfo cachedPodInfo;
	public List<string> podResourceVersions = new List<string>();

	public string url = "http://supercass.com/bah.php";
	string fullURL = "";
	string receivedText;
	string _cachedReceivedText;
	string cleanedText;
	string _cachedCleanedText;
	bool doneOnce = false;
	public PodInstantiation podInstantiation;
	WWW thisWwwCall;
	WWWForm form;
	Hashtable headers;
	//headers.Add("Header_key", "Header_val");
	//www = new WWW("http://localhost/getpostheaders", null, headers);

	void Awake() {
		podInstantiation = GetComponent<PodInstantiation> () as PodInstantiation;
		podInfo = GetComponent<PodInfo> ();
	}

	void Start() {
		//form = new WWWForm();
		headers = new Hashtable();
		headers.Add("Authorization", "Bearer <toke>");

		//fullURL = url+"/api/v1/namespaces/ocelot-app/pods";
		fullURL = url+"/api/v1/namespaces/ocelot-app/pods";
		thisWwwCall = new WWW(fullURL, null, headers);
		podInfo.pods = new List<Items>();
		podInfo.BuildJSON (receivedText);  // To PodInfo.cs
		StartCoroutine(CheckForChange());
	}
	float startCoroutineTime = 0f;
	float coroutineWaitInterval = 1f;

	void Update() {
		

		if (thisWwwCall.isDone) {
			// Run the API check every 'coroutineWaitInterval' seconds
			if (Time.time >= startCoroutineTime) {
				startCoroutineTime += coroutineWaitInterval;
				StartCoroutine (CheckForChange ());
			}

			Debug.Log ("is done");
			_cachedReceivedText = receivedText;
			_cachedCleanedText = receivedText;
			receivedText = thisWwwCall.text;
			cleanedText = receivedText;

			if (cleanedText != null && _cachedCleanedText != null) {
				for (int i = 0; i < 8; i++) {
					int index = cleanedText.IndexOf (System.Environment.NewLine);
					cleanedText = cleanedText.Substring (index + System.Environment.NewLine.Length);
					int index2 = _cachedCleanedText.IndexOf (System.Environment.NewLine);
					_cachedCleanedText = _cachedCleanedText.Substring (index + System.Environment.NewLine.Length);
				}
				for (int i = 0; i < 2; i++) {
					cleanedText = cleanedText.Remove(cleanedText.LastIndexOf(System.Environment.NewLine));
					_cachedCleanedText = _cachedCleanedText.Remove(_cachedCleanedText.LastIndexOf(System.Environment.NewLine));
				}

				if (cleanedText != _cachedCleanedText) {
					Debug.Log ("cleanedText");
					Debug.Log (cleanedText);
					Debug.Log ("_cachedCleanedText");
					Debug.Log (_cachedCleanedText);
				}
			}
				

			/*if (cachedPodInfo != podInfo) {
				Debug.LogError ("not equal");
			} else {
				Debug.LogWarning ("They're equal");
			}*/

			// Run it the first time but with the GameObject instantiations
			if (doneOnce == true) {
				if (receivedText != _cachedReceivedText) {
					//podInfo.pods.Clear();
					podInfo.pods = new List<Items> ();
					podInfo.BuildJSON (receivedText);  // To PodInfo.cs
					cachedPodInfo = podInfo;
					foreach (Items items in podInfo.pods) {
						podResourceVersions.Add (items.metadata.resourceVersion);
					}
				}
			} else {
				//podInfo.pods.Clear();
				podInfo.pods = new List<Items> ();
				podInfo.BuildJSON (receivedText);  // To PodInfo.cs
				cachedPodInfo = podInfo;
				foreach (Items items in podInfo.pods) {
					podInstantiation.CreatePod (items);
				}
				doneOnce = true;
			}


			_cachedReceivedText = receivedText;
			thisWwwCall = new WWW(fullURL, null, headers);
		}
	}

	IEnumerator CheckForChange() {
		yield return new WaitForSeconds(2f);
		//Debug.Log ("Checking for update at " + Time.time);
		yield return thisWwwCall; // May want to remove this later
		//StartCoroutine(CheckForChange());
	}
}