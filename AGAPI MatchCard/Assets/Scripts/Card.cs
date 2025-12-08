using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Animator animator;
    public Image frontImage;
    public Image backImage;
    public int cardID; // used for matching cards

    private bool isFlipped = false;

    void Start()
    {
       
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
    }

    public void OnCardClicked()
    {
        if (isFlipped) return;  

        isFlipped = true;

        animator.SetTrigger("Flip");

        Invoke("ShowFront", 0.15f);
    }

    void ShowFront()
    {
        Debug.Log("flip");
        backImage.gameObject.SetActive(false);
        frontImage.gameObject.SetActive(true);
    }
}
