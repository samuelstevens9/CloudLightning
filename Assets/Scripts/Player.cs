using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	public GameObject bolt;
	public int lifeAmount = 5;
	public float jumpHeight = 4;
	public float timeToJumpApex = 0.4f;
	public float moveSpeed = 6;
	float accelorationTimeAirborne = 0.2f;
	float accelorationTimeGrounded = 0.1f;
	float gravity;// = -20;
	float jumpVelocity;// = 8;
	Vector3 velocity;
	float velocitySmoothing;
	Controller2D controller;
	float direction = 1;
	GameObject clouds;
	MeshRenderer cloudRenderer;
	private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.
	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D> ();
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		print ("Gravity:" + gravity + "  Jump Velo:" + jumpVelocity);
		clouds = transform.FindChild ("Clouds").gameObject;
		cloudRenderer = clouds.GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (controller.enemyCollisions.Collided () || controller.enemyBoltCollisions.Collided ()) {
			print ("Enemy collided.");
			lifeAmount--;

			clouds.transform.localScale = new Vector3 (clouds.transform.localScale.x - 0.02f, 1f, 1f);
			cloudRenderer.material.color = Color.red;
			if (clouds.transform.localScale.x < 0) {
				Destroy (gameObject);
				Camera.main.SendMessage ("GameOver");
			}
		} else {
			cloudRenderer.material.color = Color.white;
		}

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		//Check if we are running either in the Unity editor or in a standalone build.
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")); //1 or -1
		bool fire = Input.GetButtonDown ("Fire1");

		int horizontal = 0;     //Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.
		#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		//Check if Input has registered more than zero touches
		if (Input.touchCount > 0)
		{
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];
			//fire = (fire && myTouch.phase == TouchPhase.Ended);
			//Left or right?
			if(touchOrigin.x < Screen.width/2){
				input.x = -1;
			}else{
				input.x = 1;
			}

			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began)
			{
				//If so, set touchOrigin to the position of that touch
				touchOrigin = myTouch.position;
			}

			//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
			else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
			{
				//Set touchEnd to equal the position of this touch
				Vector2 touchEnd = myTouch.position;

				//Calculate the difference between the beginning and end of the touch on the x axis.
				float tx = touchEnd.x - touchOrigin.x;

				//Calculate the difference between the beginning and end of the touch on the y axis.
				float ty = touchEnd.y - touchOrigin.y;

				//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
				touchOrigin.x = -1;

				//Check if the difference along the x axis is greater than the difference along the y axis.
				if(Mathf.Abs(tx) > 10 || Mathf.Abs(ty) > 10)
				{
					if (Mathf.Abs(tx) > Mathf.Abs(ty))
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = tx > 0 ? 1 : -1;
					else
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = ty > 0 ? 1 : -1;
				}
			}
		}



		#endif

		if ((vertical == 1 || Input.GetButtonDown ("Jump")) && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}

		float targetVelocityX = input.x * moveSpeed;
		if (input.x != 0) {
			direction = Mathf.Sign (velocity.x);
		}
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocitySmoothing, (controller.collisions.below) ? accelorationTimeGrounded : accelorationTimeAirborne);

		transform.localScale = new Vector3 (direction, 1f, 1f);

		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);

		if (fire) {
			GameObject b = Instantiate (bolt, transform.position, transform.rotation) as GameObject;
			b.SendMessage ("Fire", Mathf.Sign (direction));
		}
	}
}
