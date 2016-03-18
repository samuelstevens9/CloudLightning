using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	// Player Handling
	public float gravity = 1;
	public float speed = 20;
	public float acceloration = 1.5f;
	public float jumpHeight = 0.5f;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	private PlayerPhysics playerPhysics;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent <PlayerPhysics>();
	}
	
	// Update is called once per frame
	void Update () {
		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceloration);

		if (playerPhysics.grounded) {
			amountToMove.y = 0;
			if (Input.GetButtonDown ("Jump")) {
				amountToMove.y = jumpHeight;
			}
		}

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move (amountToMove);
	}

	private float IncrementTowards(float n, float target, float a){
		if (n == target) {
			return n;
		} else {
			float dir = Mathf.Sign (target - n);
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target - n)) ? n : target; 
		}
	}
}
