using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
	[SerializeField] private float rotationSpeed;
	[SerializeField] private GameObject bubbleGameObject;
	[SerializeField] private GameObject direction;
	private Bubble bubble;
	private bool isBubbleShot;

	private void Start() 
	{
		ReloadBubble();	
	}

	public void ReloadBubble()
	{
		bubble = Instantiate(bubbleGameObject).GetComponent<Bubble>() as Bubble;
	}

	private void Update() 
	{
		float mouseAxisX = Input.GetAxis("Mouse X");
		transform.Rotate(0f, 0f, -mouseAxisX * rotationSpeed);
		if(Input.GetMouseButtonUp(0))
		{
			if (bubble != null) 
			{
				bubble.direction = direction.transform.position;
				bubble.isMoving = true;
				isBubbleShot = true;
			} 
		}
		if (bubble.isMoving == false && isBubbleShot == true) 
		{
			isBubbleShot = false;
			ReloadBubble();
		}
	}

}
