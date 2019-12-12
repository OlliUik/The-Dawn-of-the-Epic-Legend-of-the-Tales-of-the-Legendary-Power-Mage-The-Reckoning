using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreditsScroller : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector2 contentPos;
    private float contentSize;
    private float startPos;
    private float viewPortSize;
    private ScrollRect scrollRect;

    private bool isDraged = false;
    [SerializeField] private float scrollSpeed = 1;
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
        if (!isDraged)
        {
            contentPos = new Vector2(0f, contentPos.y + scrollSpeed);
            scrollRect.content.localPosition = contentPos;
        }

        if (contentPos.y > (startPos + ((contentSize + viewPortSize) + 35)))
        {
            contentPos = new Vector2(0f, startPos);
        } else if (contentPos.y < startPos)
        {
            contentPos = new Vector2(0f, startPos + ((contentSize + viewPortSize) - 35));
        }
    }

    public void Reset()
    {
        contentPos = new Vector2(0f, startPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDraged = true;
        contentPos = scrollRect.content.localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        contentPos = scrollRect.content.localPosition;
        isDraged = false;
    }
}
