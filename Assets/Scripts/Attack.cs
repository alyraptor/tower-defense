using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	private LayerMask enemyLayer = 1 << 9; // Bit shift
	private LayerMask friendlyLayer = 1 << 8;

	private LayerMask detectionLayer;
	private Collider[] hitColliders;

	public List<GameObject> FindTargets(bool friendly, int range) { // Get a list of targets sorted by range (nearest to farthest)
		List<GameObject> targets = new List<GameObject>();

		if(friendly) {
			detectionLayer = enemyLayer;
		} else {
			detectionLayer = friendlyLayer;
		}

		hitColliders = Physics.OverlapSphere(this.transform.position, range, detectionLayer);
		for(int i = 0; i < hitColliders.Length; i++) {
			Health health = hitColliders[i].transform.GetComponent<Health>();
			if(health != null) {
				targets.Add(hitColliders[i].gameObject);
			}
		}

		targets.Sort(delegate(GameObject c1, GameObject c2) {
			float c1Bias = c1.GetComponent<Health>().TargetBias;
			float c2Bias = c2.GetComponent<Health>().TargetBias;
			if(c2.GetComponent<Health>().Allegiance) {
				Debug.Log(this + ".c1: " + c1.transform.position + ", .c2: " + c2.transform.position + ", diff: " + (c1.transform.position - c2.transform.position));
				// Debug.Log(this + "vs." + c1 + ".c1Bias " + (Vector3.SqrMagnitude(this.transform.position) - Vector3.SqrMagnitude(c1.transform.position) + c1Bias));
				// Debug.Log(this + "vs." + c1 + ".c1 " + (Vector3.SqrMagnitude(this.transform.position) - Vector3.SqrMagnitude(c1.transform.position)));
				// Debug.Log(this + "vs." + c2 + ".c2Bias " + (Vector3.SqrMagnitude(this.transform.position) - Vector3.SqrMagnitude(c2.transform.position) + c2Bias));
				// Debug.Log(this + "vs." + c2 + ".c2 " + (Vector3.SqrMagnitude(this.transform.position) - Vector3.SqrMagnitude(c2.transform.position)));
			}

			return (-1) * (Vector3.Distance(this.transform.position, c2.transform.position) - (c2Bias / 3)).CompareTo
			(Vector3.Distance(this.transform.position, c1.transform.position) - (c1Bias / 3));
		});

		return targets;
	}

	public void Shoot(GameObject target, GameObject projectilePrefab) {
		Vector3 offset = new Vector3(0, 0, 0);
		Shoot(target, projectilePrefab, offset);
	}

	public void Shoot(GameObject target, GameObject projectilePrefab, Vector3 spawnOffset) {

		GameObject projectileGO = (GameObject)Instantiate(projectilePrefab, transform.position + spawnOffset, transform.rotation);

		Projectile proj = projectileGO.GetComponent<Projectile>();
		proj.Target = target;
	}

	public void Melee(GameObject weapon, GameObject target) {
		
		// TODO: Perform melee animation
		// TODO: Check hit
		// TODO: Customize damage output

		DoDamage(10, target);
	}

	public void DoDamage(float amount, GameObject target) {

		Health targetHealth = target.GetComponent<Health>();
		targetHealth.TakeDamage(amount);
	}
}