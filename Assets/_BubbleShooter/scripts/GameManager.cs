using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour 
{
	public const int MAX_ROW = 11;
	public const int MAX_COLUMN = 13;



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
		
}
