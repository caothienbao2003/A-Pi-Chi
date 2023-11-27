using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    public LayerMask swordLayer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        Cursor.visible = false;
    }
}
