using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List types
using System;
using Kubernetes;

public class PodInfo : MonoBehaviour {

	string encodedString = ""; // "{\"field1\": 0.5,\"field2\": \"sampletext\",\"field3\": [1,2,3]}";
	
	public List<Items> pods = new List<Items>();
	//public List<Status> statusList = new List<Status>();
	//public List<Containers> containerList = new List<Containers>();

	public void BuildJSON(string encodedString) {
		JSONObject podsList = new JSONObject(encodedString);

		podsList.GetField(
			"items", delegate(JSONObject itemsList) {
			foreach (JSONObject thisItem in itemsList.list) {
				//Debug.Log(thisItem);
				Metadata metadata = MetadataJSON(thisItem);
				Spec spec = SpecJSON(thisItem);
				Status status = StatusJSON(thisItem);
				// Now assemble the items
				Items _item = new Items(metadata.name, metadata, spec, status);
					pods.Add(_item);
			}
			},
			delegate(string name) {  // 'name' will be equal to the name of the missing field - "itemsList"
				Debug.LogWarning("no itemsList(s)");
			}
		);
	}



	public Metadata MetadataJSON(JSONObject metadata) {
		Metadata _meta = new Metadata ();
		metadata.GetField ("metadata", delegate(JSONObject metadatas) {
			List<Labels> label = LabelsJSON(metadatas);
			_meta = new Metadata (
				metadatas ["name"].ToString ().Replace("\"", ""),
				metadatas ["namespace"].ToString ().Replace("\"", ""),
				metadatas ["selfLink"].ToString ().Replace("\"", ""),
				metadatas ["resourceVersion"].ToString ().Replace("\"", ""),
				metadatas ["creationTimestamp"].ToString ().Replace("\"", ""),
				label
			);
		});
		return _meta;
	}

	public Spec SpecJSON(JSONObject spec) {
		Spec _spec = new Spec ();
		spec.GetField ("spec", delegate(JSONObject specs) {
			Containers containers = ContainersJSON(specs);
			List<Volumes> volumes = VolumesJSON(specs);
			_spec = new Spec (
				volumes,
				containers,
				specs ["nodeName"].ToString ().Replace("\"", "")
				);
		});
		return _spec;
	}

	public Status StatusJSON(JSONObject status) {
		Status _status = new Status ();
		status.GetField ("status", delegate(JSONObject statuses) {
			Conditions conditions = ConditionsJSON (statuses);
			ContainerStatuses containerStatuses = ContainerStatusesJSON (statuses);
			_status = new Status (
				statuses ["phase"].ToString ().Replace("\"", ""),
				conditions,
				statuses ["hostIP"].ToString ().Replace("\"", ""),
				statuses ["podIP"].ToString ().Replace("\"", ""),
				statuses ["startTime"].ToString ().Replace("\"", ""),
				containerStatuses
				);
		});
		return _status;
	}




	public Containers ContainersJSON(JSONObject container) {
		Containers _container = new Containers ();
		container.GetField ("containers", delegate(JSONObject containers) {
			foreach(JSONObject thisContainer in containers) {
				List<VolumeMounts> thisVolumeMount = VolumeMountsJSON(thisContainer);
				_container = new Containers (
					thisContainer ["name"].ToString ().Replace("\"", ""),
					thisContainer ["image"].ToString ().Replace("\"", ""),
					thisContainer ["resources"].ToString ().Replace("\"", ""),
					thisVolumeMount
					);
			}
		});
		return _container;
	}

	public Conditions ConditionsJSON(JSONObject conditions) {
		Conditions _condition = new Conditions ();
		conditions.GetField ("conditions", delegate(JSONObject _conditions) {
			foreach(JSONObject theseConditions in _conditions) {
				_condition = new Conditions (
					theseConditions ["type"].ToString ().Replace("\"", ""),
					theseConditions ["status"].ToString ().Replace("\"", ""),
					theseConditions ["lastProbeTime"].ToString ().Replace("\"", ""),
					theseConditions ["lastTransitionTime"].ToString ().Replace("\"", "")
					);
			}
		});
		return _condition;
	}

	public ContainerStatuses ContainerStatusesJSON(JSONObject containerStatuses) {
		ContainerStatuses _condition = new ContainerStatuses ();
		containerStatuses.GetField ("containerStatuses", delegate(JSONObject _containerStatuses) {
			foreach(JSONObject theseContainerStatuses in _containerStatuses) {
				_condition = new ContainerStatuses (
					theseContainerStatuses ["name"].ToString ().Replace("\"", ""),
					theseContainerStatuses ["state"].ToString ().Replace("\"", ""),
					theseContainerStatuses ["lastState"].ToString ().Replace("\"", ""),
					theseContainerStatuses ["ready"].ToString ().Replace("\"", ""),
					theseContainerStatuses ["restartCount"].ToString ().Replace("\"", ""),
					theseContainerStatuses ["image"].ToString ().Replace("\"", ""),
					theseContainerStatuses ["imageID"].ToString ().Replace("\"", ""),
					theseContainerStatuses ["containerID"].ToString ().Replace("\"", "")
				);
			}
		});
		return _condition;
	}

	public List<VolumeMounts> VolumeMountsJSON(JSONObject volumeMounts) {
		List<VolumeMounts> volumeMountsList = new List<VolumeMounts>();
		volumeMounts.GetField ("volumeMounts", delegate(JSONObject _volumeMounts) {
			foreach(JSONObject theseVolumeMounts in _volumeMounts) {
				bool isReadOnly = false;
				if (theseVolumeMounts ["readOnly"] == null)
					isReadOnly = false;
				else if(theseVolumeMounts ["readOnly"].ToString ().Replace("\"", "") == "true")
					isReadOnly = true;
				else
					isReadOnly = false;

				VolumeMounts _volumeMount = new VolumeMounts (
					theseVolumeMounts ["name"].ToString ().Replace("\"", ""),
					isReadOnly,
					theseVolumeMounts ["mountPath"].ToString ().Replace("\"", "")
				);
				volumeMountsList.Add(_volumeMount);
			}
		});
		return volumeMountsList;
	}


	public List<Volumes> VolumesJSON(JSONObject volumeMounts) {
		List<Volumes> volumesList = new List<Volumes>();
		volumeMounts.GetField ("volumes", delegate(JSONObject volumes) {
			foreach(JSONObject theseVolumes in volumes) {
				Volumes _volume = new Volumes ();

				theseVolumes.GetField("name", delegate(JSONObject name) {
					_volume.name = name.ToString ().Replace("\"", "");
				}, delegate(string name) { Debug.LogWarning("no name value"); });

				theseVolumes.GetField("secret", delegate(JSONObject secret) {
					_volume.secret = secret["secretName"].ToString ().Replace("\"", "");
				}, delegate(string secret) { Debug.LogWarning("no secret value"); });

				theseVolumes.GetField("hostPath", delegate(JSONObject hostPath) {
					foreach(JSONObject thisHostPath in hostPath) {
						_volume.hostPath = thisHostPath.ToString ().Replace("\"", "");
					}

				}, delegate(string hostPath) { Debug.LogWarning("no hostPath value"); });

				volumesList.Add(_volume);
			}
		},  // Allow null values
			delegate(string name) {  // 'name' will be equal to the name of the missing field - "itemsList"
				Debug.LogWarning("no itemsList(s)");
		});
		return volumesList;
	}


	/******/
	public List<Labels> LabelsJSON(JSONObject metadata) {
		List<Labels> labelsList = new List<Labels>();
		metadata.GetField ("labels", delegate(JSONObject labels) {
			for(int i=0;i<labels.list.Count;i++) {
				string key = labels.keys[i].ToString();
				string value = labels.list[i].ToString().Replace("\"", "");;
				Labels _label = new Labels();
				_label.key = key;
				_label.value = value;
				labelsList.Add(_label);
			}

		});
		return labelsList;
	}
	/******/

	void accessData(JSONObject obj){
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
	}
	
}