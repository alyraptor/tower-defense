using System.Collections;
using UnityEngine;

namespace TowerDefense {
	public class PlayerManager : MonoBehaviour {

        public enum ControlStyle { NW, NE, N };
        public ControlStyle controlStyle;

		public float speed = 5.0f;
		public float inAirSpeed = 4.0f;
		public float jumpSpeed = 8.0f;
		public float gravity = 40.0f;
		public Vector3 unitSpeed;
		public float magnitude;
		public GameObject towerPrefab;

		private float onMeshThreshold = 3f;
		private float onVertMeshThreshold = 0.5f;
		private Vector3 moveDirection = Vector3.zero;
        private float directionMod;
        private Quaternion playerRotation;

		private CharacterController controller;
		private Build playerBuildComponent;
		private BoxCollider playerCollider;
		private Spawn buildingSpawn;
		private CameraManager cameraManager;
		private bool isBuilding = false;

		void Awake() {
			SetInitialReferences();
		}

		void SetInitialReferences() {
			controller = GetComponent<CharacterController>();
			playerBuildComponent = GetComponent<Build>();
			cameraManager = Camera.main.GetComponent<CameraManager>();
		}

		void Update() {
            PlayerMove();
            PlayerBuild();
            PlayerCamera();
		}

		private void PlayerMove() {

            if (controller.isGrounded) {

                if(controlStyle == ControlStyle.NE) {
                    directionMod = 90;
                } else if(controlStyle == ControlStyle.N) {
                    directionMod = 45;
                } else {
                    directionMod = 0;
                }

                directionMod += (float)cameraManager.CameraDirection;

                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = Quaternion.Euler(new Vector3(0, directionMod, 0)) * moveDirection;

                if (moveDirection.magnitude >= 1) { // Don't normalize if below 1, to allow for slow movement.
                    moveDirection.Normalize(); // Normalize values so that diagonal movement is as fast as regular.
                }

                if (moveDirection.magnitude != 0) { // Don't let the object rotate or reset by itself
                    playerRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, 0.1f);
                }

                moveDirection *= speed;

                if (Input.GetButton("Jump")) {
                    moveDirection.y = jumpSpeed;
                }

            } else {
                moveDirection.x = (Input.GetAxis("Horizontal") * inAirSpeed);
                moveDirection.z = (Input.GetAxis("Vertical") * inAirSpeed);
                moveDirection = Quaternion.Euler(new Vector3(0, directionMod, 0)) * moveDirection;
            }

            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
            unitSpeed = controller.velocity;
            magnitude = Vector3.Magnitude(this.transform.position);
		}

		private void PlayerCamera() {
            if (Input.mouseScrollDelta.y != 0f) {
                cameraManager.Zoom(Input.mouseScrollDelta.y);
            }
            if (Input.GetButtonDown("Rotate")) {
                cameraManager.Rotate(90f * Input.GetAxis("Rotate"));
            }
		}

		private void PlayerBuild() {

            if (controller.isGrounded) {
                if (isBuilding) {
                    if (buildingSpawn != null) {
                        if (buildingSpawn.Spawned) {
                            isBuilding = false;
                            buildingSpawn = null;
                        }
                    } else {
                        isBuilding = false;
                    }
                } else {
                    if (Input.GetButtonDown("Fire1")) {
                        if (playerBuildComponent != null && towerPrefab != null) {

                            Vector3 playerFeetLocation = transform.position - new Vector3(0, transform.localScale.y / 2, 0);
                            Vector3 roundedLocation = new Vector3(Mathf.Round(playerFeetLocation.x), playerFeetLocation.y, Mathf.Round(playerFeetLocation.z));

                            // If player is close enough to navMesh
                            if (IsVectorOnNavMesh(roundedLocation)) {

                                buildingSpawn = playerBuildComponent.BuildStructure(roundedLocation, transform.rotation, towerPrefab).GetComponent<Spawn>();
                                isBuilding = true;
                            }
                        }
                    }
                }
            }
		}

		private bool IsVectorOnNavMesh(Vector3 agentPosition) {
			// https://stackoverflow.com/questions/45416515/check-if-disabled-navmesh-agent-player-is-on-navmesh

			UnityEngine.AI.NavMeshHit hit;

			// Check for nearest point on navmesh to agent, within onMeshThreshold
			if (UnityEngine.AI.NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, UnityEngine.AI.NavMesh.AllAreas)) {
				// Check if the positions are vertically aligned
				if (Mathf.Approximately(agentPosition.x, hit.position.x)
					&& Mathf.Approximately(agentPosition.z, hit.position.z)) {
					// Check if position is within vertical threshold of mesh
					if((agentPosition.y - hit.position.y) <= onVertMeshThreshold) {
						// Lastly, check if object is below navmesh
						return agentPosition.y > hit.position.y - onVertMeshThreshold;
					}
				}
			}

			return false;
		}
	}
}