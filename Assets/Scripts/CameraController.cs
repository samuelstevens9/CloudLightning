using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	public GameObject follow;
	public Text scoreText;
	public GameObject gameOver;
	int score = 0;
	Vector3 lastPosition;
	// Use this for initialization
	void Start () {
		lastPosition = follow.transform.position;
		scoreText.text = "Score: "+score.ToString();
	}


	// Update is called once per frame
	void Update () {
		//print ("Follow X: " + follow.transform.position.x + ", Camera X: " + transform.position.x+", Extent X: " + Camera.main.OrthographicBounds().extents.x);
		//print("Camera Bounds: "+Camera.main.OrthographicBounds());
		if (follow) {
			transform.Translate (new Vector3 ((follow.transform.position.x - lastPosition.x), 0, 0));
			lastPosition = follow.transform.position;
		}
	}

	public void EnemyKilled(GameObject enemy){
		score++;

		scoreText.text = "Score: "+score.ToString();
	}

	public void GameOver(){
		gameOver.SetActive (true);
	}

}
