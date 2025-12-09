using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;

    public static int rows = 4;
    public static int cols = 4;

    private int moves = 0;
    private int score = 0;
    private int totalPairs; 
    private float timer = 0f;
    private bool isTimerRunning = true;

    
    private bool isEvaluating = false;
    private List<Card> revealedCards = new List<Card>();


    public GameObject winPanel;
    public TextMeshProUGUI summaryText;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isTimerRunning = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            timerText.text = "Time: " + Mathf.FloorToInt(timer) + " secs";
        }
    }

    public void BeginGame()
    {
        totalPairs = (rows * cols) / 2;

        CreateGrid(rows, cols);
        AdjustCardSizes(rows, cols);
        StartCoroutine(StartGamePreview());
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

    void AdjustCardSizes(int rows, int cols)
    {
        GridLayoutGroup grid = gridParent.GetComponent<GridLayoutGroup>();
        RectTransform rt = gridParent.GetComponent<RectTransform>();

        
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = cols;

        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        // width and height inside CardArea
        float totalWidth = rt.rect.width - spacingX * (cols - 1);
        float totalHeight = rt.rect.height - spacingY * (rows - 1);

        // size  per cell
        float cellWidth = totalWidth / cols;
        float cellHeight = totalHeight / rows;

        // keep cards square
        float finalSize = Mathf.Min(cellWidth, cellHeight);

        grid.cellSize = new Vector2(finalSize, finalSize);
    }

    public void CardRevealed(Card card)
    {
        if (card.IsMatched()) return;
        if (revealedCards.Contains(card)) return;

        revealedCards.Add(card);

        // Only evaluate when at least 2 cards are face-up
        if (!isEvaluating)
            StartCoroutine(EvaluateCards());
    }




    IEnumerator EvaluateCards()
    {
        isEvaluating = true;

        while (revealedCards.Count >= 2)
        {
            Card c1 = revealedCards[0];
            Card c2 = revealedCards[1];


            moves++;
            movesText.text = "Moves: " + moves;

            yield return new WaitForSeconds(0.5f);

            if (c1.cardID == c2.cardID)
            {
                if (correctSound != null)
                    audioSource.PlayOneShot(correctSound);

                score++;
                scoreText.text = "Score: " + score;

                c1.SetMatched();
                c2.SetMatched();

                c1.RemoveCard();
                c2.RemoveCard();
            }
            else
            {
                if (wrongSound != null)
                    audioSource.PlayOneShot(wrongSound);

                c1.FlipBack();
                c2.FlipBack();
            }


            revealedCards.RemoveAt(0);
            revealedCards.RemoveAt(0);
            yield return null;
        
        }

        isEvaluating = false;

        if (score == totalPairs)
            EndGame();
    }


    public bool IsPlayingFeedbackSound()
    {
        return audioSource.isPlaying;
    }

    IEnumerator StartGamePreview()
    {
        // Disable clicking during preview
        isTimerRunning = false;

        //Show all cards front instantly
        foreach (Transform child in gridParent)
        {
            Card card = child.GetComponent<Card>();
            if (card != null)
            {
                card.ShowFrontInstant();
            }
        }

        
        yield return new WaitForSeconds(1f);

        //Hide all cards instantly
        foreach (Transform child in gridParent)
        {
            Card card = child.GetComponent<Card>();
            if (card != null)
            {
                card.ShowBackInstant();
            }
        }

        // Enable clicking and start timer
        isTimerRunning = true;
    }


    void EndGame()
    {
        isTimerRunning = false;
        winPanel.SetActive(true);

        string summary = "";
        summary += "YOU WIN!\n\n";
        summary += "Moves: " + moves + "\n";
        summary += "Score: " + score+ "\n";
        summary += "Time: " + Mathf.FloorToInt(timer) + " seconds";

        summaryText.text = summary;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

}
