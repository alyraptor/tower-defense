using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed = 30f;
	public float distance = 5f;
	public float damage = 1f;

	private Vector3 projOrigin;
	private LayerMask friendlyProjLayer = 10;
	private LayerMask enemyProjLayer = 11;
	private Health targetHealth; // Target Health Component
	private Health colliderHealth; // Collider Health Component
	
	private Rigidbody rbody;

	[SerializeField]
	private GameObject target;

	public GameObject Target {
		get { return target; }
		set {
			target = value;
		}
	}

	void SetInitialReferences() {
		targetHealth = target.GetComponent<Health>();
		rbody = GetComponent<Rigidbody>();

		if(targetHealth.Allegiance) { // To avoid collisions with Spawner/Friendly. Also http://answers.unity3d.com/questions/1022551/prevent-a-game-object-collide-against-specific-col.html
			gameObject.layer = enemyProjLayer;
		} else {
			gameObject.layer = friendlyProjLayer;
		}
	}

	void Start() {
		SetInitialReferences();
		rbody.velocity = (target.transform.position - transform.position).normalized * speed;
		projOrigin = transform.position;
	}

	void Update() {
		if(target != null) {
			if(Vector3.Distance(projOrigin, transform.position) >= distance) {
				Die();
			}
		}
	}

	void OnCollisionEnter(Collision col) {
		// Check who to hurt
		colliderHealth = col.gameObject.GetComponent<Health>();
		if(colliderHealth != null) {
			if(colliderHealth.Allegiance == targetHealth.Allegiance) {
				colliderHealth.TakeDamage(damage);
				Die();
			}
		} else { // If the collider has no health, it's environment
			Die();
		}
	}

	void Die() {
		Destroy(gameObject);
	}
}