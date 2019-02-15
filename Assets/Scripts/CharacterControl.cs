using System.Collections;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

	public float speed = 5.0f;
	public float inAirSpeed = 4.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 40.0f;
	public Vector3 unitSpeed;
	public float magnitude;
	public GameObject towerPrefab;

	private float onMeshThreshold = 3;
	private float onVertMeshThreshold = 1;
	private CharacterController controller;
	private Build playerBuildComponent;
	private BoxCollider playerCollider;
	private Vector3 moveDirection = Vector3.zero;

	void Awake() {
		SetInitialReferences();
	}

	void SetInitialReferences() {
		controller = GetComponent<CharacterController>();
		playerBuildComponent = GetComponent<Build>();
	}

	void Update() {
		if (controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);

			if(moveDirection.magnitude >= 1) { // Don't normalize if below 1, to allow for slow movement.
				moveDirection.Normalize(); // Normalize values so that diagonal movement is as fast as regular.
			}

			moveDirection *= speed;
			
			if (Input.GetButton("Jump")) {
				moveDirection.y = jumpSpeed;
			}

			if (Input.GetButtonDown("Fire1")) {
				if(playerBuildComponent != null && towerPrefab != null) {

					Vector3 playerFeetLocation = transform.position - new Vector3(0, transform.localScale.y / 2, 0);
					if(IsAgentOnNavMesh(transform.gameObject)) {
						playerBuildComponent.BuildStructure(playerFeetLocation, transform.rotation, towerPrefab);
					}

				}
			}
		} else {
			moveDirection.x = (Input.GetAxis("Horizontal") * inAirSpeed);
			moveDirection.z = (Input.GetAxis("Vertical") * inAirSpeed);
		}

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		unitSpeed = controller.velocity;
		magnitude = Vector3.Magnitude(this.transform.position);
	}


	private bool IsAgentOnNavMesh(GameObject agentObject) {
		// https://stackoverflow.com/questions/45416515/check-if-disabled-navmesh-agent-player-is-on-navmesh
		
		Vector3 agentPosition = agentObject.transform.position;
		UnityEngine.AI.NavMeshHit hit;

		// Check for nearest point on navmesh to agent, within onMeshThreshold
		if (UnityEngine.AI.NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, UnityEngine.AI.NavMesh.AllAreas)) {
			// Check if the positions are vertically aligned
			if (Mathf.Approximately(agentPosition.x, hit.position.x)
				&& Mathf.Approximately(agentPosition.z, hit.position.z)) {
				if((agentPosition.y - hit.position.y) <= onVertMeshThreshold) {
					// Lastly, check if object is below navmesh
					return agentPosition.y >= hit.position.y;
				}
			}
		}

		return false;
	}
}