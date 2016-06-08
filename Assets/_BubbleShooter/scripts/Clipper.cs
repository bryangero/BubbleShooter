using UnityEngine;
using System.Collections;

public class Clipper : MonoBehaviour {

	public Bubble bubble;
	public CLIPPER_SIDE clipperSide;

	private void Start() {
//		bubble = transform.parent.GetComponent<Bubble> () as Bubble;

	}

	private void OnTriggerEnter2D(Collider2D otherCollider) 
	{
		Clipper clipper = otherCollider.GetComponent<Clipper>() as Clipper;
		if (clipper != null) 
		{
			if (clipperSide == CLIPPER_SIDE.TOP_RIGHT && clipper.clipperSide == CLIPPER_SIDE.BOTTOM_LEFT) {
				transform.parent.transform.position = new Vector3 (otherCollider.transform.position.x - 0.35f,
					otherCollider.transform.position.y - 0.35f);
			}
			else if(clipperSide == CLIPPER_SIDE.TOP_LEFT && clipper.clipperSide == CLIPPER_SIDE.BOTTOM_RIGHT) 
			{
			transform.parent.transform.position = new Vector3(otherCollider.transform.position.x + 0.35f,
					otherCollider.transform.position.y + 0.35f);
			}

			if (clipper != null) 
			{
				bubble.direction = Vector3.zero;
			} 
			bubble.isMoving = false;

			bubble.Test (otherCollider);
		}
		Debug.Log (name + " CLIP " + otherCollider.name);
	}



}
