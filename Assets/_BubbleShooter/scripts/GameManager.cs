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
	private bool isEndGame = false;

	public delegate void UpdateScoreDG(int score);
	public event UpdateScoreDG UpdateScoreEvent;

	public void SubscribeToUpdateScoreEvent(UpdateScoreDG updateScoreDG)
	{
		if (UpdateScoreEvent == null || !EndGameEvent.GetInvocationList().Contains(updateScoreDG))
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

	public delegate void EndGameDG(bool isWin);
	public event EndGameDG EndGameEvent;

	public void SubscribeToEndGameEvent(EndGameDG endGameDG)
	{
		if (EndGameEvent == null || !EndGameEvent.GetInvocationList().Contains(endGameDG))
			EndGameEvent += endGameDG;
	}

	public void UnsubscribeToEndGameEvent(EndGameDG endGameDG) 
	{
		if (EndGameEvent.GetInvocationList().Contains(endGameDG)) 
			EndGameEvent -= endGameDG;
	}

	public void CallEndGameEvent(bool isWin) 
	{
		if (EndGameEvent != null) 
			EndGameEvent(isWin);
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
		CheckGameFinished();
	}
	

	private void Awake() 
	{
		bubbles = new Bubble[MAX_ROW][];
		for (int i = 0; i < bubbles.Length; i++) 
			bubbles[i] = new Bubble[MAX_COLUMN];
		SubscribeToEndGameEvent(OnEndGame);
	}

	private void Update() 
	{
		if (isEndGame == false)
			return;

		if(Input.GetMouseButtonUp(0))
			SceneManager.LoadScene("Main");
	}

	private void OnEndGame(bool isWin) 
	{
		isEndGame = true;
	}

	public IEnumerator RunThroughBubbleMatrix(Color theColor, int rowStart, int colStart) 
	{
		for(int row = 0; row < MAX_ROW; row++)
			for (int column = 0; column < MAX_COLUMN; column++) 
				if (bubbles[row][column] != null && row == rowStart && column == colStart)
					yield return StartCoroutine(ValidateNeighbors(theColor, rowStart, colStart));
		CallPopBubbleEvent();
	}

	private void CheckGameFinished()
	{
		int nullCtr = 0;
		for (int row = 0; row < MAX_ROW; row++) {
			if (bubbles [row] [0] == null) {
				nullCtr++;
			}
		}
		Debug.Log (nullCtr + " "+MAX_ROW);
		if (nullCtr >= MAX_ROW)
			CallEndGameEvent(true);

	}
		
	private IEnumerator ValidateNeighbors(Color theColor, int row, int column) 
	{
		bubbles[row][column].ValidateNeighbors(theColor);
		yield return null;
	}


}
