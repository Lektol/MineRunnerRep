using UnityEngine;

public class MobileController : MonoBehaviour, IControllable
{
    [SerializeField] private float _minSwipeDistance = 30f;
    [SerializeField] private float _minTimeToDoSmth = 0.1f; 
    private float _currentTimeToTouch = 0; 
    
    private Vector2 _touchStartPos;
    private bool _isTouching = false;
    
    private bool _swipeUp = false;
    private bool _swipeDown = false;
    private bool _swipeLeft = false;
    private bool _swipeRight = false;
    
    void Update()
    {
        _swipeUp = false;
        _swipeDown = false;
        _swipeLeft = false;
        _swipeRight = false;
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //_currentTimeToTouch += 1 * Time.deltaTime;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _touchStartPos = touch.position;
                    _isTouching = true;
                    break;

                case TouchPhase.Moved:
                    if(_currentTimeToTouch >= _minTimeToDoSmth)
                    {
                        CheckSwipe(touch.position);
                        _isTouching = false;
                        _currentTimeToTouch = 0;
                    }
                    break;
                    
                case TouchPhase.Ended:
                    if (_isTouching)
                    {
                        CheckSwipe(touch.position);
                        _isTouching = false;
                        _currentTimeToTouch = 0;
                    }
                    break;
                    
                case TouchPhase.Canceled:
                    _isTouching = false;
                    break;
            }
        }
        if(_isTouching) _currentTimeToTouch += 1 * Time.deltaTime;
        Debug.Log(_currentTimeToTouch);
        
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            _touchStartPos = Input.mousePosition;
            _isTouching = true;
        }
        if(_currentTimeToTouch >= _minTimeToDoSmth)
        {
            CheckSwipe(Input.mousePosition);
            _isTouching = false;
            _currentTimeToTouch = 0;
        }

        if (Input.GetMouseButtonUp(0) && _isTouching)
        {
            CheckSwipe(Input.mousePosition);
            _isTouching = false;
        }
        #endif
    }
    
    private void CheckSwipe(Vector2 touchEndPos)
    {
        Vector2 swipeDelta = touchEndPos - _touchStartPos;
        
        if (swipeDelta.magnitude < _minSwipeDistance)
            return;
        
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0)
                _swipeRight = true;
            else
                _swipeLeft = true;
        }
        else
        {
            if (swipeDelta.y > 0)
                _swipeUp = true;
            else
                _swipeDown = true;
        }
    }
    
    public bool IsUp() => _swipeUp;
    public bool IsDown() => _swipeDown;
    public bool IsLeft() => _swipeLeft;
    public bool IsRight() => _swipeRight;
}