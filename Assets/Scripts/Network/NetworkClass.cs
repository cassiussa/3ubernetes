// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;

public class NetworkClass : MonoBehaviour {

	public PodInfo podInfo;

	public string url = "http://supercass.com/bah.php";
	 string receivedText;
	 string _cachedReceivedText;

	WWW www;
	void Start() {
		podInfo = GetComponent<PodInfo> ();
		www = new WWW(url);
		//yield return www;
		//text = www.text;
		//Renderer renderer = GetComponent<Renderer>();
		//renderer.material.mainTexture = www.texture;
	}

	void Update() {
		if (www.isDone) {
			receivedText = www.text;	
			if (receivedText != _cachedReceivedText) {
				podInfo.BuildJSON(receivedText);  // To PodInfo.cs
				_cachedReceivedText = receivedText;
				Debug.Log (receivedText);
			}
			www = new WWW(url);
		}
	}

}