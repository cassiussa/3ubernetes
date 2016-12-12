using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List types

/*
 * 
 * The intention of this script is to create a single Class
 * whereby we can create any Kubernetes type based on the data
 * pulled in from the json produced from the PodsList call
 * 
 */

namespace Kubernetes {

	// PodList -> Items
	[System.Serializable] // Show it in the Inspector
	public class Items {
		public string name;
		public UnityEngine.GameObject gameObject;
		public string baseURL;
		public Metadata metadata;
		public Spec spec;
		public Status status;
		
		// Constructors
		public Items(string name, UnityEngine.GameObject gameObject, string baseURL, Metadata metadata, Spec spec, Status status) {
			this.name = name;
			this.gameObject = gameObject;
			this.baseURL = baseURL;
			this.metadata = metadata;
			this.spec = spec;
			this.status = status;
		}
		public Items() {  // Allow for New() instantiation
			this.name = "";
			this.gameObject = null;
			this.baseURL = "";
			this.metadata = new Metadata();
			this.spec = new Spec();
			this.status = new Status();
		}
		public Items(Items item) {
			this.name = item.name;
			this.gameObject = item.gameObject;
			this.baseURL = item.baseURL;
			this.metadata = item.metadata;
			this.spec = item.spec;
			this.status = item.status;
		}
		public static bool operator ==(Items first, Items second) {
			return (
				first.name == second.name &&
				first.gameObject == second.gameObject &&
				first.baseURL == second.baseURL &&
				first.metadata == second.metadata &&
				first.spec == second.spec &&
				first.status == second.status
			);
		}
		public static bool operator !=(Items first, Items second) {
			return !(
				first.name == second.name &&
				first.gameObject == second.gameObject &&
				first.baseURL == second.baseURL &&
				first.metadata == second.metadata &&
				first.spec == second.spec &&
				first.status == second.status
			);
		}
	}

	// PodList -> Items -> Metadata
	[System.Serializable] // Show it in the Inspector
	public class Metadata {
		public string name;
		public string Namespace;
		public string selfLink;
		public string resourceVersion;
		public string creationTimestamp;
		public List<Labels> labels;
		
		// Constructors
		public Metadata(string name, string Namespace, string selfLink, string resourceVersion, string creationTimestamp, List<Labels> labels) {
			this.name = name;
			this.Namespace = Namespace;
			this.selfLink = selfLink;
			this.resourceVersion = resourceVersion;
			this.creationTimestamp = creationTimestamp;
			this.labels = labels;
		}
		public Metadata() {  // Allow for New() instantiation
			this.name = "";
			this.Namespace = "";
			this.selfLink = "";
			this.resourceVersion = "";
			this.creationTimestamp = "";
			this.labels = new List<Labels>();
		}
		public static bool operator ==(Metadata first, Metadata second) {
			return (
				first.name == second.name &&
				first.Namespace == second.Namespace &&
				first.selfLink == second.selfLink &&
				first.resourceVersion == second.resourceVersion &&
				first.creationTimestamp == second.creationTimestamp &&
				CheckMatch(first.labels, second.labels)
			);


		}
		public static bool operator !=(Metadata first, Metadata second) {
			return !(
				first.name == second.name &&
				first.Namespace == second.Namespace &&
				first.selfLink == second.selfLink &&
				first.resourceVersion == second.resourceVersion &&
				first.creationTimestamp == second.creationTimestamp &&
				CheckMatch(first.labels, second.labels)
			);
		}
		static bool CheckMatch(List<Labels> l1, List<Labels> l2) {
			if (l1.Count != l2.Count)
				return false;
			for (int i = 0; i < l1.Count; i++) {
				if (l1[i] != l2[i])
					return false;
			}
			return true;
		}
	}

	// PodList -> Items -> Spec
	[System.Serializable] // Show it in the Inspector
	public class Spec {
		public List<Volumes> volumes;
		public List<Containers> containers;
		public string nodeName;
		
		// Constructors
		public Spec(List<Volumes> volumes, List<Containers> containers, string nodeName) {
			this.volumes = volumes;
			this.containers = containers;
			this.nodeName = nodeName;
		}
		public Spec() {  // Allow for New() instantiation
			this.volumes = new List<Volumes>();
			this.containers = new List<Containers>();
			this.nodeName = "";
		}

		public static bool operator ==(Spec first, Spec second) {
			return (
				CheckMatch(first.volumes, second.volumes) &&
				CheckMatch2(first.containers, second.containers) &&
				first.nodeName == second.nodeName
			);
		}
		public static bool operator !=(Spec first, Spec second) {
			return !(
				CheckMatch(first.volumes, second.volumes) &&
				CheckMatch2(first.containers, second.containers) &&
				first.nodeName == second.nodeName
			);
		}

		static bool CheckMatch(List<Volumes> l1, List<Volumes> l2) {
			if (l1.Count != l2.Count)
				return false;
			for (int i = 0; i < l1.Count; i++) {
				if (l1[i] != l2[i])
					return false;
			}
			return true;
		}
		static bool CheckMatch2(List<Containers> l1, List<Containers> l2) {
			if (l1.Count != l2.Count)
				return false;
			for (int i = 0; i < l1.Count; i++) {
				if (l1[i] != l2[i])
					return false;
			}
			return true;
		}
	}


	// PodList -> Items -> Spec -> Containers
	[System.Serializable] // Show it in the Inspector
	public class Containers {
		public string name;
		public string image;
		public string resources;
		public List<VolumeMounts> volumeMounts;
		
		// Constructors
		public Containers(string name, string image, string resources, List<VolumeMounts> volumeMounts) {
			this.name = name;
			this.image = image;
			this.resources = resources;
			this.volumeMounts = volumeMounts;
		}
		public Containers() {  // Allow for New() instantiation
			this.name = "";
			this.image = "";
			this.resources = "";
			this.volumeMounts = new List<VolumeMounts>();
		}
		public static bool operator ==(Containers first, Containers second) {
			return (
				first.name == second.name &&
				first.image == second.image &&
				first.resources == second.resources &&
				CheckMatch(first.volumeMounts, second.volumeMounts)
			);
		}
		public static bool operator !=(Containers first, Containers second) {
			return !(
				first.name == second.name &&
				first.image == second.image &&
				first.resources == second.resources &&
				CheckMatch(first.volumeMounts, second.volumeMounts)
			);
		}

		static bool CheckMatch(List<VolumeMounts> l1, List<VolumeMounts> l2) {
			if (l1.Count != l2.Count)
				return false;
			for (int i = 0; i < l1.Count; i++) {
				if (l1[i] != l2[i])
					return false;
			}
			return true;
		}
	}

	/*[System.Serializable] // Show it in the Inspector
	public class ContainersList {
		public List<Containers> volumeMounts = new List<Containers>();
		public ContainersList(List<Containers> volMounts) {
			this.volumeMounts = volMounts;
		}
		//public ContainersList(Containers volMounts) { }
		public static bool operator ==(ContainersList first, ContainersList second) {
			return (CheckMatch(first.volumeMounts, second.volumeMounts));
		}
		public static bool operator !=(ContainersList first, ContainersList second) {
			return !(CheckMatch(first.volumeMounts, second.volumeMounts));
		}
		static bool CheckMatch(List<Containers> l1, List<VolumeMounts> l2) {
			if (l1.Count != l2.Count)
				return false;
			for (int i = 0; i < l1.Count; i++) {
				if (l1[i] != l2[i])
					return false;
			}
			return true;
		}
	}*/


	// PodList -> Items -> Spec -> Volumes
	[System.Serializable] // Show it in the Inspector
	public class Volumes {
		public string name;
		public string secret;
		public string hostPath;

		// Constructors
		public Volumes(string name, string secret, string hostPath) {
			this.name = name;
			this.secret = secret;
			this.hostPath = hostPath;
		}
		public Volumes() {  // Allow for New() instantiation
			this.name = "";
			this.secret = "";
			this.hostPath = "";
		}
		public static bool operator ==(Volumes first, Volumes second) {
			return (
				first.name == second.name &&
				first.secret == second.secret &&
				first.hostPath == second.hostPath
			);
		}
		public static bool operator !=(Volumes first, Volumes second) {
			return !(
				first.name == second.name &&
				first.secret == second.secret &&
				first.hostPath == second.hostPath
			);
		}
	}
	// PodList -> Items -> Spec -> Volumes(List)
	[System.Serializable] // Show it in the Inspector
	public class VolumesList {
		public List<Volumes> volumeMounts = new List<Volumes>();
		public VolumesList(List<Volumes> volumeMounts) {
			this.volumeMounts = volumeMounts;
		}
		//public VolumesList(Volumes volumeMounts) {}
		public static bool operator ==(VolumesList first, VolumesList second) {
			return (CheckMatch(first.volumeMounts, second.volumeMounts));
		}
		public static bool operator !=(VolumesList first, VolumesList second) {
			return !(CheckMatch(first.volumeMounts, second.volumeMounts));
		}
		static bool CheckMatch(List<Volumes> l1, List<Volumes> l2) {
			if (l1.Count != l2.Count)
				return false;
			for (int i = 0; i < l1.Count; i++) {
				if (l1[i] != l2[i])
					return false;
			}
			return true;
		}
	}


	// PodList -> Items -> Spec -> Containers
	[System.Serializable] // Show it in the Inspector
	public class Status {
		public string phase;
		public Conditions conditions;
		public string hostIP;
		public string podIP;
		public string startTime;
		public ContainerStatuses containerStatuses;
		
		// Constructors
		public Status(string phase, Conditions conditions, string hostIP, string podIP, string startTime, ContainerStatuses containerStatuses) {
			this.phase = phase;
			this.conditions = conditions;
			this.hostIP = hostIP;
			this.podIP = podIP;
			this.startTime = startTime;
			this.containerStatuses = containerStatuses;
		}
		public Status() {  // Allow for New() instantiation
			this.phase = "";
			this.conditions = new Conditions();
			this.hostIP = "";
			this.podIP = "";
			this.startTime = "";
			this.containerStatuses = new ContainerStatuses();
		}
		public static bool operator ==(Status first, Status second) {
			return (
				first.phase == second.phase &&
				first.conditions == second.conditions &&
				first.hostIP == second.hostIP &&
				first.podIP == second.podIP &&
				first.startTime == second.startTime &&
				first.containerStatuses == second.containerStatuses
			);
		}
		public static bool operator !=(Status first, Status second) {
			return !(
				first.phase == second.phase &&
				first.conditions == second.conditions &&
				first.hostIP == second.hostIP &&
				first.podIP == second.podIP &&
				first.startTime == second.startTime &&
				first.containerStatuses == second.containerStatuses
			);
		}
	}

	// PodList -> Items -> Spec -> Conditions
	[System.Serializable] // Show it in the Inspector
	public class Conditions {
		public string type;
		public string status;
		public string lastProbeTime;
		public string lastTransitionTime;
		
		// Constructors
		public Conditions(string type, string status, string lastProbeTime, string lastTransitionTime) {
			this.type = type;
			this.status = status;
			this.lastProbeTime = lastProbeTime;
			this.lastTransitionTime = lastTransitionTime;
		}
		public Conditions() {  // Allow for New() instantiation
			this.type = "";
			this.status = "";
			this.lastProbeTime = "";
			this.lastTransitionTime = "";
		}
		public static bool operator ==(Conditions first, Conditions second) {
			return (
				first.type == second.type &&
				first.status == second.status &&
				first.lastProbeTime == second.lastProbeTime &&
				first.lastTransitionTime == second.lastTransitionTime
			);
		}
		public static bool operator !=(Conditions first, Conditions second) {
			return !(
				first.type == second.type &&
				first.status == second.status &&
				first.lastProbeTime == second.lastProbeTime &&
				first.lastTransitionTime == second.lastTransitionTime
			);
		}
	}


	// PodList -> Items -> Spec -> Containers -> volumeMounts
	[System.Serializable] // Show it in the Inspector
	public class VolumeMounts {
		public string name;
		public bool readOnly;
		public string mountPath;
		
		// Constructors
		public VolumeMounts(string name, bool readOnly, string mountPath) {
			this.name = name;
			this.readOnly = readOnly;
			this.mountPath = mountPath;
		}
		public VolumeMounts() {  // Allow for New() instantiation
			this.name = "";
			this.readOnly = false;
			this.mountPath = "";
		}
		public static bool operator ==(VolumeMounts first, VolumeMounts second) {
			return (
				first.name == second.name &&
				first.readOnly == second.readOnly &&
				first.mountPath == second.mountPath
			);
		}
		public static bool operator !=(VolumeMounts first, VolumeMounts second) {
			return !(
				first.name == second.name &&
				first.readOnly == second.readOnly &&
				first.mountPath == second.mountPath
			);
		}
	}

	// PodList -> Items -> Spec -> Containers -> volumeMounts(List)
	[System.Serializable] // Show it in the Inspector
	public class VolumeMountsList {
		public List<VolumeMounts> volumeMounts = new List<VolumeMounts>();
		public VolumeMountsList(List<VolumeMounts> volMounts) {
			this.volumeMounts = volMounts;
		}
		//public VolumeMountsList(VolumeMounts volMounts) { }
		public static bool operator ==(VolumeMountsList first, VolumeMountsList second) {
			return (CheckMatch(first.volumeMounts, second.volumeMounts));
		}
		public static bool operator !=(VolumeMountsList first, VolumeMountsList second) {
			return !(CheckMatch(first.volumeMounts, second.volumeMounts));
		}
		static bool CheckMatch(List<VolumeMounts> l1, List<VolumeMounts> l2) {
			if (l1.Count != l2.Count)
				return false;
			for (int i = 0; i < l1.Count; i++) {
				if (l1[i] != l2[i])
					return false;
			}
			return true;
		}
	}



	// PodList -> Items -> Status -> Container Statuses
	[System.Serializable] // Show it in the Inspector
	public class ContainerStatuses {
		public string name;
		public string state;
		public string lastState;
		public string ready;
		public string restartCount;
		public string image;
		public string imageID;
		public string containerID;

		// Constructors
		public ContainerStatuses(string name, string state, string lastState, string ready, string restartCount, string image, string imageID, string containerID) {
			this.name = name;
			this.state = state;
			this.lastState = lastState;
			this.ready = ready;
			this.restartCount = restartCount;
			this.image = image;
			this.imageID = imageID;
			this.containerID = containerID;
		}
		public ContainerStatuses() {  // Allow for New() instantiation
			this.name = "";
			this.state = "";
			this.lastState = "";
			this.ready = "";
			this.restartCount = "";
			this.image = "";
			this.imageID = "";
			this.containerID = "";
		}
		public static bool operator ==(ContainerStatuses first, ContainerStatuses second) {
			return (
				first.name == second.name &&
				first.state == second.state &&
				first.lastState == second.lastState &&
				first.ready == second.ready &&
				first.restartCount == second.restartCount &&
				first.image == second.image &&
				first.imageID == second.imageID &&
				first.containerID == second.containerID
			);
		}
		public static bool operator !=(ContainerStatuses first, ContainerStatuses second) {
			return !(
				first.name == second.name &&
				first.state == second.state &&
				first.lastState == second.lastState &&
				first.ready == second.ready &&
				first.restartCount == second.restartCount &&
				first.image == second.image &&
				first.imageID == second.imageID &&
				first.containerID == second.containerID
			);
		}
	}


	// PodList -> Items -> Spec -> Containers -> volumeMounts
	[System.Serializable] // Show it in the Inspector
	public class Labels{
		public string key;
		public string value;

		// Constructors
		public Labels(string key, string value) {
			this.key = key;
			this.value = value;
		}
		public Labels(Labels labels) {
			this.key = labels.key;
			this.value = labels.value;
		}

		public Labels() { }

		public static bool operator == (Labels first, Labels second) {
			return (first.key == second.key && first.value == second.value);
		}
		public static bool operator != (Labels first, Labels second) {
			return !(first.key == second.key && first.value == second.value);
		}
	}

}