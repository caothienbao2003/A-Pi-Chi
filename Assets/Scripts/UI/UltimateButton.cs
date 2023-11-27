using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class UltimateButton : InputButton
{
    private Player player;
    private bool isObjectsActive = true;
    private GameInput gameInput;


    private void Start()
    {
        player = PlayerManager.instance.player;
        gameInput = GameInput.instance;

        gameInput.OnUltimateSkillPress += GameInput_OnUltimateSkillPress;
    }

    private void GameInput_OnUltimateSkillPress(object sender, EventArgs e)
    { 
        isObjectsActive = !isObjectsActive;
        SetActiveButtons(isObjectsActive);
    }
}
