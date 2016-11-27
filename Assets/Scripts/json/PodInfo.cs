using UnityEngine;
using System.Collections;
using System;

public class PodInfo : MonoBehaviour {

	string encodedString = "{\"field1\": 0.5,\"field2\": \"sampletext\",\"field3\": [1,2,3]}";

	public void BuildJSON(string encodedString) {
		JSONObject j = new JSONObject(encodedString);
		//accessData(j);

		j.GetField("items", delegate(JSONObject hits) {
				foreach (JSONObject pod in hits.list) {
				pod.GetField("metadata", delegate(JSONObject metadatas) {
					foreach (JSONObject metadata in metadatas.list) {
						Debug.Log(metadata);
					}
				});
					//Debug.Log(pod);
				}

		}, delegate(string name) {  //"name" will be equal to the name of the missing field.  In this case, "hits"
			Debug.LogWarning("no game sessions");
		});
	}

	/*void accessData(JSONObject obj){
		switch(obj.type){
		case JSONObject.Type.OBJECT:
			for(int i = 0; i < obj.list.Count; i++){
				string key = (string)obj.keys[i];
				JSONObject j = (JSONObject)obj.list[i];
				Debug.Log("Key: "+key);
				accessData(j);
			}
			break;
		case JSONObject.Type.ARRAY:
			foreach(JSONObject j in obj.list){
				accessData(j);
			}
			break;
		case JSONObject.Type.STRING:
			Debug.Log("Val: "+obj.str);
			break;
		case JSONObject.Type.NUMBER:
			Debug.Log("Val: "+obj.n);
			break;
		case JSONObject.Type.BOOL:
			Debug.Log("Val: "+obj.b);
			break;
		case JSONObject.Type.NULL:
			Debug.Log("NULL");
			break;
			
		}
	}*/
	
}