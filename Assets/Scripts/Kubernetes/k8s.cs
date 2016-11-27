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
		public string spec;
		public string status;
		
		// Constructors
		public Items(Metadata metadata, string spec, string status) {
			this.metadata = metadata;
			this.spec = spec;
			this.status = status;
		}
	}

	// PodList -> Items -> Metadata
	[System.Serializable] // Show it in the Inspector
	public class Metadata {
		public string name;
		//public string generateName;
		public string Namespace;
		public string selfLink;
		public string resourceVersion;
		public string creationTimestamp;
		public string labels;
		
		// Constructors
		public Metadata(string name, string Namespace, string selfLink, string resourceVersion, string creationTimestamp, string labels) {
			this.name = name;
			//this.generateName = generateName;
			this.Namespace = Namespace;
			this.selfLink = selfLink;
			this.resourceVersion = resourceVersion;
			this.creationTimestamp = creationTimestamp;
			this.labels = labels;
		}
	}

	// PodList -> Items -> Spec
	[System.Serializable] // Show it in the Inspector
	public class Spec {
		public string volumes;
		public Containers containers;
		public string nodeName;
		
		// Constructors
		public Spec(string volumes, Containers containers, string nodeName) {
			this.volumes = volumes;
			this.containers = containers;
			this.nodeName = nodeName;
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
	}
}