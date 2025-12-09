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
        
        animator.Play("Idle", 0, 0f);
    }

    public void ShowFrontInstant()
    {
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
        isFlipped = true;
    }

    public void ShowBackInstant()
    {
        backImage.gameObject.SetActive(true);
        frontImage.gameObject.SetActive(false);
        isFlipped = false;
    }

    public void OnCardClicked()
    {
        if (isMatched) return;
        if (isFlipped) return;

        animator.Play("CardFlip", 0, 0f);

        Invoke(nameof(ShowFront), 0.15f);

        CardManager.Instance.CardRevealed(this);
    }

    void ShowFront()
    {
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
        isFlipped = true;
    }

    // Called when mismatch happens
    public void FlipBack()
    {
        if (!isFlipped || isMatched) return;

        animator.Play("CardFlip", 0, 0f);
        Invoke(nameof(ShowBack), 0.15f);
    }

    void ShowBack()
    {
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        isFlipped = false;
    }

    // Called when match happens
    public void SetMatched()
    {
        isMatched = true;
        var btn = GetComponent<Button>();
        if (btn != null) btn.interactable = false;
    }

    public bool IsMatched() => isMatched;

    // Hide card for matched pairs
    public void RemoveCard()
    {
        var btn = GetComponent<Button>();
        if (btn != null) btn.interactable = false;

        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(false);

        var img = GetComponent<Image>();
        if (img != null) img.enabled = false;
    }
}
