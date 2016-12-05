﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List types
using Kubernetes;

public class BuildPodVariable : MonoBehaviour {

	public string name = "";
	public string baseURL = "";
	public Metadata metadata = new Metadata();
	public Spec spec = new Spec();
	public Status status = new Status();
	public Metadata _meta = new Metadata ();

	public void BeginJSON (string apiText) {
		_meta = MetadataJSON (new JSONObject (apiText));
	}

	public Metadata MetadataJSON(JSONObject _metadata) {
		Debug.Log (_metadata);
		Metadata _meta = new Metadata ();
		_metadata.GetField ("metadata", delegate(JSONObject metadata) {
			Debug.Log(metadata);
			List<Labels> label = LabelsJSON(metadata);
			/*_meta = new Metadata (
				metadata ["name"].ToString ().Replace("\"", ""),
				metadata ["namespace"].ToString ().Replace("\"", ""),
				metadata ["selfLink"].ToString ().Replace("\"", ""),
				metadata ["resourceVersion"].ToString ().Replace("\"", ""),
				metadata ["creationTimestamp"].ToString ().Replace("\"", ""),
				label
			);*/
		}, delegate(string name) {  }
		);
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
				}, delegate(string name) {  });

				theseVolumes.GetField("secret", delegate(JSONObject secret) {
					_volume.secret = secret["secretName"].ToString ().Replace("\"", "");
				}, delegate(string secret) {  });

				theseVolumes.GetField("hostPath", delegate(JSONObject hostPath) {
					foreach(JSONObject thisHostPath in hostPath) {
						_volume.hostPath = thisHostPath.ToString ().Replace("\"", "");
					}

				}, delegate(string hostPath) {  });

				volumesList.Add(_volume);
			}
		},  // Allow null values
			delegate(string name) {  // 'name' will be equal to the name of the missing field - "itemsList"
				Debug.LogWarning("no itemsList(s)");
			});
		return volumesList;
	}

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
}
