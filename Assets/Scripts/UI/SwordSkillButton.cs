using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.InputSystem.OnScreen;

public class SwordSkillButton : InputButton
{
    [SerializeField] private GameObject swordSkillJoyStick;
    [SerializeField] private GameObject joystickBackground;

    [Header("Attack Button Visual")]
    [SerializeField] private GameObject attackButton;

    private OnScreenStick onScreenStick;

    private SwordSkill swordSkill;
    private TextMeshProUGUI skillText;
    private float defaultOnScreenStickMovementRange;
    private GameInput gameInput;

    private void Start()
    {
        swordSkill = SkillManager.instance.swordSkill;
        skillText = swordSkillJoyStick.GetComponentInChildren<TextMeshProUGUI>();

        onScreenStick = swordSkillJoyStick.GetComponent<OnScreenStick>();
        defaultOnScreenStickMovementRange = onScreenStick.movementRange;

        gameInput = GameInput.instance;
        gameInput.OnSwordSkillPress += GameInput_OnSwordSkillPress;
        gameInput.OnSwordSkillRelease += GameInput_OnSwordSkillRelease;
    }

    private void GameInput_OnSwordSkillRelease(object sender, System.EventArgs e)
    {
        joystickBackground.SetActive(false);
        SetActiveButtons(true);

        if (swordSkill.IsAllSwordThrown())
        {
            onScreenStick.movementRange = 0;
        }
    }

    private void GameInput_OnSwordSkillPress(object sender, System.EventArgs e)
    {
        if (swordSkill.IsAllSwordReturned())
        {
            onScreenStick.movementRange = defaultOnScreenStickMovementRange;
        }

        if (!swordSkill.IsAllSwordThrown())
        {
            joystickBackground.SetActive(true);
            SetActiveButtons(false);
        }
        else
        {
            SetActiveButtons(true);

        }
    }

    private void Update()
    {
        if (swordSkill.IsAllSwordReturned())
        {
            skillText.text = "S";
        }
        else
        {
            skillText.text = "R";
        }
    }


}
