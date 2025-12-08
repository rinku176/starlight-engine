using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{

    public static CardManager Instance;

    public GameObject cardPrefab;
    public Transform gridParent;

    public Sprite[] emojiSprites;

    public TextMeshProUGUI movesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    private int moves = 0;
    private int score = 0;
    private int totalPairs = 8; //for 4x4 grid
    private float timer = 0f;
    private bool isTimerRunning = true;

    private Card firstCard = null;
    private Card secondCard = null;
    private bool canClick = true;

    public GameObject winPanel;
    public TextMeshProUGUI summaryText;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateGrid(4, 4);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            timerText.text = "Time: " + Mathf.FloorToInt(timer) + "secs";
        }
    }

    void CreateGrid(int rows, int cols)
    {
        int totalCards = rows * cols;

        // create IDs for the cards
        List<int> ids = new List<int>();
        for (int i = 1; i <= totalCards / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        // shuffle the IDs
        for (int i = 0; i < ids.Count; i++)
        {
            int randomIndex = Random.Range(i, ids.Count);
            int temp = ids[i];
            ids[i] = ids[randomIndex];
            ids[randomIndex] = temp;
        }

        // spawn cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject newCardGO = Instantiate(cardPrefab, gridParent);
            Card cardComponent = newCardGO.GetComponent<Card>();
            cardComponent.cardID = ids[i];
            cardComponent.frontImage.sprite = emojiSprites[ids[i] - 1];

        }
    }

    public void CardRevealed(Card card)
    {
        if (!canClick) return;

        if (firstCard == null)
        {
            
            firstCard = card;
        }
        else if (secondCard == null && card != firstCard)
        {
            
            secondCard = card;
            canClick = false; 

            StartCoroutine(ResolveCards());
        }
    }

    IEnumerator ResolveCards()
    {
        moves++;
        movesText.text = "Moves: " + moves;

        yield return new WaitForSeconds(0.5f);

        if (firstCard.cardID == secondCard.cardID)
        {
            score++;
            scoreText.text = "Score: " + score;

            firstCard.SetMatched();
            secondCard.SetMatched();

            firstCard.RemoveCard();
            secondCard.RemoveCard();

        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
        }

        firstCard = null;
        secondCard = null;
        canClick = true;

        if (score == totalPairs)
        {
            EndGame();
        }

    }


    void EndGame()
    {
        isTimerRunning = false;
        winPanel.SetActive(true);

        string summary = "";
        summary += "YOU WIN!\n\n";
        summary += "Moves: " + moves + "\n";
        summary += "Score: " + score;
        summary += "Time: " + Mathf.FloorToInt(timer) + "seconds";

        summaryText.text = summary;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

}
