using UnityEngine;
using System.Collections;

public class Border : MonoBehaviour 
{

	[SerializeField] private Vector3 newDirection;

	private void OnTriggerEnter2D(Collider2D otherCollider) 
	{
		Bubble bubble = otherCollider.GetComponent<Bubble>() as Bubble;
		bubble.direction = new Vector3(bubble.direction.x * newDirection.x, 
									   bubble.direction.y * newDirection.y);
	}

}
