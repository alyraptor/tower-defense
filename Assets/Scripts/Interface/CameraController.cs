using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float smoothTime = 0.15f;
	public float zoomMod;

    private Camera cam;

    private float zoomStartTime;
	private float cameraHeight = 2.3f;
	private Vector3 velocity = Vector3.zero;

	private float zoom = 1f;
	private float currentZoom;

	void Awake() {
        cam = gameObject.GetComponent<Camera>();
		currentZoom = cam.orthographicSize;
	}

	void Update () {
		if (target != null) {
			Vector3 goalPos = target.position;
			goalPos.y = cameraHeight;
			goalPos.x = target.position.x - 1f;
			goalPos.z = target.position.z - 1f;
			transform.position = Vector3.SmoothDamp (transform.position, goalPos, ref velocity, smoothTime);

            zoomStartTime = Time.time;
            float fracZoom = (Time.time - zoomStartTime) / smoothTime;
            if (fracZoom <= 1) {
                cam.orthographicSize = Mathf.Lerp(currentZoom, zoom, fracZoom);
            }
		}
	}

	public void Zoom (float zoomChange) {
		zoom = zoom + (zoomChange / zoomMod);
	}
}