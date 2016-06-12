using UnityEngine;
using System.Collections;

public class ScoreDisplay : MonoBehaviour 
{
	public TextMesh scoreText;

	public void Animate(int score) 
	{
		gameObject.GetComponent<Animator>().enabled = true;
		scoreText.text = score.ToString();
	}

	public void DestroyOnFinish() 
	{
		Destroy(transform.parent.gameObject);
	}
}
