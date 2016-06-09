using UnityEngine;
using System.Collections;

public class HexGrid : MonoBehaviour 
{
	ArrayList bubbles = new ArrayList();
	private GameManager gameManager;
	public Transform spawnThis;   
	public int x = 5;
	public int y = 5;
	public float radius = 0.5f;
	public bool useAsInnerCircleRadius = true;
	private float offsetX, offsetY;

	private void Start() 
	{
		gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
		float unitLength = (useAsInnerCircleRadius) ? (radius / (Mathf.Sqrt(3) / 2)) : radius;
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetY = unitLength * 1.5f;
		for( int i = 0; i < x; i++ ) 
		{
			
			for( int j = 0; j < y; j++ ) 
			{
				Vector2 hexpos = HexOffset(i, j);
				Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
				Transform t = Instantiate(spawnThis, pos, Quaternion.identity) as Transform;
				t.transform.parent = transform;
				t.name = i +"-"+ j;
				t.GetComponent<Bubble>().row = i;
				t.GetComponent<Bubble>().column = j;
			}
		}
	}

	public Vector2 HexOffset(int x, int y) 
	{
		Vector2 position = Vector2.zero;
		if(y % 2 == 0) 
		{
			position.x = transform.position.x + ( x * offsetX);
			position.y = transform.position.y + ( y * -offsetY);
		}
		else 
		{
			position.x = transform.position.x + ( (x + 0.5f) * offsetX);
			position.y = transform.position.y + (y * -offsetY);
		}
		return position;
	}

}