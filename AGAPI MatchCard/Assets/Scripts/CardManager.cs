using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public GameObject cardPrefab;
    public Transform gridParent;

    public Sprite[] emojiSprites;

    private Card firstCard = null;
    private Card secondCard = null;
    private bool canClick = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateGrid(4, 4);
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
       
        yield return new WaitForSeconds(0.5f);

        if (firstCard.cardID == secondCard.cardID)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
        }

        firstCard = null;
        secondCard = null;
        canClick = true;
    }
}
