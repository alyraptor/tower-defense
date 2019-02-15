using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour {

	public void BuildStructure(Vector3 spawnLocation, Quaternion spawnRotation, GameObject spawnPrefab) {

		float spawnY = spawnPrefab.GetComponent<BoxCollider>().size.y;
		Vector3 spawnOffset = spawnLocation - new Vector3(0, spawnY / 2, 0);
		spawnLocation = spawnLocation + new Vector3(0, spawnY / 2, 0);

		GameObject structureGO = (GameObject)Instantiate(spawnPrefab, spawnOffset, spawnRotation);

	}

}
