/**

using UnityEngine;
using System.Collections;

public class ArchiveTarget : MonoBehaviour {

	public string[] FindTargets(bool friendly, string command, string wType) {

		if(friendly) { // True if on the player's team, Find enemies to target
			FindFoes(command, wType);
		} else { // False if on enemy team, Find friendlies to target
			FindFriendlies(command, wType);
		}

		if(targetInfo) {
			return targetInfo;
		} else {
			string[] targetInfo = new string[]
				{ 0, 0 };

			return targetInfo;
		}
	}

	public void FindFoes(string command, string wType) {

		Enemy[] targets = GameObject.FindObjectsOfType<Enemy>();
		Enemy nearestTarget = null;
		float dist = Mathf.Infinity;

		// Get an array of targets and set the one closest to this Gameobject
		for(int i = 0; i < targets.Length; i++) {
			if(friendly) {
				Enemy tar = targets[i];
			} else {
				Tower tar = targets[i];
			}

			float d = Vector3.Distance(this.transform.position, tar.transform.position);
			if(nearestTarget == null || d < dist) {
				nearestTarget = tar;
				dist = d;
			}
		}

		if(targets.Length > 0) {

			if(wType == "ranged") {

				// Set destination and rotation of bullet, and rotation of tower
				Vector3 dir = nearestTarget.transform.position - this.transform.position;

			}
		}

		string[] targetInfo = new string[]
			{ nearestTarget, dir };

		return targetInfo;
	}

	public void FindFriendlies(string command, string wType) {

		Enemy[] targets = GameObject.FindObjectsOfType<Enemy>();
		Enemy nearestTarget = null;
		float dist = Mathf.Infinity;

		
	}
}
				// TODO: Put this in Attack script			
				// fireCooldownLeft -= Time.deltaTime;
				// if(fireCooldownLeft <= 0 && dir.magnitude <= range) {
				// 	fireCooldownLeft = fireCooldown;
				// 	Fire(nearestTarget);
				// }
**/