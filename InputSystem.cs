using System;
using UnityEngine;

public class InputSystem : MonoSingleton<InputSystem>
{
    public static Action<SwipeDirection> OnSwipe;
    public static Action<Transform, Vector3> OnPressed;

    [SerializeField]
    private float swipeThreshold;

    private Vector3 startInputPosition;

    private SwipeDirection swipeDirection;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            startInputPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(startInputPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                OnPressed?.Invoke(hit.transform, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }


        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                swipeDirection = SwipeDetector(startInputPosition, Input.mousePosition);
                OnSwipe?.Invoke(swipeDirection);
            }
        }
#endif

#if UNITY_ANDROID || UNITY_IPHONE

        if(Input.touchCount > 0) 
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startInputPosition = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(startInputPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    OnPressed?.Invoke(hit.transform, Camera.main.ScreenToWorldPoint(startInputPosition));
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    swipeDirection = SwipeDetector(startInputPosition, touch.position);
                    OnSwipe?.Invoke(swipeDirection);
                }
            }
        }
#endif
    }

    private SwipeDirection SwipeDetector(Vector2 inputStartPosition, Vector2 currentInputPosition)
    {
        Vector2 inputPositionDifference = currentInputPosition - inputStartPosition;

        if (inputPositionDifference.magnitude > swipeThreshold)
        {
            if (Mathf.Abs(inputPositionDifference.x) > Mathf.Abs(inputPositionDifference.y))
            {
                if (inputPositionDifference.x < 0)
                    return SwipeDirection.Left;
                else
                    return SwipeDirection.Right;
            }
            else
            {
                if (inputPositionDifference.y < 0)
                    return SwipeDirection.Down;
                else
                    return SwipeDirection.Up;
            }
        }

        return SwipeDirection.None;
    }

}

public enum SwipeDirection
{
    None,
    Left,
    Right,
    Up,
    Down
}