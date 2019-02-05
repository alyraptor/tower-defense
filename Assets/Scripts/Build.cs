using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour {

	public void BuildStructure(Vector3 structureLocation, Quaternion structureRotation, GameObject structurePrefab) {

		float spawnY = structurePrefab.GetComponent<BoxCollider>().size.y;
		Vector3 spawnOffset = new Vector3(0, -spawnY, 0);

		GameObject structureGO = SpawnStructure(structureLocation, structureRotation, spawnOffset, structurePrefab);
		BuildAnimation(structureLocation, spawnOffset, structureGO);

	}

	private GameObject SpawnStructure(Vector3 structureLocation, Quaternion structureRotation, Vector3 spawnOffset, GameObject structurePrefab) {
		GameObject structureGO = (GameObject)Instantiate(structurePrefab, structureLocation + spawnOffset, structureRotation);
		return structureGO;
	}

	private void BuildAnimation(Vector3 structureLocation, Vector3 spawnOffset, GameObject structureGO) {
		structureGO.transform.position += -spawnOffset * Time.deltaTime;
	}

}
