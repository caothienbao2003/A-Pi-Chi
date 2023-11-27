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
    private UltimateSkill ultimateSkill;


    private void Start()
    {
        player = PlayerManager.instance.player;
        gameInput = GameInput.instance;
        ultimateSkill = SkillManager.instance.ultimateSkill;
    }

    private void Update()
    {
        if(ultimateSkill.IsUsingSkill())
        {
            SetActiveButtons(false);
        }
        else
        {
            SetActiveButtons(true);
        }
    }
}
