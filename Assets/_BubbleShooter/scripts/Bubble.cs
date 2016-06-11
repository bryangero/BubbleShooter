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

	public void Pop()
	{
		gameManager.bubbles[row][column] = null;
		Destroy(gameObject);
	}

	public void CancelPop() 
	{
		isChecked = false;
	}

	public void ValidateNeighbors(Color theColor) 
	{
		if (isChecked == true)
			return;
		if (bubbleColor != theColor)
			return;
		isChecked = true;
		int topleftNeighborRow = row;
		int topleftNeighborColumn = column - 1;
		if (topleftNeighborColumn % 2 != 0) 
			topleftNeighborRow--;
		if (topleftNeighborColumn >= 0 && topleftNeighborRow  >= 0) 
		{
			if (gameManager.bubbles[topleftNeighborRow][topleftNeighborColumn] != null) 
			{
				if (gameManager.bubbles[topleftNeighborRow][topleftNeighborColumn].bubbleColor == theColor) 
				{
					gameManager.bubbles[topleftNeighborRow][topleftNeighborColumn].ValidateNeighbors(theColor);
					gameManager.SubscribeToPopBubbleEvent(gameManager.bubbles[topleftNeighborRow][topleftNeighborColumn].Pop,
														  gameManager.bubbles[topleftNeighborRow][topleftNeighborColumn].CancelPop);
				}
			}
		}
		int topRightNeighborRow = row;
		int topRightNeighborColumn = column - 1;
		if (topRightNeighborColumn % 2 == 0) 
			topRightNeighborRow++;
		if (topRightNeighborRow < GameManager.MAX_ROW && topRightNeighborColumn >= 0) 
		{
			if (gameManager.bubbles[topRightNeighborRow][topRightNeighborColumn] != null) 
			{
				if (gameManager.bubbles[topRightNeighborRow][topRightNeighborColumn].bubbleColor == theColor) 
				{
					gameManager.bubbles[topRightNeighborRow][topRightNeighborColumn].ValidateNeighbors(theColor);
					gameManager.SubscribeToPopBubbleEvent(gameManager.bubbles[topRightNeighborRow][topRightNeighborColumn].Pop,
														  gameManager.bubbles[topRightNeighborRow][topRightNeighborColumn].CancelPop);
				}
			}
		}
		int leftNeighborRow = row - 1;
		int leftNeighborColumn = column;
		if(leftNeighborRow >= 0)
		{
			if (gameManager.bubbles[leftNeighborRow][leftNeighborColumn] != null) 
			{
				if (gameManager.bubbles[leftNeighborRow][leftNeighborColumn].bubbleColor == theColor) 
				{
					gameManager.bubbles[leftNeighborRow][leftNeighborColumn].ValidateNeighbors(theColor);
					gameManager.SubscribeToPopBubbleEvent(gameManager.bubbles[leftNeighborRow][leftNeighborColumn].Pop,
														  gameManager.bubbles[leftNeighborRow][leftNeighborColumn].CancelPop);
				}
			}
		}
		int rightNeighborRow = row + 1;
		int rightNeighborColumn = column;
		if (rightNeighborRow < GameManager.MAX_ROW) 
		{
			if (gameManager.bubbles[rightNeighborRow][rightNeighborColumn] != null)
			{
				if (gameManager.bubbles[rightNeighborRow][rightNeighborColumn].bubbleColor == theColor) 
				{
					gameManager.bubbles[rightNeighborRow][rightNeighborColumn].ValidateNeighbors(theColor);
					gameManager.SubscribeToPopBubbleEvent(gameManager.bubbles[rightNeighborRow][rightNeighborColumn].Pop, 
														  gameManager.bubbles[rightNeighborRow][rightNeighborColumn].CancelPop);
				}
			}
		}
		int bottomleftNeighborRow = row;
		int bottomleftNeighborColumn = column + 1;
		if (bottomleftNeighborColumn % 2 != 0) 
			bottomleftNeighborRow--;
		if (bottomleftNeighborColumn >= 0 && bottomleftNeighborRow >= 0) 
		{
			if (gameManager.bubbles[bottomleftNeighborRow][bottomleftNeighborColumn] != null) 
			{
				if (gameManager.bubbles[bottomleftNeighborRow][bottomleftNeighborColumn].bubbleColor == theColor) 
				{
					gameManager.bubbles[bottomleftNeighborRow][bottomleftNeighborColumn].ValidateNeighbors(theColor);
					gameManager.SubscribeToPopBubbleEvent(gameManager.bubbles[bottomleftNeighborRow][bottomleftNeighborColumn].Pop, 
														  gameManager.bubbles[bottomleftNeighborRow][bottomleftNeighborColumn].CancelPop);
				}
			}
		}
		int bottomRightNeighborRow = row;
		int bottomRightNeighborColumn = column + 1;
		if (bottomRightNeighborColumn % 2 == 0)
			bottomRightNeighborRow++;
		if(bottomRightNeighborRow < GameManager.MAX_ROW)
		{
			if (gameManager.bubbles[bottomRightNeighborRow][bottomRightNeighborColumn] != null) 
			{
				if (gameManager.bubbles[bottomRightNeighborRow][bottomRightNeighborColumn].bubbleColor == theColor) 
				{
					gameManager.bubbles[bottomRightNeighborRow][bottomRightNeighborColumn].ValidateNeighbors(theColor);
					gameManager.SubscribeToPopBubbleEvent(gameManager.bubbles[bottomRightNeighborRow][bottomRightNeighborColumn].Pop,
														  gameManager.bubbles[bottomRightNeighborRow][bottomRightNeighborColumn].CancelPop);
				}
			}
		}
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
					if (gameManager.bubbles[row][column] != null) 
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
				gameManager.bubbles[row][column] = this;
				Vector2 hexpos = hexGrid.HexOffset(row, column);
				Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
				transform.position = pos;
				direction = Vector3.zero;
				myCollider.enabled = false;
				mySnapColliders.SetActive(true);
				name = row + "-" + column;
				transform.parent = gameManager.transform;
				StartCoroutine(gameManager.ValidateNeighbors(bubbleColor, row, column));
				if(BubbleLandedEvent != null) 
					BubbleLandedEvent(bubbleColor);
			} 
			isMoving = false;
		}
	}
		
}
