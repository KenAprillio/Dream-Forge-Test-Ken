using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectCatcher : MonoBehaviour
{
	PlayerHealth playerHealth;
	GameManager gameManager;

	void Start()
	{
		playerHealth = GetComponent<PlayerHealth>();
		gameManager = GameManager.Instance;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (!gameManager.GameInProgress) return;
		
		if (collision.TryGetComponent(out Object obj))
		{
			ProcessObject(obj);
		}
	}
	
	private void ProcessObject(Object obj)
	{
		switch (obj.Type)
		{
			case ObjectType.Score:
				gameManager.ProcessScore();
				break;
			case ObjectType.Heal:
				playerHealth.IncreaseHealth();
				break;
			case ObjectType.Damage:
				playerHealth.DecreaseHealth();
				break;
		}
		
		PoolerManager.ReturnObjectToPool(obj.gameObject);
	}
	
}
