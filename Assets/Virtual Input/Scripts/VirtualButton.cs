using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event EventHandler ButtonDownHandler;

    [SerializeField] private float rotationLimit = 40;
    [SerializeField] private float rotationSpeed = 15;
    [SerializeField] private Color colorWhenPressed;

    private Image image;
    private Color defaultColor;
    private bool rotate = false;

    private void Start()
    {
        image = GetComponent<Image>();
        defaultColor = image.color;
    }

    void FixedUpdate()
    {
        float targetRotate = rotate ? rotationLimit : 0f;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(targetRotate, 0, 0);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotationSpeed);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        rotate = true;
        image.color = colorWhenPressed;

        ButtonDownHandler?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        rotate = false;
        image.color = defaultColor;
    }
}
