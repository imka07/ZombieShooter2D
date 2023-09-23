﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private GameObject activeObject;
    [SerializeField] private Canvas canvas;
    private Camera _mainCamera;
    public delegate void OnDragEndHandler(Vector3 screenPosition);
    public OnDragEndHandler OnDragEnd;
    private Test test;
    public AudioSource failed;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        test = FindObjectOfType<Test>();
        _mainCamera = Camera.main;
    }

    public void OnDown(BaseEventData data)
    {
        if(test.tntCount > 0)
        {
           
            PointerEventData pointerData = (PointerEventData)data;
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerData.position, canvas.worldCamera, out position);

            activeObject.transform.position = canvas.transform.TransformPoint(position);

            activeObject.SetActive(true);
        }
        else
        {
            anim.SetTrigger("Error");
            failed.Play();
        }
    }

    public void OnUp(BaseEventData data)
    {
       
        PointerEventData pointerData = (PointerEventData)data;
        activeObject.SetActive(false);
        OnDragEnd?.Invoke(pointerData.position);
    }

    public void OnDrag(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerData.position, canvas.worldCamera, out position);

        activeObject.transform.position = canvas.transform.TransformPoint(position);
    }
}
