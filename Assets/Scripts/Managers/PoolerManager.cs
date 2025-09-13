using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolerManager : MonoBehaviour
{
	public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
	
	public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
	{
		// Find the pool with the same name as the object to be spawned
		PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
		
		if (pool == null) // If the pool doesnt exist, create it
		{
			pool = new PooledObjectInfo(objectToSpawn.name);
			ObjectPools.Add(pool);
		}
		
		// Check if there's any inactive object in the pool
		GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();
		
		if (spawnableObj == null)
		{
			// Create one if no inactive object in pool
			spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
		} else 
		{
			// If exist, set the position and rotation of object to the desired spot
			spawnableObj.transform.position = spawnPosition;
			spawnableObj.transform.rotation = spawnRotation;
			
			// Also removes the object from the inactive list, and set it active
			pool.InactiveObjects.Remove(spawnableObj);
			spawnableObj.SetActive(true);
		}
		
		return spawnableObj;
	}
	
	public static void ReturnObjectToPool(GameObject obj)
	{
		string goName = obj.name.Substring(0, obj.name.Length - 7); // Removes the (Clone) string from the objects name
		
		PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
		
		if (pool == null)
		{
			Debug.Log("Trying to return object thats not been pooled");
		} else 
		{
			// Set the object inactive and add it back to the pool list
			obj.SetActive(false);
			pool.InactiveObjects.Add(obj);
		}
	}
	
	public class PooledObjectInfo
	{
		public string LookupString { get; private set; }
		public List<GameObject> InactiveObjects = new List<GameObject>();
		
		public PooledObjectInfo(string lookupString)
		{
			LookupString = lookupString;
		}
	}
}
