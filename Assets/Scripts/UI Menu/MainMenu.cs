using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public GameObject FadeToBlackForMenuUI;
	FadeToBlackForMenu fadeToBlackForMenu;
	
	void Start()
	{
		fadeToBlackForMenu = GameObject.FindObjectOfType(typeof(FadeToBlackForMenu)) as FadeToBlackForMenu;
		FadeToBlackForMenuUI.SetActive(true);
	}
	
	void Update()
	{
		FadeToBlackForMenuUI.SetActive(false);
		//fadeToBlackForMenu.Fade(true);
		Invoke(nameof(EnableButtons), 2f);
	}
	
    public void PlayGame()
    {
        Time.timeScale = 1f;
		FadeToBlackForMenuUI.SetActive(true);
		if (DifficultyManager.difficultyLevel < 1)
		{
			SceneManager.LoadScene("Tutorial");
		} else {
			SceneManager.LoadScene("Map");
		}
    }

    public void QuitGame ()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
	
	void EnableButtons()
	{
		//FadeToBlackForMenuUI.SetActive(false);
		CancelInvoke(nameof(EnableButtons));
	}
}
