using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuUI : MonoBehaviour {

	public void OnClickPlay() 
	{
		SceneManager.LoadScene("Main");	
	}

	public void OnClickExit() 
	{
		Application.Quit();
	}

}
