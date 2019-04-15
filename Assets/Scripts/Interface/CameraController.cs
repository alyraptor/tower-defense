using System.Collections;
using UnityEngine;

namespace TowerDefense {
    public class CameraController : MonoBehaviour {

        private Camera cam;
        private GameObject camParent;
        private Transform target; // The camera will follow this target

        private float smoothTime = 0.15f; // How much to smooth camera movement (in seconds)
        private float cameraHeight = 1.3f;
        private Vector3 velocity = Vector3.zero; // Starting velocity for SmoothDamp

        private float zoom; // Start zoom for method
        private float zoomTarget; // End zoom for method
        private float zoomStartTime; // Reference of time when method starts
		private float zoomDuration = 0.15f; // How long a zoom lasts (in seconds)
        private float zoomMod = 1.5f;  // Dampen user input
        private float minZoom = 2f; // Minimum the player can zoom (in Ortho camera size)
        private float maxZoom = 15f; // Maximum the player can zoom (in Ortho camera size)
        private float zoomDefault = 5f; // Starting Zoom level (in Ortho camera size)
        private bool isZooming;

        private Quaternion currentRotation; // Start rotation for method
        private Quaternion rotationTarget; // End rotation for method
        private float rotationDuration = 0.15f; // How long a rotation lasts (in seconds)
        private float rotationStartTime; // Reference of time when method starts
		private float rotationTargetAngle;
        private bool isRotating;

        void Awake() {
            cam = gameObject.GetComponent<Camera>();
            camParent = cam.transform.parent.gameObject;
            zoom = zoomTarget = cam.orthographicSize = zoomDefault;
            target = GameObject.FindWithTag("Player").transform;
        }

        void Update() {
            if (target != null) {
                Vector3 goalPos = new Vector3(target.position.x, cameraHeight, target.position.z);
                camParent.transform.position = Vector3.SmoothDamp(camParent.transform.position, goalPos, ref velocity, smoothTime);

                if (isZooming) {
                    float fracZoom = (Time.time - zoomStartTime) / zoomDuration;
                    if (fracZoom <= 1) {
                        cam.orthographicSize = Mathf.Lerp(zoom, zoomTarget, fracZoom);
                    } else {
                        cam.orthographicSize = zoomTarget;
                        isZooming = false;
                    }
                }
                if (isRotating) {
                    float fracRotation = (Time.time - rotationStartTime) / rotationDuration;
                    if (fracRotation <= 1) {
                        camParent.transform.rotation = Quaternion.Slerp(currentRotation, rotationTarget, fracRotation);
                    } else {
                        camParent.transform.rotation = rotationTarget;
                        isRotating = false;
                    }
                }
            }
        }

        public void Zoom(float zoomChange) { // Ease-in/Ease-out Zoom

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

        public void Rotate(float rotationChange) {

            currentRotation = camParent.transform.rotation;
			rotationTargetAngle += rotationChange;
            rotationTarget = Quaternion.Euler(new Vector3(0, rotationTargetAngle, 0));

            rotationStartTime = Time.time;
            isRotating = true;
        }
    }
}