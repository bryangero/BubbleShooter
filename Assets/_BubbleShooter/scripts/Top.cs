using UnityEngine;
using System.Collections;

public class Top : MonoBehaviour {

	[SerializeField] private GameManager gameManager;

	public int row;
	public int column;

	public void OnTriggerEnter2D(Collider2D otherCollider) 
	{
		Bubble bubble = otherCollider.GetComponent<Bubble>() as Bubble;
		if (bubble)
		{
			bubble.transform.position = transform.position;
			gameManager.bubbles [row] [column] = bubble;
		}
			
	}



}
