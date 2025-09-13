using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private int maxHealth;
	
	public int currentHealth { get; private set; }
	
	UIManager uiManager;

	void Start()
	{
		uiManager = UIManager.Instance;
		
		GameManager.Instance.OnGameStarted += OnGameStart;
	}
	
	private void OnGameStart()
	{
		currentHealth = maxHealth;
		uiManager.UpdateHealth(currentHealth);
		
		Debug.Log("Game Started!");
	}
	
	public void DecreaseHealth()
	{
		currentHealth--;
		uiManager.UpdateHealth(currentHealth);
		
		if (currentHealth <= 0)
		{
			GameManager.Instance.EndGame(false);
		}
	}
	
	public void IncreaseHealth()
	{
		if (currentHealth < maxHealth) currentHealth++;
		uiManager.UpdateHealth(currentHealth);
	}

	void OnDisable()
	{
		GameManager.Instance.OnGameStarted -= OnGameStart;
	}
	
}
