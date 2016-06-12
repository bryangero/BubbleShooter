using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
	[SerializeField] private GameObject bubbleGameObject;
	[SerializeField] private GameObject direction;
	private GameManager gameManager;
	private Bubble bubble;
	private bool isBubbleShot;
	private bool isEndGame;

	private void Start() 
	{
		gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
		gameManager.SubscribeToEndGameEvent(OnEndGame);
		ReloadBubble(Color.white);	
	}

	public void ReloadBubble (Color color)
	{
		isBubbleShot = false;
		bubble = Instantiate(bubbleGameObject).GetComponent<Bubble>() as Bubble;
	}

	public void OnEndGame(bool isWin)
	{
		isEndGame = true;
	}
		
	private void Update() 
	{
		if (isEndGame == true)
			return;
		if (isBubbleShot)
			return;
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Quaternion newRotation = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
		newRotation.x = 0;
		newRotation.y = 0;
		transform.rotation = newRotation;
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
