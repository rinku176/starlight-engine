using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Animator animator;
    public Image frontImage;
    public Image backImage;
    public int cardID;

    private bool isFlipped = false;
    private bool isMatched = false;

    void Start()
    {
        ShowBack(); 
    }

    void ShowBack()
    {
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        isFlipped = false;
    }

    void ShowFront()
    {
        backImage.gameObject.SetActive(false);
        frontImage.gameObject.SetActive(true);
        isFlipped = true;
    }

    public void OnCardClicked()
    {
        // ignore clicks if this card is already matched or face up
        if (isMatched) return;
        if (isFlipped) return;

        // if manager is missing
        if (CardManager.Instance == null)
        {
            return;
        }

        animator.Play("Flip", 0, 0f);

        Invoke(nameof(ShowFrontAndNotifyManager), 0.15f);
    }

    void ShowFrontAndNotifyManager()
    {
        ShowFront();
        CardManager.Instance.CardRevealed(this);
    }

    // Called by cardmanager when card is of a non-matching pair
    public void FlipBack()
    {
        if (!isFlipped || isMatched) return;

        // play flip animation again, then show back
        animator.Play("Flip", 0, 0f);
        Invoke(nameof(ShowBack), 0.15f);
    }

    // Called by cardmanager when card is of a matching pair
    public void SetMatched()
    {
        isMatched = true;

        //disable button so it can't be clicked again
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.interactable = false;
        }
    }

    public void RemoveCard()
    {
        // Disable button so the player can't click it anymore
        var btn = GetComponent<Button>();
        if (btn != null)
            btn.interactable = false;

        // Hide the front and back images
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(false);

        // Optional: make card transparent (still takes space in grid)
        var img = GetComponent<Image>();
        if (img != null)
            img.enabled = false;

        // Card remains in grid, but invisible and unclickable
    }
}
