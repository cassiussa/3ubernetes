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
		public Metadata metadata;
		public Spec spec;
		public Status status;
		
		// Constructors
		public Items(Metadata metadata, Spec spec, Status status) {
			this.metadata = metadata;
			this.spec = spec;
			this.status = status;
		}
		public Items() {  // Allow for New() instantiation
			this.metadata = new Metadata();
			this.spec = new Spec();
			this.status = new Status();
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
		public string labels;
		
		// Constructors
		public Metadata(string name, string Namespace, string selfLink, string resourceVersion, string creationTimestamp, string labels) {
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
			this.labels = "";
		}
	}

	// PodList -> Items -> Spec
	[System.Serializable] // Show it in the Inspector
	public class Spec {
		public List<Volumes> volumes;
		public Containers containers;
		public string nodeName;
		
		// Constructors
		public Spec(List<Volumes> volumes, Containers containers, string nodeName) {
			this.volumes = volumes;
			this.containers = containers;
			this.nodeName = nodeName;
		}
		public Spec() {  // Allow for New() instantiation
			this.volumes = new List<Volumes>();
			this.containers = new Containers();
			this.nodeName = "";
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
	}

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
	}
	// PodList -> Items -> Spec -> Volumes(List)
	[System.Serializable] // Show it in the Inspector
	public class VolumesList {
		public List<Volumes> volumeMounts = new List<Volumes>();
		public VolumesList(Volumes volumeMounts) {}
	}


	// PodList -> Items -> Spec -> Containers
	[System.Serializable] // Show it in the Inspector
	public class Status {
		public string phase;
		public Conditions conditions;
		public string hostIP;
		public string podIP;
		public string startTime;
		public string containerStatuses;
		
		// Constructors
		public Status(string phase, Conditions conditions, string hostIP, string podIP, string startTime, string containerStatuses) {
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
			this.containerStatuses = "";
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
	}

	// PodList -> Items -> Spec -> Containers -> volumeMounts(List)
	[System.Serializable] // Show it in the Inspector
	public class VolumeMountsList {
		public List<VolumeMounts> volumeMounts = new List<VolumeMounts>();
		public VolumeMountsList(VolumeMounts volumeMounts) { }
	}


}