using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {

	public ArrayList bubbles = new ArrayList();

	public delegate void PopBubbleDG(Color color);
	public event PopBubbleDG PopBubbleEvent;


	public void SubscribeToPopBubbleEvent(PopBubbleDG popBubbleDG) {
		if (PopBubbleEvent == null || !PopBubbleEvent.GetInvocationList ().Contains (popBubbleDG)) {
			PopBubbleEvent += popBubbleDG;
		} else {
			Debug.Log ("UNSUCCESSFUL BIND");
		}
	}

	public void UnsubscribeToPopBubbleEvent(PopBubbleDG popBubbleDG) {
		if (PopBubbleEvent.GetInvocationList().Contains(popBubbleDG)) {
			PopBubbleEvent -= popBubbleDG;
		} 
	}

	public void CallPopBubbleEvent(Color color) {
		PopBubbleEvent(color);
	}
		
}
