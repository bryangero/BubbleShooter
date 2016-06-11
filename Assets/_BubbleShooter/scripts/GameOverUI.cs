using UnityEngine;
using System.Collections;

public class GameOverUI : MonoBehaviour {

	[SerializeField] private GameManager gameManager;
	[SerializeField] private GameObject gameOverText;

	private void Awake() {
		gameManager = GameObject.FindObjectOfType<GameManager>() as GameManager;
		gameManager.SubscribeToGameOverEvent(OnGameOver);
	}

	public void OnGameOver() 
	{
		gameOverText.SetActive(true);
	}


}
