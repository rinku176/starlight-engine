using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;

    public void PlayEasy()
    {
        CardManager.rows = 2;
        CardManager.cols = 2;
        Debug.Log("start Easy");
        StartGame();
    }

    public void PlayMedium()
    {
        CardManager.rows = 4;
        CardManager.cols = 4;
        StartGame();
    }

    public void PlayHard()
    {
        CardManager.rows = 5;
        CardManager.cols = 6;
        StartGame();
    }

    void StartGame()
    {
        mainMenu.SetActive(false);

        FindObjectOfType<CardManager>().BeginGame();
    }

}
