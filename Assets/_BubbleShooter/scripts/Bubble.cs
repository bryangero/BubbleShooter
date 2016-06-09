using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour 
{
	
	public delegate void BubbleLandedDG();
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
//		UpdateHits();
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
		if (this.bubbleColor == bubbleColor) {
			gameManager.UnsubscribeToPopBubbleEvent(Pop);
			Destroy(gameObject);
		}
	}

	public void ValidateColorForPop(Color bubbleColor)
	{
		if (this.bubbleColor == bubbleColor) {
			UpdateHits();
		}
	}

	private void UpdateHits() {
		int bubbleLayer = LayerMask.NameToLayer("bubble");
		int bubbleLayerMask = 1 << bubbleLayer;
		hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), 
			.4f, 
			Vector2.up,
			0f,
			bubbleLayerMask);
		Debug.Log(name + " " + hits.Length);
//		for (int i = 0; i < hits.Length; i++) 
//		{
//			Bubble bubble = hits[i].collider.GetComponent<Bubble>();
//			gameManager.SubscribeToPopBubbleEvent(Pop);
//		}
	}

	private void OnTriggerEnter2D(Collider2D otherCollider) 
	{
		Border border = otherCollider.GetComponent<Border>() as Border;
		if (isMoving == true) 
		{
			if (border != null) 
			{
				if (border.name != "TopBorder")
					return;
			}
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
					{
						if (row - 1 >= 0) 
							row--;
					}

				}
				else if (otherCollider.name == "BottomSnapRight") 
				{
					column = otherBubble.column + 1;
					row = otherBubble.row;
					if (column % 2 == 0) 
					{
						if (row + 1 < GameManager.MAX_ROW) 
							row++;
					}
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
					{
						column++;
					}
						
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
				otherBubble.ValidateColorForPop(bubbleColor);
			} 
			isMoving = false;
			if(BubbleLandedEvent != null) 
				BubbleLandedEvent();
		}
	}
		
}
