using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	
	[SerializeField] private bool isGameInProgress;
	public bool GameInProgress => isGameInProgress;
	
	[SerializeField] private float maxGameTime;
	public float maxTime => maxGameTime;
	
	public float currentGameTime { get; private set; }
	public float playerScore { get; private set; }
	
	public Action OnGameStarted;
	
	UIManager uiManager;
	

	void Awake()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);
		else Instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{
		currentGameTime = maxGameTime;
		uiManager = UIManager.Instance;
		
		uiManager.UpdateScore(0);
	}

	// Update is called once per frame
	void Update()
	{
		if (!isGameInProgress) return; // Stops the timer if the game is not in progress
		
		currentGameTime -= Time.deltaTime;
		uiManager.UpdateTimer(currentGameTime);
		
		// When the timer hits zero, the game ends
		if (currentGameTime <= 0f)
		{
			Debug.Log("Timer ended!");
			isGameInProgress = false;
			
			EndGame(true);
		}
	}
	
	public void StartGame()
	{
		// Sets game in progress and reset all progress 
		isGameInProgress = true;
		currentGameTime = maxGameTime;
		playerScore = 0;
		uiManager.UpdateScore(playerScore);
		
		OnGameStarted?.Invoke();
	}
	
	public void EndGame(bool win)
	{
		if (!win) isGameInProgress = false;
		
		uiManager.ShowResultScreen(win, playerScore);
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
	
	public void ProcessScore()
	{
		playerScore += 100;
		uiManager.UpdateScore(playerScore);
	}
}
