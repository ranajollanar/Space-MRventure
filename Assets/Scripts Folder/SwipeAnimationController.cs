using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject card;
    public GameObject swipeArrow;

    private bool swiped = false;
    public Vector3 initialCardPosition;
    private Quaternion initialSwipeRotation;
    public Vector3 targetCardPosition = Vector3.zero;

    void Start()
    {
        initialCardPosition = card.transform.position;
        initialSwipeRotation = swipeArrow.transform.localRotation;
    }
    public void OnButtonClick()
    {
        swiped = !swiped;

        if (swiped)
        {
            targetCardPosition = new Vector3(5.58502197f, 0f, 0f);
            Debug.Log("targetCardPosition : " + targetCardPosition);
            swipeArrow.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
            StartCoroutine(LerpPosition());

        }
        else
        {
            // Move the card back to its initial position
            targetCardPosition = initialCardPosition;

            // Restore the initial rotation of the swipe arrow
            swipeArrow.transform.localRotation = initialSwipeRotation;
        }
    }

    void Update()
    {
        //card.transform.position = Vector3.Lerp(card.transform.position, targetCardPosition, Time.deltaTime * 2f);
    }

     IEnumerator LerpPosition()
    {
        card.transform.position = Vector3.Lerp(card.transform.position, targetCardPosition, Time.deltaTime * 2f);

        yield return null;
    }
}
