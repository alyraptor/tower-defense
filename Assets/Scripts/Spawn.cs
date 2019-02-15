using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
	
	public Vector3 spawnLocation;
	public Vector3 spawnOffset;

	private GameObject spawnGO;
	private Entity entityComponent;
	public bool spawned = false;

	private float duration = 0.25f;

	private float startTime;
	private float journeyLength;

	void Awake() {

		spawnGO = transform.gameObject;
		entityComponent = spawnGO.GetComponent<Entity>();

		float spawnY = spawnGO.GetComponent<BoxCollider>().size.y;
		spawnOffset = transform.position;
		spawnLocation = transform.position + new Vector3(0, spawnY, 0);
		
	}

	void Start() {
		startTime = Time.time;
		journeyLength = Vector3.Distance(spawnOffset, spawnLocation);
	}

	void FixedUpdate () {
		if(!spawned) {
			if(spawnGO != null) {

				float fracJourney = (Time.time - startTime) / duration;
				
				if (fracJourney <= 1) {
					transform.position = new Vector3(spawnLocation.x, Mathf.SmoothStep(spawnOffset.y, spawnLocation.y, fracJourney), spawnLocation.z);
				} else {
					transform.position = spawnLocation;
					entityComponent.enabled = true;
					spawned = true;
				}
			}
		}
	}
}
