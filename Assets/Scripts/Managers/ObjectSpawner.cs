using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
	[Tooltip("X for min time, Y for max time")]
	[SerializeField] private Vector2 objectSpawnTimeRange;
	private float objectCurrentSpawnTime;
	
	[SerializeField] private Collider2D spawnAreaCollider;
	[SerializeField] private List<GameObject> objectsToSpawn = new List<GameObject>();

	void Start()
	{
		GameManager.Instance.OnGameStarted += OnGameStart;
	}
	
	private void OnGameStart()
	{
		if (GameManager.Instance.GameInProgress) // Start spawning objects if game is in progress
		{
			// Set base spawn time to the max time
			objectCurrentSpawnTime = objectSpawnTimeRange.y;
			
			StartCoroutine(SpawnObjectPeriodically());
			StartCoroutine(SetSpawnTime());
			
			Debug.Log("Game Started!");
		}
	}

	private IEnumerator SpawnObjectPeriodically()
	{
		while (GameManager.Instance.GameInProgress)
		{
			// While game is in progress, spawn object every determined interval
			SpawnObject();
			
			yield return new WaitForSeconds(objectCurrentSpawnTime);
		}
	}
	
	private IEnumerator SetSpawnTime()
	{
		while (GameManager.Instance.GameInProgress)
		{
			// Every 10 seconds, speed up the spawning interval
			yield return new WaitForSeconds(10f);
			
			objectCurrentSpawnTime = SetSpawnInterval(objectSpawnTimeRange.y, objectSpawnTimeRange.x, 
							GameManager.Instance.maxTime, GameManager.Instance.currentGameTime);
		}
	}
	
	public void SpawnObject()
	{
		// Determine where it would spawn
		Vector2 spawnPos = GetRandomPointInCollider(spawnAreaCollider);
		
		// Pick the object to spawn
		int index = Random.Range(0, objectsToSpawn.Count);
		GameObject obj = objectsToSpawn[index];
		
		// Spawn the object from the pooler
		PoolerManager.SpawnObject(obj, spawnPos, obj.transform.rotation);
	}
	
	private Vector2 GetRandomPointInCollider(Collider2D collider, float offset = 1f)
	{
		Bounds bounds = collider.bounds;
		
		// Find the minimum and maximum bounds of the collider
		Vector2 minBounds = new Vector2(bounds.min.x + offset, bounds.min.y + offset);
		Vector2 maxBounds = new Vector2(bounds.max.x - offset, bounds.max.y - offset);
		
		// Pick a random number between the bounds and return them
		float randomX = Random.Range(minBounds.x, maxBounds.x);
		float randomY = Random.Range(minBounds.y, maxBounds.y);
		
		return new Vector2(randomX, randomY);
	}
	
	private float SetSpawnInterval(float maxTime, float minTime, float maxGameTime, float currentGameTime)
	{
		// Find which step it is based on current time vs max game time
		int step = Mathf.FloorToInt((maxGameTime - currentGameTime) / 10f);
		
		// Get how many steps it is from max game time divided by 10 seconds
		int totalSteps = Mathf.FloorToInt(maxGameTime / 10f);
		
		// Evenly decrease spawn interval between steps
		float stepSize = (maxTime - minTime) / (totalSteps - 1);
		return Mathf.Max(maxTime - (step * stepSize), minTime);
	}

	void OnDisable()
	{
		GameManager.Instance.OnGameStarted -= OnGameStart;
	}
	
}
