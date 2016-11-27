// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {

	public string url = "http://supercass.com/bah.php";
	public string receivedText;
	public string _cachedReceivedText;

	WWW www;
	void Start() {
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
				_cachedReceivedText = receivedText;
				Debug.Log (receivedText);
			}
			www = new WWW(url);
		}
	}

}