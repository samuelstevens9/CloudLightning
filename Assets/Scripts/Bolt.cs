using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Bolt : MonoBehaviour {

	public float boltSpeed = 18;
	Vector3 velocity;
	float velocitySmoothing;
	Controller2D controller;
	bool fired = false;
	float startX;
	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D> (); 

	}
	
	// Update is called once per frame
	void Update () {
		if (fired) {
			if (controller.enemyCollisions.Collided () || controller.collisions.Collided () 
				|| Mathf.Abs(startX - transform.position.x) > 100 ) {
				print ("Collided so destroy");
				Destroy (gameObject);
			}
			controller.Move (velocity * Time.deltaTime);
		}
	}

	public void Fire(int directionX){
		velocity = new Vector3 (directionX*boltSpeed, 0, 0);
		//print (velocity);//clone.velocity = transform.TransformDirection(Vector3.forward * 10);
		startX = transform.position.x;
		transform.localScale = new Vector3 (-directionX, 1f, 1f);
		fired = true;
	}
}
