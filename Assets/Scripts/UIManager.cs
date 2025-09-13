using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	
	[Header("UI References")]
	[SerializeField] private List<GameObject> healths = new List<GameObject>();
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private TMP_Text timerText;
	
	[Header("End Screen References")]
	[SerializeField] private GameObject endScreen;
	[SerializeField] private TMP_Text winText;
	[SerializeField] private TMP_Text finalScoreText;

	void Awake()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);
		else Instance = this;
	}
	
	public void ShowResultScreen(bool win, float finalScore)
	{
		endScreen.SetActive(true);
		
		if (win) winText.text = "You Win!";
		else winText.text = "You Lose!";
		
		finalScoreText.text = "Your Score: " + finalScore;
	}
	
	public void UpdateHealth(int currentHealth)
	{
		// Set all health inactive before setting a few active according to current health
		foreach (var health in healths) health.SetActive(false);
		
		for (int i = 0; i < currentHealth; i++) healths[i].SetActive(true);
	}
	
	public void UpdateScore(float currentScore)
	{
		scoreText.text = currentScore.ToString();
	}
	
	public void UpdateTimer(float currentTime)
	{
		timerText.text = currentTime.ToString("0");
	}
}
