using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float smoothTime = 0.15f;
    public float zoomDuration = 0.15f;
	public float zoomMod = 2f;
	public float minZoom = 2f;
	public float maxZoom = 20f;
	public float zoomDefault = 3.5f;

    private Camera cam;

    private float zoomStartTime;
	private float cameraHeight = 2.3f;
	private Vector3 velocity = Vector3.zero;

	private float zoomTarget;
	private float zoom;
	private bool isZooming;


	void Awake() {
        cam = gameObject.GetComponent<Camera>();
		zoom = zoomTarget = cam.orthographicSize = zoomDefault;
	}

	void Update () {
		if (target != null) {
			Vector3 goalPos = target.position;
			goalPos.y = cameraHeight;
			goalPos.x = target.position.x - 1f;
			goalPos.z = target.position.z - 1f;
			transform.position = Vector3.SmoothDamp (transform.position, goalPos, ref velocity, smoothTime);

            if(isZooming) {
				float fracZoom = (Time.time - zoomStartTime) / zoomDuration;
				if (fracZoom <= 1) {
					cam.orthographicSize = Mathf.Lerp(zoom, zoomTarget, fracZoom);
				} else {
					cam.orthographicSize = zoomTarget;
					isZooming = false;
				}
			}
		}
	}

	public void Zoom (float zoomChange) {

        zoom = cam.orthographicSize;

		// Invert change in Zoom to account for ortho size being on a reverse scale
		zoomChange = -zoomChange;
		zoomTarget = zoomTarget + (zoomChange / zoomMod);

		// If zoom is close to default, snap it to the default.
		zoomTarget = Mathf.Abs(zoomTarget - zoomDefault) <= 0.25f ? zoomDefault : zoomTarget;
		zoomTarget = zoomTarget < minZoom ? minZoom : zoomTarget > maxZoom ? maxZoom : zoomTarget;

        zoomStartTime = Time.time;
		isZooming = true;
	}
}