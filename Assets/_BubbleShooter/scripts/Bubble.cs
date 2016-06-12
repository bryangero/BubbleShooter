using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour 
{
	public delegate void BubbleLandedDG(Color color);
	public event BubbleLandedDG BubbleLandedEvent;
	[SerializeField] private ScoreDisplay scoreDisplay;
	[SerializeField] private float speed;
	[SerializeField] private GameObject mySnapColliders;
	[SerializeField] private Collider2D myCollider;
	public Color bubbleColor;
	public Vector3 direction;
	public int row;
	public int column;
	public bool isChecked;
	private GameManager gameManager;
	private HexGrid hexGrid;
	private bool isMoving;

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
		mySnapColliders.SetActive(true);
	}

	private void Update() 
	{
		if (direction != Vector3.zero) 
		{
			mySnapColliders.SetActive(false);
			isMoving = true;
			myCollider.enabled = true;
		}
		transform.Translate(direction *(Time.deltaTime*speed));
	}

	public void Pop(int score)
	{
		scoreDisplay.Animate(score);
		gameManager.bubbles[row][column] = null;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponent<AudioSource>().PlayDelayed(Random.Range(0f, 0.2f));
	}

	public void CancelPop() 
	{
		isChecked = false;
	}

	public void ValidateNeighbors(Color colorHit) 
	{
		if (isChecked == true)
			return;
		if (bubbleColor != colorHit)
			return;
		isChecked = true;
		gameManager.SubscribeToPopBubbleEvent(Pop, CancelPop);

		int topleftNeighborRow = row;
		int topleftNeighborColumn = column - 1;
		if (topleftNeighborColumn % 2 != 0) 
			topleftNeighborRow--;
		if (topleftNeighborRow  >= 0 && topleftNeighborColumn >= 0) 
			MakeNeighborValidate(topleftNeighborRow, topleftNeighborColumn, colorHit);
		int topRightNeighborRow = row;
		int topRightNeighborColumn = column - 1;
		if (topRightNeighborColumn % 2 == 0) 
			topRightNeighborRow++;
		if (topRightNeighborRow < GameManager.MAX_ROW && topRightNeighborColumn >= 0) 
			MakeNeighborValidate(topRightNeighborRow, topRightNeighborColumn, colorHit);
		int leftNeighborRow = row - 1;
		int leftNeighborColumn = column;
		if (leftNeighborRow >= 0)
			MakeNeighborValidate(leftNeighborRow, leftNeighborColumn, colorHit);
		int rightNeighborRow = row + 1;
		int rightNeighborColumn = column;
		if (rightNeighborRow < GameManager.MAX_ROW)
			MakeNeighborValidate(rightNeighborRow, rightNeighborColumn, colorHit);
		int bottomleftNeighborRow = row;
		int bottomleftNeighborColumn = column + 1;
		if (bottomleftNeighborColumn % 2 != 0) 
			bottomleftNeighborRow--;
		if (bottomleftNeighborRow >= 0 && 
			bottomleftNeighborColumn < GameManager.MAX_COLUMN) 
			MakeNeighborValidate(bottomleftNeighborRow, bottomleftNeighborColumn, colorHit);
		int bottomRightNeighborRow = row;
		int bottomRightNeighborColumn = column + 1;
		if (bottomRightNeighborColumn % 2 == 0)
			bottomRightNeighborRow++;
		if (bottomRightNeighborRow < GameManager.MAX_ROW && 
			bottomRightNeighborColumn < GameManager.MAX_COLUMN)
			MakeNeighborValidate(bottomRightNeighborRow, bottomRightNeighborColumn, colorHit);
	}

	private void MakeNeighborValidate(int row, int column, Color colorHit) 
	{
		if (gameManager.bubbles[row][column] != null)
		{
			if (gameManager.bubbles[row][column].bubbleColor == colorHit) 
			{
				gameManager.bubbles[row][column].ValidateNeighbors(colorHit);
				gameManager.SubscribeToPopBubbleEvent(gameManager.bubbles[row][column].Pop,
													  gameManager.bubbles[row][column].CancelPop);
			}
		}
	}
		
	private void OnTriggerEnter2D(Collider2D otherCollider) 
	{
		if (isMoving == true) 
		{
			Bubble otherBubble = otherCollider.transform.parent.
											   transform.parent.GetComponent<Bubble>() as Bubble;
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
				if (column < GameManager.MAX_COLUMN)
				{
					gameManager.bubbles[row][column] = this;
					StartCoroutine(gameManager.RunThroughBubbleMatrix(row, column, bubbleColor));
					Vector2 hexpos = hexGrid.HexOffset(row, column);
					Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
					transform.position = pos;
				}
				else  
					gameManager.CallEndGameEvent(false);
				direction = Vector3.zero;
				myCollider.enabled = false;
				mySnapColliders.SetActive(true);
				name = row + "-" + column;
				transform.parent = gameManager.transform;
				if (BubbleLandedEvent != null) 
					BubbleLandedEvent(bubbleColor);
			} 
			isMoving = false;
		}
	}
}
