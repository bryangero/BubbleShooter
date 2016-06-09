using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour 
{

	public delegate void GameOverDG();
	public event GameOverDG GameOverEvent;

	public Bubble[][] bubbles;

	public const int MAX_ROW = 11;
	public const int MAX_COLUMN = 13;



	private void Awake() 
	{
		bubbles = new Bubble[MAX_ROW][];
		for (int i = 0; i < bubbles.Length; i++) 
		{
			bubbles[i] = new Bubble[MAX_COLUMN];
		}
	}

	public delegate void PopBubbleDG(Color color);
	public event PopBubbleDG PopBubbleEvent;

	public void SubscribeToPopBubbleEvent(PopBubbleDG popBubbleDG)
	{
		if (PopBubbleEvent == null || !PopBubbleEvent.GetInvocationList ().Contains (popBubbleDG))
			PopBubbleEvent += popBubbleDG;
	}

	public void UnsubscribeToPopBubbleEvent(PopBubbleDG popBubbleDG) 
	{
		if (PopBubbleEvent.GetInvocationList().Contains(popBubbleDG)) 
			PopBubbleEvent -= popBubbleDG;
	}

	public void CallPopBubbleEvent(Color color) 
	{
		if(PopBubbleEvent != null)
			PopBubbleEvent(color);
	}

	public void OnGameOver() {
		
	}

}
