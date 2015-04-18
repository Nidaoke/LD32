using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour 
{
	public Transform enemy;
	public GameObject[] spawnPoints;
	
	void Start () 
	{
		StartCoroutine("SpawnEnemy");
	}
	
	IEnumerator SpawnEnemy()
	{
		int spawnPos = Random.Range (0,spawnPoints.Length);
		Instantiate(enemy,spawnPoints[spawnPos].transform.position,Quaternion.identity);
		
		yield return new WaitForSeconds(3.0f);
		StartCoroutine("SpawnEnemy");
	}
	
	
}
