using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour 
{
	public int score = 0;

	public delegate void GameOverDG();
	public event GameOverDG GameOverEvent;

	public Bubble[][] bubbles;

	public const int MAX_ROW = 11;
	public const int MAX_COLUMN = 13;

	private void Awake() 
	{
		bubbles = new Bubble[MAX_ROW][];
		for (int i = 0; i < bubbles.Length; i++) 
			bubbles[i] = new Bubble[MAX_COLUMN];
	}

	public delegate void PopBubbleDG(int score);
	public event PopBubbleDG PopBubbleEvent;

	public delegate void CancelPopBubbleDG();
	public event CancelPopBubbleDG CancelPopBubbleEvent;

	public void SubscribeToPopBubbleEvent(PopBubbleDG popBubbleDG, CancelPopBubbleDG cancelPopBubbleDG)
	{
		if (PopBubbleEvent == null || !PopBubbleEvent.GetInvocationList().Contains (popBubbleDG))
			PopBubbleEvent += popBubbleDG;
		
		if (CancelPopBubbleEvent == null || !CancelPopBubbleEvent.GetInvocationList().Contains (popBubbleDG))
			CancelPopBubbleEvent += cancelPopBubbleDG;
	}

	public void UnsubscribeToPopBubbleEvent(PopBubbleDG popBubbleDG) 
	{
		if (PopBubbleEvent.GetInvocationList().Contains(popBubbleDG)) 
			PopBubbleEvent -= popBubbleDG;
	}

	public void CallPopBubbleEvent() 
	{
		if (PopBubbleEvent != null && PopBubbleEvent.GetInvocationList ().Length >= 3) {
			int scorePerBubble = 5 * PopBubbleEvent.GetInvocationList().Length;
			PopBubbleEvent(scorePerBubble);
			int totalScore = scorePerBubble * PopBubbleEvent.GetInvocationList().Length;
			score += totalScore;
			Debug.Log(score);
		} else if (CancelPopBubbleEvent != null) {
			CancelPopBubbleEvent();
		}
		PopBubbleEvent = null;
		CancelPopBubbleEvent = null;
	}

		
	public IEnumerator ValidateNeighbors(Color theColor, int rowStart, int colStart) 
	{
		for(int row = 0; row < MAX_ROW; row++)
			for (int column = 0; column < MAX_COLUMN; column++) 
				if (bubbles [row] [column] != null && row == rowStart && column == colStart)
					yield return StartCoroutine(ValidateInOrder(theColor,rowStart,colStart));
		CallPopBubbleEvent();
	}
		
	private IEnumerator ValidateInOrder(Color theColor, int row, int column) 
	{
		bubbles[row][column].ValidateNeighbors(theColor);
		yield return null;
	}


}
