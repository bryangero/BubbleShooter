using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
	public float rotationSpeed;
	public GameObject bubbleGameObject;
	public GameObject direction;

	private void Update() 
	{
		if(Input.GetKey (KeyCode.LeftArrow)) 
		{
			transform.Rotate(0f, 0f, rotationSpeed);
		}
		if(Input.GetKey (KeyCode.RightArrow)) 
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
