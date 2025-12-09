using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;

    public void PlayEasy()
{
    CardManager.rows = 2;
    CardManager.cols = 2;
    CardManager.currentDifficulty = "Easy";
    StartGame();
}

public void PlayMedium()
{
    CardManager.rows = 4;
    CardManager.cols = 4;
    CardManager.currentDifficulty = "Medium";
    StartGame();
    }

    public void PlayHard()
{
    CardManager.rows = 5;
    CardManager.cols = 6;
    CardManager.currentDifficulty = "Hard";
    StartGame();
    }


    void StartGame()
    {
        mainMenu.SetActive(false);

        FindObjectOfType<CardManager>().BeginGame();
    }

}
