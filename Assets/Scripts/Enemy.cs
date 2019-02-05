using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int attackRange;
	public GameObject weapon;

	private Attack attackComponent;

	private UnityEngine.AI.NavMeshAgent nav;
	private List<GameObject> targetsList;

	void Awake() {
		SetInitialReferences();
	}

	void Update() {
		FindTarget();
		MoveToTarget(targetsList[0].transform.position);
		if(Vector3.Distance(targetsList[0].transform.position, transform.position) < attackRange) {
			AttackTarget(targetsList[0]);
		}
	}

	void SetInitialReferences() {
		attackComponent = transform.GetComponent<Attack>();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	void FindTarget() {
		targetsList = attackComponent.FindTargets(false, 20);
	}

	void MoveToTarget(Vector3 targetPosition) {
		nav.SetDestination(targetPosition);
	}

	void AttackTarget(GameObject targetGO) {
		attackComponent.Melee(weapon, targetGO);
	}
}