using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float smoothTime = 0.3f;

	private Vector3 velocity = Vector3.zero;

	void Update () {
		Vector3 goalPos = target.position;
		goalPos.y = transform.position.y;
		goalPos.x = target.position.x - 1f;
		goalPos.z = target.position.z - 1f;
		transform.position = Vector3.SmoothDamp (transform.position, goalPos, ref velocity, smoothTime);
	}
}