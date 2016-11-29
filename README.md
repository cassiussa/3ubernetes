# 3ubernetes
## Pronunciation: Euber-Net-ease
### 3D VR Kubernetes Realtime

# Don't bother downloading this yet.  It's miles away from being useful at the moment.

# Stream of changes in Kubernetes via API
https://104.198.50.196/api/v1/pods?watch=true&pretty=true&fieldSelector=metadata.name=secrets-test-4xg8x&resourceVersion=380416


# Notes
#### SOMETHING HAS CHANGED
int : metadata.resourceVersion - when this changes, that means the Pod has changed

#### CONTAINER STARTING UP
bool : status.containerStatuses.ready
bool : status.conditions.status
string : status.conditions.message - Example: "'containers with unready status: [ocelot-nginx]'""
string : status.conditions.reason - Example: "ContainersNotReady"

