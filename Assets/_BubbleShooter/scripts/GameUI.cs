using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour 
{
	[SerializeField] private GameManager gameManager;
	[SerializeField] private Text endGameLabel;
	[SerializeField] private Text scoreLabel;

	private void Awake() 
	{
		gameManager = GameObject.FindObjectOfType<GameManager>() as GameManager;
		gameManager.SubscribeToEndGameEvent(OnEndGame);
		gameManager.SubscribeToUpdateScoreEvent(OnUpdateScore);
	}

	public void OnUpdateScore(int score) 
	{
		scoreLabel.text = score.ToString();
	}
		
	public void OnEndGame(bool isWin) 
	{
		endGameLabel.gameObject.SetActive(true);
		if (isWin)
			endGameLabel.text = "You Win!";
		else
			endGameLabel.text = "Game Over!";
	}
}
