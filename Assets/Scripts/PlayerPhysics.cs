using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {

	public LayerMask collisionMask;
	[HideInInspector]
	public bool grounded;

	private BoxCollider boxCollider;
	private Vector3 s;
	private Vector3	c;

	private float skin = 0.005f; //distance padding between player and floor

	Ray ray;
	RaycastHit hit; 

	public void Move(Vector2 moveAmount){
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x; 
		Vector2 p = transform.position;
		grounded = false;
		//raycasting to detect ground
		for (int i = 0; i < 3; i++) {
			float dir = Mathf.Sign (deltaY);
			float x = (p.x + c.x - s.x / 2) + s.x / 2 * i;//left center and the right point of collider
			float y = p.y + c.y + s.y/2 * dir;

			ray = new Ray (new Vector2 (x, y), new Vector2(0,dir));
			Debug.DrawRay (ray.origin, ray.direction);
			//stop player from going down
			if (Physics.Raycast (ray, out hit, Mathf.Abs(deltaY), collisionMask)) {
				//Get distance between player and floor
				float dst = Vector3.Distance(ray.origin,hit.point);

				if (dst > skin) {
					deltaY = dst * dir + skin;
				} else {
					deltaY = 0;
				}
				grounded = true;
				break;
			}
		}

		//raycasting to detect ground
		for (int i = 0; i < 3; i++) {
			float dir = Mathf.Sign (deltaX);
			float y = (p.y + c.y - s.y / 2) + s.y / 2 * i;//left center and the right point of collider
			float x = p.x + c.x + s.x/2 * dir;

			ray = new Ray (new Vector2 (x, y), new Vector2(dir,0));
			Debug.DrawRay (ray.origin, ray.direction);
			//stop player from going down
			if (Physics.Raycast (ray, out hit, Mathf.Abs(deltaX), collisionMask)) {
				//Get distance between player and floor
				float dst = Vector3.Distance(ray.origin,hit.point);

				if (dst > skin) {
					deltaX = dst * dir + skin;
				} else {
					deltaX = 0;
				}
				//grounded = true;
				break;
			}
		}

		Vector2 finalTransform = new Vector2 (deltaX, deltaY);

		transform.Translate (finalTransform);
	}
	// Use this for initialization
	void Start () {
		boxCollider = GetComponent<BoxCollider> ();
		s = boxCollider.size;
		c = boxCollider.center; 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
