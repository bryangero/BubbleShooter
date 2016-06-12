using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameUI : MonoBehaviour {

	[SerializeField] private GameManager gameManager;
	[SerializeField] private GameObject gameOverText;

	[SerializeField] private Text scoreLabel;

	private void Awake() 
	{
		gameManager = GameObject.FindObjectOfType<GameManager>() as GameManager;
		gameManager.SubscribeToGameOverEvent(OnGameOver);
		gameManager.SubscribeToUpdateScoreEvent(OnUpdateScore);
	}

	public void OnUpdateScore(int score) 
	{
		scoreLabel.text = score.ToString();
	}
		
	public void OnGameOver() 
	{
		gameOverText.SetActive(true);
	}


}
