using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Enemy : MonoBehaviour {

	public GameObject bolt;

	public float jumpHeight = 4;
	public float timeToJumpApex = 0.4f;
	public float minMoveSpeed = 10;
	public float maxMoveSpeed = 16;
	float moveSpeed = 6;
	float accelorationTimeAirborne = 0.2f;
	float accelorationTimeGrounded = 0.1f;
	float gravity;// = -20;
	float jumpVelocity;// = 8;
	public float fireRateSeconds = 1f;
	float ellapsedTime = 0;
	Vector3 velocity;
	Vector2 enemyMove;
	float velocitySmoothing;
	Controller2D controller;
	float direction = 1;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D> ();
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		print ("Gravity:" + gravity + "  Jump Velo:" + jumpVelocity);
		enemyMove = new Vector2 (0.25f, 0);
		moveSpeed = Random.Range (minMoveSpeed, maxMoveSpeed);//* Random.value;
	}

	// Update is called once per frame
	void Update () {
		
		if (controller.boltCollisions.Collided ()) {
			Camera.main.SendMessage ("EnemyKilled", gameObject);
			Destroy (gameObject);
			return;
		}

		FindPlayer ();

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}



		if ((controller.collisions.left || controller.collisions.right) && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}

		float targetVelocityX = enemyMove.x * moveSpeed;
		if (enemyMove.x != 0) {
			direction = Mathf.Sign (velocity.x);
		}
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocitySmoothing, (controller.collisions.below) ? accelorationTimeGrounded : accelorationTimeAirborne);

		transform.localScale = new Vector3 (direction, 1f, 1f);

		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);

		ellapsedTime += Time.deltaTime;
		if (ellapsedTime > fireRateSeconds) {
			ellapsedTime = 0;
			GameObject b = Instantiate (bolt, transform.position, transform.rotation) as GameObject;
			b.SendMessage ("Fire", Mathf.Sign (direction));
		}

	}

	void FindPlayer(){
		Vector2 rayOrigin = new Vector2 (transform.position.x, transform.position.y);//controller.raycastOrigins.middleRight;
		//print ("Pos:" + transform.position + ", RayO:" + rayOrigin);
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right,100,controller.playerMask);

		Debug.DrawRay (rayOrigin, Vector2.right * 100, Color.red);

		if (hit && enemyMove.x < 0) {
			print ("Player to the right");
			enemyMove.x *= -1;
		}

		//rayOrigin = controller.raycastOrigins.middleLeft;
		hit = Physics2D.Raycast (rayOrigin, Vector2.left,100,controller.playerMask);

		Debug.DrawRay (rayOrigin, Vector2.left * 100, Color.red);

		if (hit && enemyMove.x > 0) {
			print ("Player to the left");
			enemyMove.x *= -1;
		}
	}
}
