using System.Collections;
using UnityEngine;

namespace TowerDefense {
	public class Health : MonoBehaviour {

		public float maxHealth;

		private float flashTime = 0.05f;
		private Color hitColor = new Color(1f, 0f, 0f, 1f);
		private Color normalColor;
		private MeshRenderer rendy;
		private MeshRenderer[] rendsArray;

		[SerializeField]
		private float currentHealth;

		public float CurrentHealth {
			get { return currentHealth; }
			set {
				currentHealth = value;
			}
		}

		[SerializeField]
		private bool allegiance; // True = Friendly, False = Enemy

		public bool Allegiance {
			get { return allegiance; }
			set {
				allegiance = value;
			}
		}

		[SerializeField]
		private float targetBias;

		public float TargetBias {
			get { return targetBias; }
		}

		void Awake() {
			SetInitialReferences();
		}

		void SetInitialReferences() {
			rendsArray = gameObject.GetComponentsInChildren<MeshRenderer>();
			if (rendsArray.Length > 0) {
				rendy = rendsArray[0];
				normalColor = rendy.material.color;
			}
			currentHealth = maxHealth;
		}

		public void TakeDamage(float damage) {
			currentHealth -= damage;
			if(currentHealth <= 0) {
				Die();
			} else {
				StartCoroutine(Flash());
			}
		}

		public void Die() {
			// Separate out and check whether GO is player
			Destroy(gameObject);
		}

		IEnumerator Flash() {
			rendy.material.color = hitColor;
			yield return new WaitForSeconds(flashTime);
			rendy.material.color = normalColor;
		}
	}
}