using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	public LayerMask collisionMask;
	public LayerMask enemyMask;
	public LayerMask enemyBoltMask;
	public LayerMask boltMask;
	public LayerMask playerMask;

	const float skinWidth = 0.015f;
	public int horizontalRayCount = 3;
	public int veticalRayCount = 3;
	float horizontalRaySpacing;
	float verticalRaySpacing;

	BoxCollider2D boxCollider;
	public RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;
	public CollisionInfo enemyCollisions;
	public CollisionInfo boltCollisions;
	public CollisionInfo enemyBoltCollisions;

	// Use this for initialization
	void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Move(Vector3 velocity){
		UpdateRaycastOrigins ();
		collisions.Reset ();
		enemyCollisions.Reset ();
		boltCollisions.Reset ();
		enemyBoltCollisions.Reset ();

		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		transform.Translate (velocity);
	}

	void HorizontalCollisions(ref Vector3 velocity){ //or return velocity for functional progamming
		float directionX = Mathf.Sign(velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth ;

		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (hit) {
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, enemyMask);
			if (hit) {
				enemyCollisions.left = directionX == -1;
				enemyCollisions.right = directionX == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, boltMask);
			if (hit) {
				boltCollisions.left = directionX == -1;
				boltCollisions.right = directionX == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, enemyBoltMask);
			if (hit) {
				enemyBoltCollisions.left = directionX == -1;
				enemyBoltCollisions.right = directionX == 1;
			}
		}

		directionX *= -1; //check the other side
		rayLength = Mathf.Abs (velocity.x) + skinWidth ;
		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (hit) {
				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, enemyMask);
			if (hit) {
				enemyCollisions.left = directionX == -1;
				enemyCollisions.right = directionX == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, boltMask);
			if (hit) {
				boltCollisions.left = directionX == -1;
				boltCollisions.right = directionX == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, enemyBoltMask);
			if (hit) {
				enemyBoltCollisions.left = directionX == -1;
				enemyBoltCollisions.right = directionX == 1;
			}
		
		}
	}
	void VerticalCollisions(ref Vector3 velocity){ //or return velocity for functional progamming
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth ;

		for (int i = 0; i < veticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY,rayLength,collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, enemyMask);
			if (hit) {
				enemyCollisions.below = directionY == -1;
				enemyCollisions.above = directionY == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, boltMask);
			if (hit) {
				boltCollisions.below = directionY == -1;
				boltCollisions.above = directionY == 1;
			}
		}

		/*
		directionY *= -1;
		rayLength = Mathf.Abs (velocity.y) + skinWidth ;
		for (int i = 0; i < veticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY,rayLength,collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit) {
				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, enemyMask);
			if (hit) {
				enemyCollisions.below = directionY == -1;
				enemyCollisions.above = directionY == 1;
			}

			hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, boltMask);
			if (hit) {
				boltCollisions.below = directionY == -1;
				boltCollisions.above = directionY == 1;
			}
		}
		*/

	}

	public void UpdateRaycastOrigins(){
		Bounds bounds = boxCollider.bounds;
		bounds.Expand (skinWidth * -2); 

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
		raycastOrigins.middleLeft = new Vector2 (bounds.min.x, bounds.max.y/2);
		raycastOrigins.middleRight = new Vector2 (bounds.max.x, bounds.max.y/2);

	}

	void CalculateRaySpacing(){
		Bounds bounds = boxCollider.bounds;
		bounds.Expand (skinWidth * -2); 

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		veticalRayCount = Mathf.Clamp (veticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (veticalRayCount - 1);

	}

	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
		public Vector2 middleLeft, middleRight;
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public void Reset(){
			above = below = false;
			left = right = false;
		}
		public bool Collided(){
			return above || below || left || right;
		}
	}
}
