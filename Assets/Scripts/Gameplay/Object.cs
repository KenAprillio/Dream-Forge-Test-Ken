using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
	[SerializeField] private float despawnTime;
	[SerializeField] private ObjectType type;
	public ObjectType Type => type;

	private IEnumerator DeSpawn()
	{
		yield return new WaitForSeconds(despawnTime);
		
		PoolerManager.ReturnObjectToPool(gameObject);
	}

	void OnEnable()
	{
		StartCoroutine(DeSpawn());
	}

	void OnDisable()
	{
		StopCoroutine(DeSpawn());
	}
}
