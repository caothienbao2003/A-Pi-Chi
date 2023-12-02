using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.OnScreen;

public class SwordSkillButton : InputButton
{
    [SerializeField] private GameObject swordSkillJoyStick;
    [SerializeField] private GameObject joystickBackground;

    [Header("Attack Button Visual")]
    [SerializeField] private GameObject attackButton;
    [SerializeField] private Image swordSkillIcon;
    [SerializeField] private Sprite throwSwordSprite;
    [SerializeField] private Sprite returnSwordSprite;
    [SerializeField] private GameObject arrowIcons;

    private OnScreenStick onScreenStick;

    private SwordSkill swordSkill;

    private float defaultOnScreenStickMovementRange;
    private GameInput gameInput;

    private void Start()
    {
        swordSkill = SkillManager.instance.swordSkill;

        onScreenStick = swordSkillJoyStick.GetComponent<OnScreenStick>();
        defaultOnScreenStickMovementRange = onScreenStick.movementRange;

        gameInput = GameInput.instance;
        gameInput.OnSwordSkillPress += GameInput_OnSwordSkillPress;
        gameInput.OnSwordSkillRelease += GameInput_OnSwordSkillRelease;
    }

    private void GameInput_OnSwordSkillRelease(object sender, System.EventArgs e)
    {
        joystickBackground.SetActive(false);
    }

    private void GameInput_OnSwordSkillPress(object sender, System.EventArgs e)
    {
        arrowIcons.SetActive(false);

        if (!swordSkill.IsAllSwordThrown())
        {
            joystickBackground.SetActive(true);
        }
    }

    private void Update()
    {
        if (swordSkill.IsAllSwordReturned())
        {
            swordSkillIcon.sprite = throwSwordSprite;
        }
        if (swordSkill.IsAllSwordThrown())
        {
            swordSkillIcon.sprite = returnSwordSprite;
        }
    }

    public void FreezeStick(bool freeze)
    {
        if(freeze)
        {
            onScreenStick.movementRange = 0;
        }
        else
        {
            onScreenStick.movementRange = defaultOnScreenStickMovementRange;
        }
    }

    public void SetActiveArrowIcons(bool active)
    {
        arrowIcons.SetActive(active);
    }


}
