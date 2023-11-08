using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class JoyStick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler 
{
    public JoyStick instance;
    private RectTransform joystickTransform;
    public float dragThreshold=0.6f;
    public int dragMovementDistance=30;
    public int dragOffsetDistance=100;
    public event Action<Vector2> onMove;
    // Start is called before the first frame update


    void Awake()
    {
        if (instance == null)
            instance = this;
        joystickTransform=(RectTransform)transform;
    }

    // Update is called once per frame
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTransform,eventData.position,null,out offset);
        offset=Vector2.ClampMagnitude(offset,dragOffsetDistance)/dragOffsetDistance;
        joystickTransform.anchoredPosition=offset*dragMovementDistance;
        Vector2 inputVector=CalculateMovementInput(offset);
        onMove?.Invoke(inputVector);
    }

    Vector2 CalculateMovementInput(Vector2 offset){
        float x=Mathf.Abs(offset.x)>dragThreshold?offset.x:0;
        float y=Mathf.Abs(offset.y)>dragThreshold?offset.y:0;
        return new Vector2(x,y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickTransform.anchoredPosition=Vector2.zero;
        onMove?.Invoke(Vector2.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }
}
