using System.Collections;
using UnityEngine;

namespace TowerDefense {
    public class CameraController : MonoBehaviour {

		public float smoothTime = 0.15f;
		public float zoomDuration = 0.15f;
		public float zoomMod = 1.5f;

		public float rotationDuration = 5f;

		private Camera cam;
		private GameObject camParent;
		private Transform target;

		private float minZoom = 2f;
		private float maxZoom = 15f;
		private float zoomDefault = 5f;
		private float cameraHeight = 1.3f;
		private Vector3 velocity = Vector3.zero;

		private float zoom;
		private float zoomTarget;
		private float zoomStartTime;
		private bool isZooming;

		private Quaternion rotation;
		private Quaternion rotationTarget;
		private float rotationStartTime;
		private bool isRotating;

		void Awake() {
			cam = gameObject.GetComponent<Camera>();
			camParent = cam.transform.parent.gameObject;
			zoom = zoomTarget = cam.orthographicSize = zoomDefault;
			target = GameObject.FindWithTag("Player").transform;
		}

		void Update () {
			if (target != null) {
				Vector3 goalPos = new Vector3(target.position.x, cameraHeight, target.position.z);
				camParent.transform.position = Vector3.SmoothDamp (camParent.transform.position, goalPos, ref velocity, smoothTime);

				if(isZooming) {
					float fracZoom = (Time.time - zoomStartTime) / zoomDuration;
					if (fracZoom <= 1) {
						cam.orthographicSize = Mathf.Lerp(zoom, zoomTarget, fracZoom);
					} else {
						cam.orthographicSize = zoomTarget;
						isZooming = false;
					}
				}
				if(isRotating) {
                    float fracRotation = (Time.time - rotationStartTime) / rotationDuration;
                    if (fracRotation <= 1) {
                        camParent.transform.rotation = Quaternion.Slerp(rotation, rotationTarget, fracRotation);
                    } else {
                        camParent.transform.rotation = rotationTarget;
                        isRotating = false;
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

		public void Rotate (float rotationChange) {

            rotationStartTime = Time.time;

            rotation = camParent.transform.rotation;
            rotationTarget = rotation * Quaternion.Euler(new Vector3(0, rotationChange, 0)); // this adds a 90 degrees Y rotation

            Debug.Log(rotation);
            Debug.Log(rotationTarget);

            isRotating = true;
		}
	}
}