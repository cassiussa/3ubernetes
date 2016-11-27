using UnityEngine;
using System.Collections;

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
		public string volumes;
		public string containers;
		public string nodeName;
		
		// Constructors
		public Spec(string volumes, string containers, string nodeName) {
			this.volumes = volumes;
			this.containers = containers;
			this.nodeName = nodeName;
		}
		public Spec() {  // Allow for New() instantiation
			this.volumes = "";
			this.containers = "";
			this.nodeName = "";
		}
	}


	// PodList -> Items -> Spec -> Containers
	[System.Serializable] // Show it in the Inspector
	public class Containers {
		public string name;
		public string image;
		public string resources;
		public string volumeMounts;
		
		// Constructors
		public Containers(string name, string image, string resources, string volumeMounts) {
			this.name = name;
			this.image = image;
			this.resources = resources;
			this.volumeMounts = volumeMounts;
		}
		public Containers() {  // Allow for New() instantiation
			this.name = "";
			this.image = "";
			this.resources = "";
			this.volumeMounts = "";
		}
	}

	// PodList -> Items -> Spec -> Containers
	[System.Serializable] // Show it in the Inspector
	public class Status {
		public string phase;
		public string conditions;
		public string hostIP;
		public string podIP;
		public string startTime;
		public string containerStatuses;
		
		// Constructors
		public Status(string phase, string conditions, string hostIP, string podIP, string startTime, string containerStatuses) {
			this.phase = phase;
			this.conditions = conditions;
			this.hostIP = hostIP;
			this.podIP = podIP;
			this.startTime = startTime;
			this.containerStatuses = containerStatuses;
		}
		public Status() {  // Allow for New() instantiation
			this.phase = "";
			this.conditions = "";
			this.hostIP = "";
			this.podIP = "";
			this.startTime = "";
			this.containerStatuses = "";
		}
	}


}