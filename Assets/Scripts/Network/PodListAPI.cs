// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List types
using Kubernetes;

public class PodListAPI : MonoBehaviour {

	public PodArray podArray;
	PodArray _cachedPodArray;
	public string apiKey;
	//public List<string> podResourceVersions = new List<string>();

	public string url = "http://supercass.com/bah.php";
	string fullURL = "";
	string receivedText;
	string cleanedText;
	string _cachedCleanedText;
	bool doneOnce = false;
	public PodInstantiation podInstantiation;
	WWW thisWwwCall;
	Hashtable headers;
	//headers.Add("Header_key", "Header_val");
	//www = new WWW("http://localhost/getpostheaders", null, headers);

	void Awake() {
		podInstantiation = GetComponent<PodInstantiation> () as PodInstantiation;
		podArray = GetComponent<PodArray> ();
	}

	void Start() {
		headers = new Hashtable();
		headers.Add ("Authorization", "Bearer " + apiKey);

		fullURL = url+"/api/v1/pods";
		//fullURL = url+"/api/v1/namespaces/ocelot-app/pods";
		thisWwwCall = new WWW(fullURL, null, headers);
		podArray.pods = new List<Items>();
		//podInfo.BuildJSON (receivedText);  // To PodInfo.cs
		StartCoroutine(CheckForChange());

		Conditions cond = new Conditions ("type", "status", "lprobe", "ltt");

		Labels lab1 = new Labels ("key", "value");
		List<Labels> llab1 = new List <Labels>();
		llab1.Add (lab1);
		Metadata mdata = new Metadata ("name", "namespace", "selfLink", "resource", "time", llab1);
		List<Volumes> lvol1 = new List<Volumes> ();
		lvol1.Add (new Volumes ("name","secret","hostpath"));
		List<VolumeMounts> lvolm1 = new List<VolumeMounts> ();
		lvolm1.Add (new VolumeMounts ());
		Containers cont1 = new Containers ("name", "image", "resource", lvolm1);
		Spec spc1 = new Spec (lvol1, cont1, "nodename");
		ContainerStatuses contStatus1 = new ContainerStatuses ("name", "state", "lastState", "ready", "count", "image", "imageid", "contid");
		Conditions cond1 = new Conditions ("type", "status", "lprove", "ltt");
		Status sta1 = new Status ("phase", cond1, "hostip", "podip", "time", contStatus1);

		Labels lab2 = new Labels ("key", "value");
		List<Labels> llab2 = new List <Labels>();
		llab2.Add (lab2);
		Metadata mdata2 = new Metadata ("name", "namespace", "selfLink", "resource", "time", llab2);
		List<Volumes> lvol2 = new List<Volumes> ();
		lvol2.Add (new Volumes ("name","secret","hostpath"));
		List<VolumeMounts> lvolm2 = new List<VolumeMounts> ();
		lvolm2.Add (new VolumeMounts ());
		Containers cont2 = new Containers ("name", "image", "resource", lvolm2);
		Spec spc2 = new Spec (lvol2, cont1, "nodename");
		ContainerStatuses contStatus2 = new ContainerStatuses ("name", "state", "lastState", "ready", "count", "image", "imageid", "contid");
		Conditions cond2 = new Conditions ("type", "status", "lprove", "ltt");
		Status sta2 = new Status ("phase", cond2, "hostip", "podip", "time", contStatus2);
		// spc1 and spc2
		Items label1 = new Items ("name", gameObject, "baseurl", mdata, spc1, sta1);
		Items label2 = new Items ("name", gameObject, "baseurl", mdata, spc2, sta2);



		if (label1 == label2) {
			Debug.Log ("equal");
		} else {
			Debug.Log ("not equal");
		}
	}
	// Check to see if the Lists have same values.  Needed so that I can
	// Get the == operator overriding properly for Items type conditionals
	bool CheckMatch(List<Labels> l1, List<Labels> l2) {
		if (l1.Count != l2.Count)
			return false;
		for (int i = 0; i < l1.Count; i++) {
			if (l1[i] != l2[i])
				return false;
		}
		return true;
	}



	float startCoroutineTime = 0f;
	float coroutineWaitInterval = 1f;

	void Update() {


		if (thisWwwCall.isDone) {  // The API check is complete

			// Create new text entries
			receivedText = thisWwwCall.text;
			cleanedText = PurgeOuterObject (thisWwwCall.text);


			if (doneOnce == false) {  // Hasn't been run before
				podArray.pods = new List<Items> ();
				podArray.BuildJSON (receivedText, podArray.pods);  // To PodInfo.cs
				foreach (Items items in podArray.pods) {
					podInstantiation.CreatePod (items, headers);
				}

				doneOnce = true;

			} else {  // Has been run before
				if (cleanedText != _cachedCleanedText) {
					//PodArray _cachedPodArray = podArray;
					//podArray.pods = podArray.changedPods;
					podArray.changedPods = new List<Items> ();
					podArray.BuildJSON (receivedText, podArray.changedPods);  // To PodInfo.cs
					// We should compare the new to the old, individually
					foreach (Items outterItem in podArray.changedPods) {
						foreach (Items innerItem in podArray.pods) {
							if (innerItem.name == outterItem.name) {
								// Now check to see if the Items are equal
								if (innerItem != outterItem) {
									Debug.Log (innerItem.name + " is not equal");
								}
								/*
								 * If not, do the below
								 * GameObject.Find (outterItem.name).GetComponent<UpdateThisPod> ().item = outterItem;
								 */

							}
						}
					}
				}
			}

			_cachedPodArray = podArray;
			_cachedCleanedText = cleanedText;

			// Run the API check every 'coroutineWaitInterval' seconds
			if (Time.time >= startCoroutineTime) {
				startCoroutineTime += coroutineWaitInterval;
				StartCoroutine (CheckForChange ());
			}
		}
	}

	IEnumerator CheckForChange() {
		yield return new WaitForSeconds(2f);
		thisWwwCall = new WWW(fullURL, null, headers);
		yield return true; // May want to remove this later
	}

	private string PurgeOuterObject (string originalText) {
		/* 
		 * The code below is a hack to remove the outer-object
		 * emcompassing the data we want
		 */
		string cleanText;
		cleanText = originalText.Substring (originalText.IndexOf ('}') + 1);
		cleanText = cleanText.Substring (cleanText.IndexOf (',') + 1);
		cleanText = cleanText.Substring (cleanText.IndexOf ('"') + 1);
		cleanText = cleanText.Remove (cleanText.Length - 2);
		return cleanText;
	}
}