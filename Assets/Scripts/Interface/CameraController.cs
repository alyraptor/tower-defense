using System.Collections;
using UnityEngine;

namespace TowerDefense {
    public class CameraController : MonoBehaviour {

		public float smoothTime = 0.15f; // How much to smooth camera movement (in seconds)
		public float zoomDuration = 0.15f; // How long a zoom lasts (in seconds)
		public float zoomMod = 1.5f;  // Dampen user input
		public float rotationDuration = 5f; // How long a rotation lasts (in seconds)

		private Camera cam;
		private GameObject camParent;
		private Transform target; // The camera will follow this target

		private float minZoom = 2f; // Minimum the player can zoom (in Ortho camera size)
		private float maxZoom = 15f; // Maximum the player can zoom (in Ortho camera size)
		private float zoomDefault = 5f; // Starting Zoom level (in Ortho camera size)
		private float cameraHeight = 1.3f;
		private Vector3 velocity = Vector3.zero; // Starting velocity for SmoothDamp

		private float zoom; // Start zoom for method
		private float zoomTarget; // End zoom for method
		private float zoomStartTime; // Reference of time when method starts
		private bool isZooming;

		private Quaternion rotation; // Start rotation for method
		private Quaternion rotationTarget; // End rotation for method
		private float rotationStartTime; // Reference of time when method starts
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
				camParent.transform.position = Vector3.SmoothDamp(camParent.transform.position, goalPos, ref velocity, smoothTime);

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

		public void Zoom (float zoomChange) { // Ease-in/Ease-out Zoom

			isZooming = true;
			zoomStartTime = Time.time;
			zoom = cam.orthographicSize;

			// Invert change in Zoom to account for ortho size being on a reverse scale
			zoomChange = -zoomChange;
			zoomTarget = zoomTarget + (zoomChange / zoomMod);

			// If zoom is close to default, snap it to the default.
			zoomTarget = Mathf.Abs(zoomTarget - zoomDefault) <= 0.25f ? zoomDefault : zoomTarget;
			zoomTarget = zoomTarget < minZoom ? minZoom : zoomTarget > maxZoom ? maxZoom : zoomTarget;
		}

		public void Rotate (float rotationChange) {

            isRotating = true;
            rotationStartTime = Time.time;
            rotation = camParent.transform.rotation;

            rotationTarget = rotation * Quaternion.Euler(new Vector3(0, rotationChange, 0));
		}
	}
}