using UnityEngine;

public class MobileController : MonoBehaviour, IControllable
{
    [SerializeField] private float minSwipeDistance = 50f;
    
    private Vector2 touchStartPos;
    private bool isTouching = false;
    
    private bool swipeUp = false;
    private bool swipeDown = false;
    private bool swipeLeft = false;
    private bool swipeRight = false;
    
    void Update()
    {
        swipeUp = false;
        swipeDown = false;
        swipeLeft = false;
        swipeRight = false;
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isTouching = true;
                    break;
                    
                case TouchPhase.Ended:
                    if (isTouching)
                    {
                        CheckSwipe(touch.position);
                        isTouching = false;
                    }
                    break;
                    
                case TouchPhase.Canceled:
                    isTouching = false;
                    break;
            }
        }
        
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isTouching = true;
        }

        if (Input.GetMouseButtonUp(0) && isTouching)
        {
            CheckSwipe(Input.mousePosition);
            isTouching = false;
        }
        #endif
    }
    
    private void CheckSwipe(Vector2 touchEndPos)
    {
        Vector2 swipeDelta = touchEndPos - touchStartPos;
        
        if (swipeDelta.magnitude < minSwipeDistance)
            return;
        
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0)
                swipeRight = true;
            else
                swipeLeft = true;
        }
        else
        {
            if (swipeDelta.y > 0)
                swipeUp = true;
            else
                swipeDown = true;
        }
    }
    
    public bool IsUp() => swipeUp;
    public bool IsDown() => swipeDown;
    public bool IsLeft() => swipeLeft;
    public bool IsRight() => swipeRight;
}