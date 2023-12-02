using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance { get; private set; }
    public SwordSkillButton swordSkillButton { get; private set; }
    [SerializeField] protected CanvasGroup basicButtonsCanvasGroup;
    [SerializeField] protected CanvasGroup ultimateButtonCanvasGroup;
    [SerializeField] protected CanvasGroup swordSkillButtonCanvasGroup;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }    
        else
        {
            Destroy(instance.gameObject);
        }
    }

    private void Start()
    {
        swordSkillButton = GetComponent<SwordSkillButton>();
    }

    //Do not use in update
    public void EnableButton(CanvasGroup buttonCanvasGroup)
    {
        buttonCanvasGroup.alpha = 1;
        buttonCanvasGroup.blocksRaycasts = true;
        buttonCanvasGroup.interactable = true;
    }

    public void DisableButton(CanvasGroup buttonCanvasGroup)
    {
        buttonCanvasGroup.alpha = 0;
        buttonCanvasGroup.blocksRaycasts = false;
        buttonCanvasGroup.interactable = false;
    }

    public void EnableAll()
    {
        EnableButton(basicButtonsCanvasGroup);
        EnableButton(ultimateButtonCanvasGroup);
        EnableButton(swordSkillButtonCanvasGroup);
    }

    public void DisableAll()
    {
        DisableButton(basicButtonsCanvasGroup);
        DisableButton(ultimateButtonCanvasGroup);
        DisableButton(swordSkillButtonCanvasGroup);
    }

    public void HoldingSwordSkillButton()
    {
        EnableButton(swordSkillButtonCanvasGroup);
        DisableButton(basicButtonsCanvasGroup);
        DisableButton(ultimateButtonCanvasGroup);
    }

    public void HoldingUltimateButton()
    {
        EnableButton(ultimateButtonCanvasGroup);
        DisableButton(swordSkillButtonCanvasGroup);
        DisableButton(basicButtonsCanvasGroup);
    }
}
