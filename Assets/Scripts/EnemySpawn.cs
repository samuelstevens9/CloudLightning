using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {

	public GameObject enemy;
	public int maxEnemies = 5;
	public float spawnRateSeconds = 3f; 
	int enemyCount = 0;
	float ellapsedTime = 0;
	ArrayList enemies;
	// Use this for initialization
	void Start () {
		//enemies = new ArrayList ();
		Bounds camBounds = Camera.main.OrthographicBounds ();
		print (camBounds);
		//enemies.Add (e1);
	}
	public int GetEnemyCount(){
		return enemyCount;
	}

	public void EnemyKilled(GameObject eny){
		enemyCount--;
	}
	public void SpawnEnemy(){
		if (enemyCount > maxEnemies)
			return;
		Bounds camBounds = Camera.main.OrthographicBounds ();

		Vector2 ePos = new Vector3 (Random.Range(camBounds.center.x-camBounds.extents.x,camBounds.center.x+camBounds.extents.x), camBounds.extents.y );
		//GameObject e1 = Instantiate (enemy, ePos, Quaternion.identity) as GameObject;
		Instantiate (enemy, ePos, Quaternion.identity);
		enemyCount++;
	}

	// Update is called once per frame
	void Update () {
		ellapsedTime += Time.deltaTime;
		if (ellapsedTime > spawnRateSeconds) {
			ellapsedTime = 0;
			SpawnEnemy ();	
		}
	}
}
