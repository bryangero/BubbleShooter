using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
	private int numberOfShots;
	private GameManager gameManager;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private GameObject bubbleGameObject;
	[SerializeField] private GameObject direction;
	private Bubble bubble;
	private bool isBubbleShot;
	private bool isGameOver;

	private void Start() 
	{
		gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
		gameManager.SubscribeToGameOverEvent(OnGameOver);
		ReloadBubble(Color.white);	
	}

	public void ReloadBubble (Color color)
	{
		isBubbleShot = false;
		bubble = Instantiate(bubbleGameObject).GetComponent<Bubble>() as Bubble;
		bubble.name = numberOfShots.ToString();
		numberOfShots++;
	}

	public void OnGameOver()
	{
		isGameOver = true;
	}
		
	private void Update() 
	{
		if (isGameOver)
			return;
		if (isBubbleShot)
			return;
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
		rot.x = 0;
		rot.y = 0;
		transform.rotation = rot;
//		Debug.DrawLine(transform.position, transform.position + mousePosition);
		if(Input.GetMouseButtonUp(0))
		{
			if (bubble != null) 
			{
				bubble.direction = direction.transform.position;
				bubble.BubbleLandedEvent += ReloadBubble;
				isBubbleShot = true;
			} 
		}
	}

}
