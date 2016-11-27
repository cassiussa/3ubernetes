using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List types
using System;
using Kubernetes;

public class PodInfo : MonoBehaviour {

	string encodedString = "{\"field1\": 0.5,\"field2\": \"sampletext\",\"field3\": [1,2,3]}";

	public Items item = new Items();
	public List<Items> itemList = new List<Items>();

	public void BuildJSON(string encodedString) {
		JSONObject podsList = new JSONObject(encodedString);

		podsList.GetField(
			"items", delegate(JSONObject itemsList) {
			foreach (JSONObject thisItem in itemsList.list) {
				Debug.Log(thisItem);
				Metadata metadata = MetadataJSON(thisItem);
				Items _item = new Items(metadata, thisItem["spec"].ToString(), thisItem["status"].ToString());
				item = _item;
				itemList.Add(item);
				}
			},
			delegate(string name) {  // 'name' will be equal to the name of the missing field - "itemsList"
				Debug.LogWarning("no itemsList(s)");
			}
		);

	}

	public Metadata MetadataJSON(JSONObject metadata) {
		Metadata _meta_ = new Metadata ();
		metadata.GetField ("metadata", delegate(JSONObject metadatas) {
			Debug.Log (metadatas ["selfLink"]);
			_meta_ = new Metadata (
				metadatas ["name"].ToString ().Replace("\"", ""),
				metadatas ["namespace"].ToString ().Replace("\"", ""),
				metadatas ["selfLink"].ToString ().Replace("\"", ""),
				metadatas ["resourceVersion"].ToString ().Replace("\"", ""),
				metadatas ["creationTimestamp"].ToString ().Replace("\"", ""),
				metadatas ["labels"].ToString ()
			);
		});
		return _meta_;
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