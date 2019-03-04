using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float smoothTime = 0.15f;

	private float cameraHeight = 3f;
	private Vector3 velocity = Vector3.zero;

	void Update () {
		if (target != null) {
			Vector3 goalPos = target.position;
			goalPos.y = cameraHeight;
			goalPos.x = target.position.x - 1f;
			goalPos.z = target.position.z - 1f;
			transform.position = Vector3.SmoothDamp (transform.position, goalPos, ref velocity, smoothTime);
		}
	}
}