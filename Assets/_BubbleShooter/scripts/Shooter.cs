using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
	[SerializeField] private float rotationSpeed;
	[SerializeField] private GameObject bubbleGameObject;
	[SerializeField] private GameObject direction;

	private void Update() 
	{
		if(Input.GetKey(KeyCode.LeftArrow)) 
		{
			transform.Rotate(0f, 0f, rotationSpeed);
		}
		if(Input.GetKey(KeyCode.RightArrow)) 
		{
			transform.Rotate(0f, 0f, -rotationSpeed);
		}
		if(Input.GetKeyUp(KeyCode.Space))
		{
			Bubble bubble = Instantiate(bubbleGameObject).GetComponent<Bubble>() as Bubble;
			bubble.direction = direction.transform.position; 
		}
	}

}
