using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public Vector3 spawnLocation;
	public Vector3 spawnOffset;
	public float duration = 0.25f;

	private float startTime;
	private GameObject spawnGO;
	private GameObject entitiesManager;
	private Health healthComponent;
	private Entity entityComponent;


	[SerializeField]
	private bool spawned = false;

	public bool Spawned {
		get { return spawned; }
	}

	void Awake() {

		spawnGO = transform.gameObject;

		healthComponent = spawnGO.GetComponent<Health>();
		entitiesManager = GameObject.FindGameObjectsWithTag("EntitiesManager")[0];
		if(entitiesManager != null) {
			if(healthComponent != null) {
				if(healthComponent.Allegiance) {
					spawnGO.transform.parent = entitiesManager.transform.Find("Friendlies").gameObject.transform;
				} else {
					spawnGO.transform.parent = entitiesManager.transform.Find("Enemies").gameObject.transform;
				}
			} else {
				spawnGO.transform.parent = entitiesManager.transform;
			}
		}

		entityComponent = spawnGO.GetComponent<Entity>();

		float spawnY = spawnGO.GetComponent<Collider>().bounds.size.y;
		spawnOffset = transform.position;
		spawnLocation = transform.position + new Vector3(0, spawnY, 0);

	}

	void Start() {
		startTime = Time.time;
	}

	void Update () {
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
		} else {
			Destroy(this);
		}
	}
}
