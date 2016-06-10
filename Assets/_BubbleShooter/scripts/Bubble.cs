using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour 
{
	
	public delegate void BubbleLandedDG(Color color);
	public event BubbleLandedDG BubbleLandedEvent;
	public HexGrid hexGrid;
	[SerializeField] private GameManager gameManager;
	[SerializeField] private float speed;
	public GameObject mySnapColliders;
	public Collider2D myCollider;
	public Color bubbleColor;
	public bool isMoving;
	public Vector3 direction;
	public Transform topRight;
	public Transform topLeft;
	public Transform BottomRight;
	public Transform BottomLeft;
	public Transform Right;
	public Transform Left;
	public RaycastHit2D[] hits;
	public int row;
	public int column;

	public bool isChecked;

	private void Start() 
	{
		hexGrid = GameObject.FindObjectOfType(typeof(HexGrid)) as HexGrid;
		gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
		Color[] colors = new Color[5] { Color.red, Color.blue, Color.yellow, 
										Color.green, Color.magenta };
//		Color[] colors = new Color[1] { Color.red };
		bubbleColor = colors[Random.Range(0, colors.Length)];
		gameObject.GetComponent<SpriteRenderer>().color = bubbleColor;
		direction = Vector3.zero;
		myCollider.enabled = false;
		mySnapColliders.SetActive (true);
	}

	private void Update() 
	{
		if (direction != Vector3.zero) 
		{
			mySnapColliders.SetActive (false);
			isMoving = true;
			myCollider.enabled = true;
		}
		transform.Translate(direction * (Time.deltaTime*speed));
	}

	public void Pop(Color bubbleColor)
	{
		gameObject.GetComponent<SpriteRenderer>().color = bubbleColor;
		this.bubbleColor = bubbleColor;
//		gameManager.UnsubscribeToPopBubbleEvent(Pop);
		Destroy(gameObject);
	}

	public void ValidateNeighbors(Color theColor) 
	{
		if (isChecked == true)
			return;
		if (bubbleColor != theColor) {
			gameManager.CallPopBubbleEvent (theColor);
			Debug.Log ("IM NOT THE SAME");
			return;
		}
		bool isSameColor = false;
		isChecked = true;
		int TopleftNeighborRow = row;
		int TopleftNeighborColumn = column - 1;
		if (TopleftNeighborColumn % 2 != 0) 
			TopleftNeighborRow--;
		if (TopleftNeighborColumn >= 0 && TopleftNeighborRow  >= 0) 
		{
			if (gameManager.bubbles [TopleftNeighborRow] [TopleftNeighborColumn] != null) 
			{
				if (gameManager.bubbles [TopleftNeighborRow] [TopleftNeighborColumn].bubbleColor == theColor) 
				{
					isSameColor = true;
					gameManager.bubbles [TopleftNeighborRow] [TopleftNeighborColumn].ValidateNeighbors(theColor);
					gameManager.bubbles [TopleftNeighborRow] [TopleftNeighborColumn].Pop(Color.white);
//					Debug.Log(gameManager.bubbles [TopleftNeighborRow] [TopleftNeighborColumn] + " TOP LEFT sameColor");
				}
			}
		}
		int TopRightNeighborRow = row;
		int TopRightNeighborColumn = column - 1;
		if (TopRightNeighborColumn % 2 == 0) 
			TopRightNeighborRow++;
		if (TopRightNeighborRow < GameManager.MAX_ROW && TopRightNeighborColumn >= 0) 
		{
			if (gameManager.bubbles [TopRightNeighborRow] [TopRightNeighborColumn] != null) 
			{
				if (gameManager.bubbles [TopRightNeighborRow] [TopRightNeighborColumn].bubbleColor == theColor) 
				{
					isSameColor = true;
					gameManager.bubbles [TopRightNeighborRow] [TopRightNeighborColumn].ValidateNeighbors(theColor);
					gameManager.bubbles [TopRightNeighborRow] [TopRightNeighborColumn].Pop(Color.white);
//					Debug.Log(gameManager.bubbles [TopRightNeighborRow] [TopRightNeighborColumn] + " TOP RIGHT sameColor");
				}
			}
		}

		int leftNeighborRow = row - 1;
		int leftNeighborColumn = column;
		if(leftNeighborRow >= 0)
		{
			if (gameManager.bubbles [leftNeighborRow] [leftNeighborColumn] != null) 
			{
				if (gameManager.bubbles [leftNeighborRow] [leftNeighborColumn].bubbleColor == theColor) 
				{
					isSameColor = true;
					gameManager.bubbles [leftNeighborRow] [leftNeighborColumn].ValidateNeighbors(theColor);
					gameManager.bubbles [leftNeighborRow] [leftNeighborColumn].Pop(Color.white);
//					Debug.Log(gameManager.bubbles [leftNeighborRow] [leftNeighborColumn].name + " LEFT sameColor");
				}
			}
		}

		int RightNeighborRow = row + 1;
		int RightNeighborColumn = column;
		if (RightNeighborRow < GameManager.MAX_ROW) 
		{
			if (gameManager.bubbles [RightNeighborRow] [RightNeighborColumn] != null)
			{
				if (gameManager.bubbles [RightNeighborRow] [RightNeighborColumn].bubbleColor == theColor) 
				{
					isSameColor = true;
					gameManager.bubbles [RightNeighborRow] [RightNeighborColumn].ValidateNeighbors(theColor);
					gameManager.bubbles [RightNeighborRow] [RightNeighborColumn].Pop(Color.white);
//					Debug.Log(gameManager.bubbles [RightNeighborRow] [RightNeighborColumn] + " RIGHT sameColor");
				}
			}
		}

		int BottomleftNeighborRow = row;
		int BottomleftNeighborColumn = column + 1;
		if (BottomleftNeighborColumn % 2 != 0) 
			BottomleftNeighborRow--;
		if (BottomleftNeighborColumn >= 0 && BottomleftNeighborRow >= 0) 
		{
			if (gameManager.bubbles [BottomleftNeighborRow] [BottomleftNeighborColumn] != null) 
			{
				if (gameManager.bubbles [BottomleftNeighborRow] [BottomleftNeighborColumn].bubbleColor == theColor) 
				{
					isSameColor = true;
					gameManager.bubbles [BottomleftNeighborRow] [BottomleftNeighborColumn].ValidateNeighbors(theColor);
					gameManager.bubbles [BottomleftNeighborRow] [BottomleftNeighborColumn].Pop(Color.white);
//					Debug.Log(gameManager.bubbles [BottomleftNeighborRow] [BottomleftNeighborColumn].name + " BOTTOM LEFT sameColor");
				}
			}
		}
		int BottomRightNeighborRow = row;
		int BottomRightNeighborColumn = column + 1;
		if (BottomRightNeighborColumn % 2 == 0)
			BottomRightNeighborRow++;
		if(BottomRightNeighborRow < GameManager.MAX_ROW)
		{
			if (gameManager.bubbles [BottomRightNeighborRow] [BottomRightNeighborColumn] != null) 
			{
				if (gameManager.bubbles [BottomRightNeighborRow] [BottomRightNeighborColumn].bubbleColor == theColor) 
				{
					isSameColor = true;
					gameManager.bubbles [BottomRightNeighborRow] [BottomRightNeighborColumn].ValidateNeighbors(theColor);
					gameManager.bubbles [BottomRightNeighborRow] [BottomRightNeighborColumn].Pop(Color.white);
//					Debug.Log(gameManager.bubbles[BottomRightNeighborRow] [BottomRightNeighborColumn].name + " BOTTOM RIGHT sameColor");
				}
			}
		}
		if (isSameColor) 
		{
//			GameManager.bubbleCount++;
//			Debug.Log (GameManager.bubbleCount);
//			gameManager.SubscribeToPopBubbleEvent(Pop);
//			if (GameManager.bubbleCount >= 3) {
//				Pop (Color.white);
//				gameManager.CallPopBubbleEvent (theColor);
//			} 
		}
		else 
		{
			Debug.Log ("IM NOT THE SAME");
//			gameManager.CallPopBubbleEvent (theColor);
		}

		if(BubbleLandedEvent != null) 
			BubbleLandedEvent(bubbleColor);
	}
		
	private void OnTriggerEnter2D(Collider2D otherCollider) 
	{
		Border border = otherCollider.GetComponent<Border>() as Border;
		if (isMoving == true) 
		{
			if (border != null) 
				if (border.name != "TopBorder")
					return;
			Clipper otherClipper = otherCollider.GetComponent<Clipper>() as Clipper;
			Bubble otherBubble = null; 
			if (otherClipper != null) 
				otherBubble = otherClipper.bubble;
			if (otherBubble != null) 
			{
				if (otherCollider.name == "BottomSnapLeft") 
				{
					column = otherBubble.column + 1;
					row = otherBubble.row;
					if (column % 2 != 0) 
						if (row - 1 >= 0) 
							row--;
				}
				else if (otherCollider.name == "BottomSnapRight") 
				{
					column = otherBubble.column + 1;
					row = otherBubble.row;
					if (column % 2 == 0) 
						if (row + 1 < GameManager.MAX_ROW) 
							row++;
				}
				else if (otherCollider.name == "Left") 
				{
					column = otherBubble.column;
					row = otherBubble.row;
					if (row - 1 >= 0)
						row--;
					else
						column++;
					if (gameManager.bubbles [row] [column] != null) 
						column++;
				}
				else if (otherCollider.name == "Right") 
				{
					row = otherBubble.row;
					column = otherBubble.column;
					if (row + 1 < GameManager.MAX_ROW)
						row++;
					else
						column++;
						
				}
				gameManager.bubbles [row] [column] = this;
				Vector2 hexpos = hexGrid.HexOffset(row ,column);
				Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
				transform.position = pos;
				direction = Vector3.zero;
				myCollider.enabled = false;
				mySnapColliders.SetActive(true);
				name = row + "-" + column;
				transform.parent = gameManager.transform;
				ValidateNeighbors(bubbleColor);
			} 
			isMoving = false;
		}
	}
		
}
