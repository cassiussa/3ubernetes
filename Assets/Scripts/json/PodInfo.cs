using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List types
using System;
using Kubernetes;

public class PodInfo : MonoBehaviour {

	string encodedString = ""; // "{\"field1\": 0.5,\"field2\": \"sampletext\",\"field3\": [1,2,3]}";
	
	public List<Items> itemList = new List<Items>();
	public List<Status> statusList = new List<Status>();
	public List<Containers> containerList = new List<Containers>();

	public void BuildJSON(string encodedString) {
		JSONObject podsList = new JSONObject(encodedString);

		podsList.GetField(
			"items", delegate(JSONObject itemsList) {
			foreach (JSONObject thisItem in itemsList.list) {
				Debug.Log(thisItem);
				Metadata metadata = MetadataJSON(thisItem);
				Spec spec = SpecJSON(thisItem);
				Status status = StatusJSON(thisItem);
				// Now assemble the items
				Items _item = new Items(metadata, spec, status);
				itemList.Add(_item);
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
			//Debug.Log (metadatas ["selfLink"]);
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

	public Spec SpecJSON(JSONObject spec) {
		Spec _spec_ = new Spec ();
		spec.GetField ("spec", delegate(JSONObject specs) {
			Containers containers = ContainersJSON(specs);

			_spec_ = new Spec (
				specs ["volumes"].ToString ().Replace("\"", ""),
				containers,
				specs ["nodeName"].ToString ().Replace("\"", "")
				);
		});
		return _spec_;
	}

	public Status StatusJSON(JSONObject status) {
		Status _status_ = new Status ();
		status.GetField ("status", delegate(JSONObject statuses) {
			//Debug.Log (metadatas ["selfLink"]);
			_status_ = new Status (
				statuses ["phase"].ToString ().Replace("\"", ""),
				statuses ["conditions"].ToString ().Replace("\"", ""),
				statuses ["hostIP"].ToString ().Replace("\"", ""),
				statuses ["podIP"].ToString ().Replace("\"", ""),
				statuses ["startTime"].ToString ().Replace("\"", ""),
				statuses ["containerStatuses"].ToString ().Replace("\"", "")
				);
		});
		return _status_;
	}




	public Containers ContainersJSON(JSONObject container) {
		Containers _container_ = new Containers ();
		container.GetField ("containers", delegate(JSONObject containers) {
			foreach(JSONObject thisContainer in containers) {
				Debug.LogError (thisContainer.ToString());
				_container_ = new Containers (
					thisContainer ["name"].ToString ().Replace("\"", ""),
					thisContainer ["image"].ToString ().Replace("\"", ""),
					thisContainer ["resources"].ToString ().Replace("\"", ""),
					thisContainer ["volumeMounts"].ToString ().Replace("\"", "")
				);
			}
		});
		return _container_;
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