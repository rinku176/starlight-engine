using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Animator animator;
    public Image frontImage;
    public Image backImage;
    public int cardID;

    public AudioClip flipSound;
    
    private AudioSource audioSource;

    private bool isFlipped = false;
    private bool isMatched = false;

    void Start()
    {
        animator.Play("Idle", 0, 0f);
        audioSource = GetComponent<AudioSource>();
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
        if (CardManager.Instance == null) return;

        if (flipSound != null && !CardManager.Instance.IsPlayingFeedbackSound())
            audioSource.PlayOneShot(flipSound);

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

    public IEnumerator MatchAnimation()
    {
        // save original values
        Vector3 originalScale = transform.localScale;
        Vector3 bigScale = originalScale * 1.2f;
        Vector3 smallScale = originalScale * 0.8f;

        Image cardBG = GetComponent<Image>();
        Color startColor = cardBG.color;

        float t = 0f;

        // scale up
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, bigScale, t / 0.15f);
            yield return null;
        }

        // Reset timer
        t = 0f;

        // scale down + fadeout
        while (t < 0.15f)
        {
            t += Time.deltaTime;

            transform.localScale = Vector3.Lerp(bigScale, smallScale, t / 0.15f);

            Color fade = startColor;
            fade.a = Mathf.Lerp(1f, 0f, t / 0.15f);
            cardBG.color = fade;

            yield return null;
        }

        RemoveCard();
    }


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
