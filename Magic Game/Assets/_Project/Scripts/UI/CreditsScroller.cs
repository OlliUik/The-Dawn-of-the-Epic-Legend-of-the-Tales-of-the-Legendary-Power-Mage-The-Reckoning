using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreditsScroller : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector2 contentPos;
    private ScrollRect scrollRect;

    private float contentSize;
    private float startPos;
    private float viewPortSize;
    private float waitCounter = 0;
    private float cscrolleSpeed = 0;

    private bool isDraged = false;
    [SerializeField] private float scrollSpeed = 1;
    [SerializeField] [Range(0,5)] private int waitTime;
    [SerializeField] private RectTransform contentBox;

    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentPos = scrollRect.content.localPosition;
        contentSize = contentBox.rect.height;
        viewPortSize = scrollRect.viewport.rect.height;
        startPos = contentPos.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (contentPos.y > (startPos + (contentSize + (viewPortSize))))
        {
            contentPos = new Vector2(0f, startPos);
            scrollRect.content.localPosition = contentPos;
        }
        else if (contentPos.y < startPos - viewPortSize)
        {
            contentPos = new Vector2(0f, startPos + (contentSize + viewPortSize));
            scrollRect.content.localPosition = contentPos;
        }

        if (!isDraged)
        {
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
            } else
            {
                contentPos = scrollRect.content.localPosition;

                cscrolleSpeed = Mathf.Lerp(cscrolleSpeed, scrollSpeed, Time.deltaTime);
                contentPos = new Vector2(0f, contentPos.y + cscrolleSpeed);
                scrollRect.content.localPosition = contentPos;
            }
        }
    }

    public void Reset()
    {
        contentPos = new Vector2(0f, startPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDraged = true;
        waitCounter = waitTime;
        cscrolleSpeed = 0;
        contentPos = scrollRect.content.localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        contentPos = scrollRect.content.localPosition;
        isDraged = false;
    }
}
