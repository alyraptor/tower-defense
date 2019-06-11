using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense {
    public class Build : MonoBehaviour {

		public GameObject BuildStructure(Vector3 spawnLocation, Quaternion spawnRotation, GameObject spawnPrefab) {

			float spawnY = spawnPrefab.GetComponent<BoxCollider>().size.y;
			Vector3 spawnOffset = spawnLocation - new Vector3(0, spawnY, 0);
			spawnLocation = spawnLocation + new Vector3(0, spawnY, 0);

			return (GameObject)Instantiate(spawnPrefab, spawnOffset, spawnRotation);
		}
	}
}
