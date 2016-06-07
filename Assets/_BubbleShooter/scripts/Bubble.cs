using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour 
{
	
	[SerializeField] private float speed;
	public Color bubbleColor;
	public bool isMoving;
	public Vector3 direction;
	public Rigidbody2D rigidbody2D;
	public Transform topRight;
	public Transform topLeft;
	public Transform BottomRight;
	public Transform BottomLeft;
	public Transform Right;
	public Transform Left;
	public RaycastHit2D[] hits;

	private void Start() 
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		Color[] colors = new Color[5] { Color.red, Color.blue, Color.yellow, 
										Color.green, Color.magenta };
		bubbleColor = colors[Random.Range(0, colors.Length)];
		gameObject.GetComponent<SpriteRenderer>().color = bubbleColor;
	}

	private void Update() 
	{
		int bubbleLayer = LayerMask.NameToLayer("bubble");
		int bubbleLayerMask = 1 << bubbleLayer;
		hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), 
									   1f, 
									   Vector2.up,
									   1f,
 									   bubbleLayerMask);
		if (direction != Vector3.zero) 
			isMoving = true;
		transform.Translate(direction * (Time.deltaTime*speed));
	}

	public void Pop(Color bubbleColor)
	{
		if (this.bubbleColor == bubbleColor) 
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D otherCollider) 
	{
		Bubble otherBubble = otherCollider.GetComponent<Bubble>() as Bubble;
		if (isMoving == true) 
		{
			if (otherBubble != null) 
			{
				direction = Vector3.zero;
				otherBubble.Pop(bubbleColor);
				if (otherBubble.bubbleColor == bubbleColor)
					Pop (bubbleColor);
			} 
			isMoving = false;
		}
	}
		
}
