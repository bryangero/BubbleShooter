using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour 
{
	public int score = 0;
	public Bubble[][] bubbles;
	public const int MAX_ROW = 11;
	public const int MAX_COLUMN = 13;
	private bool isGameOver = false;

	public delegate void UpdateScoreDG(int score);
	public event UpdateScoreDG UpdateScoreEvent;

	public void SubscribeToUpdateScoreEvent(UpdateScoreDG updateScoreDG)
	{
		if (UpdateScoreEvent == null || !GameOverEvent.GetInvocationList().Contains(updateScoreDG))
			UpdateScoreEvent += updateScoreDG;
	}

	public void UnsubscribeToUpdateScoreEvent(UpdateScoreDG updateScoreDG) 
	{
		if (UpdateScoreEvent.GetInvocationList().Contains(updateScoreDG)) 
			UpdateScoreEvent -= updateScoreDG;
	}

	public void CallUpdateScoreEvent(int score) 
	{
		if (UpdateScoreEvent != null) 
			UpdateScoreEvent(score);
	}

	public delegate void GameOverDG();
	public event GameOverDG GameOverEvent;

	public void SubscribeToGameOverEvent(GameOverDG gameOverDG)
	{
		if (GameOverEvent == null || !GameOverEvent.GetInvocationList().Contains(gameOverDG))
			GameOverEvent += gameOverDG;
	}

	public void UnsubscribeToGameOverEvent(GameOverDG gameOverDG) 
	{
		if (GameOverEvent.GetInvocationList().Contains(gameOverDG)) 
			GameOverEvent -= gameOverDG;
	}

	public void CallGameOverEvent() 
	{
		if (GameOverEvent != null) 
			GameOverEvent();
	}

	public delegate void PopBubbleDG(int score);
	public event PopBubbleDG PopBubbleEvent;

	public delegate void CancelPopBubbleDG();
	public event CancelPopBubbleDG CancelPopBubbleEvent;

	public void SubscribeToPopBubbleEvent(PopBubbleDG popBubbleDG, CancelPopBubbleDG cancelPopBubbleDG)
	{
		if (PopBubbleEvent == null || !PopBubbleEvent.GetInvocationList().Contains(popBubbleDG))
			PopBubbleEvent += popBubbleDG;
		
		if (CancelPopBubbleEvent == null || !CancelPopBubbleEvent.GetInvocationList().Contains(popBubbleDG))
			CancelPopBubbleEvent += cancelPopBubbleDG;
	}

	public void UnsubscribeToPopBubbleEvent(PopBubbleDG popBubbleDG) 
	{
		if (PopBubbleEvent.GetInvocationList().Contains(popBubbleDG)) 
			PopBubbleEvent -= popBubbleDG;
	}

	public void CallPopBubbleEvent() 
	{
		if (PopBubbleEvent != null && PopBubbleEvent.GetInvocationList().Length >= 3) 
		{
			int scorePerBubble = 5 * PopBubbleEvent.GetInvocationList().Length;
			int totalScore = scorePerBubble * PopBubbleEvent.GetInvocationList().Length;
			score += totalScore;
			PopBubbleEvent(scorePerBubble);
			UpdateScoreEvent(score);
		}
		else if (CancelPopBubbleEvent != null) 
			CancelPopBubbleEvent();
		
		PopBubbleEvent = null;
		CancelPopBubbleEvent = null;
	}

	private void Awake() 
	{
		bubbles = new Bubble[MAX_ROW][];
		for (int i = 0; i < bubbles.Length; i++) 
			bubbles[i] = new Bubble[MAX_COLUMN];
		SubscribeToGameOverEvent(OnGameOver);
	}

	private void Update() 
	{
		if (isGameOver == false)
			return;

		if(Input.GetMouseButtonUp(0))
			SceneManager.LoadScene("Main");
	}

	private void OnGameOver() 
	{
		isGameOver = true;
	}

	public IEnumerator RunThroughBubbleMatrix(Color theColor, int rowStart, int colStart) 
	{
		for(int row = 0; row < MAX_ROW; row++)
			for (int column = 0; column < MAX_COLUMN; column++) 
				if (bubbles[row][column] != null && row == rowStart && column == colStart)
					yield return StartCoroutine(ValidateNeighbors(theColor, rowStart, colStart));
		CallPopBubbleEvent();
	}
		
	private IEnumerator ValidateNeighbors(Color theColor, int row, int column) 
	{
		bubbles[row][column].ValidateNeighbors(theColor);
		yield return null;
	}


}
