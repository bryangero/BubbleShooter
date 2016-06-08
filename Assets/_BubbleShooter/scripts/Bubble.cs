using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour 
{
	
	public delegate void BubbleLandedDG();
	public event BubbleLandedDG BubbleLandedEvent;

	[SerializeField] private GameManager gameManager;
	[SerializeField] private float speed;
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

	private void Start() 
	{
		gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
		Color[] colors = new Color[5] { Color.red, Color.blue, Color.yellow, 
										Color.green, Color.magenta };
//		Color[] colors = new Color[1] { Color.red };
		bubbleColor = colors[Random.Range(0, colors.Length)];
		gameObject.GetComponent<SpriteRenderer>().color = bubbleColor;
	}

	private void Update() 
	{
//		UpdateHits();
		if (direction != Vector3.zero) 
			isMoving = true;
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

	public void Test(Collider2D otherCollider) 
	{
			if(BubbleLandedEvent != null) 
				BubbleLandedEvent();
	}



	private void OnTriggerEnter2D(Collider2D otherCollider) 
	{
		Bubble otherBubble = otherCollider.GetComponent<Bubble>() as Bubble;
		Border border = otherCollider.GetComponent<Border>() as Border;
		if (isMoving == true) 
		{
			if (border != null) 
			{
				if (border.name != "TopBorder")
					return;
			}
			if (otherBubble != null) 
			{
				direction = Vector3.zero;
				otherBubble.ValidateColorForPop(bubbleColor);
			} 
			isMoving = false;
			if(BubbleLandedEvent != null) 
				BubbleLandedEvent();
		}
	}

		
}
