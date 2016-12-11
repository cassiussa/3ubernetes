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

		/*
		 * This section was used to verify that the 
		 * conditionals on Items was traversing all the way
		 * down the object successfully.
		Labels lab1 = new Labels ("key", "value");
		Labels lab2 = new Labels ("key", "value");
		List<Labels> llab1 = new List <Labels>();
		List<Labels> llab2 = new List <Labels>();
		llab1.Add (lab1);
		llab2.Add (lab2);
		Metadata mdata1 = new Metadata ("name", "namespace", "selfLink", "resource", "time", llab1);
		Metadata mdata2 = new Metadata ("name", "namespace", "selfLink", "resource", "time", llab2);
		List<Volumes> lvol1 = new List<Volumes> ();
		List<Volumes> lvol2 = new List<Volumes> ();
		lvol1.Add (new Volumes ("name","secret","hostpath"));
		lvol2.Add (new Volumes ("name","secret","hostpath"));
		List<VolumeMounts> lvolm1 = new List<VolumeMounts> ();
		List<VolumeMounts> lvolm2 = new List<VolumeMounts> ();
		lvolm1.Add (new VolumeMounts ("name",true,"mountpath"));
		lvolm2.Add (new VolumeMounts ("name",true,"mountpath"));
		Containers cont1 = new Containers ("name", "image", "resource", lvolm1);
		Containers cont2 = new Containers ("name", "image", "resource", lvolm2);
		Spec spc1 = new Spec (lvol1, cont1, "nodename");
		Spec spc2 = new Spec (lvol2, cont2, "nodename");
		ContainerStatuses contStatus1 = new ContainerStatuses ("name", "state", "lastState", "ready", "count", "image", "imageid", "contid");
		ContainerStatuses contStatus2 = new ContainerStatuses ("name", "state", "lastState", "ready", "count", "image", "imageid", "contid");
		Conditions cond1 = new Conditions ("type", "status", "lprove", "ltt");
		Conditions cond2 = new Conditions ("type", "status", "lprove", "ltt");
		Status sta1 = new Status ("phase", cond1, "hostip", "podip", "time", contStatus1);
		Status sta2 = new Status ("phase", cond2, "hostip", "podip", "time", contStatus2);
		Items label1 = new Items ("name", gameObject, "baseurl", mdata1, spc1, sta1);
		Items label2 = new Items ("name", gameObject, "baseurl", mdata2, spc2, sta2);

		if(label1 == label2) {
			Debug.Log ("equal");
		} else {
			Debug.Log ("not equal");
		}
		*/





		/*if (label1 == label2) {
			Debug.Log ("equal");
		} else {
			Debug.Log ("not equal");
		}*/
	}


	float startCoroutineTime = 0f;
	float coroutineWaitInterval = 1f;

	void Update() {

		// The API check is complete
		if (thisWwwCall.isDone) {

			// Create new text entries
			receivedText = thisWwwCall.text;
			cleanedText = PurgeOuterObject (thisWwwCall.text);

			// Hasn't been run before
			if (doneOnce == false) {
				podArray.pods = new List<Items> ();
				podArray.BuildJSON (receivedText, podArray.pods);  // To PodInfo.cs
				foreach (Items items in podArray.pods) {
					podInstantiation.CreatePod (items, headers);
				}

				doneOnce = true;
				// Has been run before
			} else {
				//  We have an update on something
				if (cleanedText != _cachedCleanedText) {
					podArray.changedPods = new List<Items> ();
					podArray.BuildJSON (receivedText, podArray.changedPods);  // To PodInfo.cs
					// We should compare the new to the old, individually
					foreach (Items changedPods in podArray.changedPods) {
						foreach (Items currentPods in podArray.pods) {
							if (changedPods.name == currentPods.name) {
								changedPods.gameObject = currentPods.gameObject;
								// Now check to see if the Items are equal
								if (currentPods != changedPods) {
									Debug.Log (changedPods.name + " is not equal" + currentPods.name);
									UpdateThisPod updateThisPod = GameObject.Find (changedPods.name).GetComponent<UpdateThisPod> ();
									updateThisPod.item = changedPods;
									updateThisPod.Copy ();
								}
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