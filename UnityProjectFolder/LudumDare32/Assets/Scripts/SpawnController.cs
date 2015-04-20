using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour 
{
	public Transform[] enemies;
	public GameObject[] spawnPoints;
	
	void Start () 
	{
		StartCoroutine("SpawnEnemy");
	}
	
	IEnumerator SpawnEnemy()
	{
		int spawnPos = Random.Range (0,spawnPoints.Length);
		int enemy = Random.Range(0,enemies.Length);
		Instantiate(enemies[enemy],spawnPoints[spawnPos].transform.position,Quaternion.identity);
		
		yield return new WaitForSeconds(3.0f);
		StartCoroutine("SpawnEnemy");
	}
	
	
}
