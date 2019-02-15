using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	public GameObject projectilePrefab;
	public float range = 10f;
	public Vector3 spawnOffset = new Vector3(0f, .25f, 0f);
	
	private Transform towerTransform;
	private Entity entityComponent;
	private Attack attackComponent;

	private List<GameObject> targets;

	private float fireCooldown = 1f;
	private float fireCooldownLeft = 0f;
	private Vector3 direction;

	[SerializeField]
	private string command;

	public string Command {
		get { return command; }
		set {
			command = value;
		}
	}

	void Start () {
		SetInitialReferences();
	}

	void SetInitialReferences() {
		entityComponent = transform.GetComponent<Entity>();
		attackComponent = transform.GetComponent<Attack>();
	}

	void Update () {
		if(entityComponent.enabled) {
			targets = attackComponent.FindTargets(true, 10);
			if(targets.Count > 0) {

				PointAt(targets[0]);

				fireCooldownLeft -= Time.deltaTime;
				if(fireCooldownLeft <= 0 && direction.magnitude <= range) {
					fireCooldownLeft = fireCooldown;
					Fire(targets[0], projectilePrefab);
				}
			}
		}
	}

	void PointAt(GameObject target) {
		direction = target.transform.position - this.transform.position;
		Quaternion lookRot = Quaternion.LookRotation( direction );
		transform.rotation = Quaternion.Euler( 0, lookRot.eulerAngles.y + 90, 0 );
	}

	void Fire(GameObject target, GameObject projectile) {
		attackComponent.Shoot(target, projectile, spawnOffset);
	}
}