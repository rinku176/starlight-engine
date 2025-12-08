using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public GameObject cardPrefab;
    public Transform gridParent;

    private Card firstCard = null;
    private Card secondCard = null;

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

        
        List<int> ids = new List<int>();

        for (int i = 1; i <= totalCards / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        // shuffle the list
        for (int i = 0; i < ids.Count; i++)
        {
            int temp = ids[i];
            int randomIndex = Random.Range(i, ids.Count);
            ids[i] = ids[randomIndex];
            ids[randomIndex] = temp;
        }

        // create cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, gridParent);
            Card card = newCard.GetComponent<Card>();
            card.cardID = ids[i];
        }
    }

    public void CardRevealed(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            secondCard = card;
            CheckMatch();
        }
    }

    void CheckMatch()
    {
        if (firstCard.cardID == secondCard.cardID)
        {
            Debug.Log("MATCH!");
        }
        else
        {
            Debug.Log("NO MATCH!");
        }

        firstCard = null;
        secondCard = null;
    }
}
