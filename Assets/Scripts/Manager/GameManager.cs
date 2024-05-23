using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            UIController.Instance.SetMenuPanel(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public string ReturnSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void EndScene()
    {
        SceneManager.LoadScene("EndScene");
    }

}
