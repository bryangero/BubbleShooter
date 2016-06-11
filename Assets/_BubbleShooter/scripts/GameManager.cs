using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour 
{
	public static int bubbleCount;
	public static bool ischeckingbubbles;

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
		PopBubbleEvent = null;
		bubbleCount = 0;
	}

	private IEnumerator ValidateInOrder(Color theColor, int row, int column) {
			int topleftNeighborRow = row;
			int topleftNeighborColumn = column - 1;
			if (topleftNeighborColumn % 2 != 0) 
				topleftNeighborRow--;
			if (topleftNeighborColumn >= 0 && topleftNeighborRow  >= 0) 
			{
				if (bubbles[topleftNeighborRow][topleftNeighborColumn] != null) 
				{
					if (bubbles[topleftNeighborRow][topleftNeighborColumn].isChecked == false &&
						bubbles[topleftNeighborRow][topleftNeighborColumn].bubbleColor == theColor) 
					{
						bubbles[topleftNeighborRow][topleftNeighborColumn].ValidateNeighbors(theColor);
						PopBubbleEvent += bubbles[topleftNeighborRow][topleftNeighborColumn].Pop;
					}
				}
			}
			int topRightNeighborRow = row;
			int topRightNeighborColumn = column - 1;
			if (topRightNeighborColumn % 2 == 0) 
				topRightNeighborRow++;
			if (topRightNeighborRow < GameManager.MAX_ROW && topRightNeighborColumn >= 0) 
			{
				if (bubbles[topRightNeighborRow][topRightNeighborColumn] != null) 
				{
					if (bubbles[topRightNeighborRow][topRightNeighborColumn].isChecked == false &&
						bubbles[topRightNeighborRow][topRightNeighborColumn].bubbleColor == theColor) 
					{
						bubbles[topRightNeighborRow][topRightNeighborColumn].ValidateNeighbors(theColor);
						PopBubbleEvent += bubbles[topRightNeighborRow][topRightNeighborColumn].Pop;
					}
				}
			}

			int leftNeighborRow = row - 1;
			int leftNeighborColumn = column;
			if(leftNeighborRow >= 0)
			{
				if (bubbles[leftNeighborRow][leftNeighborColumn] != null) 
				{
					if (bubbles[leftNeighborRow][leftNeighborColumn].isChecked == false &&
						bubbles[leftNeighborRow][leftNeighborColumn].bubbleColor == theColor) 
					{
						bubbles[leftNeighborRow][leftNeighborColumn].ValidateNeighbors(theColor);
						PopBubbleEvent += bubbles[leftNeighborRow][leftNeighborColumn].Pop;
					}
				}
			}

			int rightNeighborRow = row + 1;
			int rightNeighborColumn = column;
			if (rightNeighborRow < GameManager.MAX_ROW) 
			{
				if (bubbles[rightNeighborRow][rightNeighborColumn] != null)
				{
					if (bubbles[rightNeighborRow][rightNeighborColumn].isChecked == false &&
						bubbles[rightNeighborRow][rightNeighborColumn].bubbleColor == theColor) 
					{
						bubbles[rightNeighborRow][rightNeighborColumn].ValidateNeighbors(theColor);
						PopBubbleEvent += bubbles[rightNeighborRow][rightNeighborColumn].Pop;
					}
				}
			}

			int bottomleftNeighborRow = row;
			int bottomleftNeighborColumn = column + 1;
			if (bottomleftNeighborColumn % 2 != 0) 
				bottomleftNeighborRow--;
			if (bottomleftNeighborColumn >= 0 && bottomleftNeighborRow >= 0) 
			{
				if (bubbles[bottomleftNeighborRow][bottomleftNeighborColumn] != null) 
				{
					if (bubbles[bottomleftNeighborRow][bottomleftNeighborColumn].isChecked == false &&
						bubbles[bottomleftNeighborRow][bottomleftNeighborColumn].bubbleColor == theColor) 
					{
						bubbles[bottomleftNeighborRow][bottomleftNeighborColumn].ValidateNeighbors (theColor);
						PopBubbleEvent += bubbles[bottomleftNeighborRow][bottomleftNeighborColumn].Pop;
					}
				}
			}
			int bottomRightNeighborRow = row;
			int bottomRightNeighborColumn = column + 1;
			if (bottomRightNeighborColumn % 2 == 0)
				bottomRightNeighborRow++;
			if(bottomRightNeighborRow < GameManager.MAX_ROW)
			{
				if (bubbles[bottomRightNeighborRow][bottomRightNeighborColumn] != null) 
				{
					if (bubbles[bottomRightNeighborRow][bottomRightNeighborColumn].isChecked == false &&
						bubbles[bottomRightNeighborRow][bottomRightNeighborColumn].bubbleColor == theColor) 
					{
						bubbles[bottomRightNeighborRow][bottomRightNeighborColumn].ValidateNeighbors(theColor);
						PopBubbleEvent += bubbles[bottomRightNeighborRow][bottomRightNeighborColumn].Pop;
					}
				}
			}
		yield return null;
	}
		
	public IEnumerator ValidateNeighbors(Color theColor, int rowStart, int colStart) 
	{
		for(int row = 0; row < MAX_ROW; row++)
		{
			for (int column = 0; column < MAX_COLUMN; column++) 
			{
				if (bubbles [row] [column] != null && row == rowStart && column == colStart)
					yield return StartCoroutine(ValidateInOrder(theColor,rowStart,colStart));
			}
		}
		CallPopBubbleEvent(Color.white);
	}
		


}
