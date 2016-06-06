using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour 
{

	[SerializeField] private float speed;
	public Vector3 direction;

	private void Update() 
	{
		transform.Translate(direction * (Time.deltaTime*speed));
	}


}
