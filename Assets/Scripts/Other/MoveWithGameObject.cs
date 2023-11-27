using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithGameObject : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothSpeed;

    private Vector2 velocity = Vector2.zero;
    private Camera mainCamera;

    private void LateUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, (Vector2) target.transform.position, smoothSpeed * Time.deltaTime);
    }
}
