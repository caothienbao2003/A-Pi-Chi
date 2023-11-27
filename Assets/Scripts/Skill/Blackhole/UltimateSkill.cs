using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkill : Skill
{
    [Header("Player info")]
    public float flyTime;
    public float flySpeed;

    [Header("Prefab")]
    [SerializeField] private GameObject blackholePrefab;

    [Header("Setup Blackhole")]
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float skrinkSpeed;
    public float blackholeExistTime;
    [SerializeField] private GameObject targetIndicatorPrefap;

    [Header("Attack Info")]
    [SerializeField] private int amountOfAttack = 4;
    [SerializeField] private float cloneAttackCooldown;

    private Blackhole currentBlackhole;
    private bool isCancelBlackhole = false;
    private GameInput gameInput;

    private void Start()
    {
        gameInput = GameInput.instance;
        gameInput.OnUltimateSkillPress += GameInput_OnUltimateSkillPress;
    }

    private void GameInput_OnUltimateSkillPress(object sender, System.EventArgs e)
    {
        if (isCancelBlackhole)
        {
            CancelBlackhole();
        }
        isCancelBlackhole = !isCancelBlackhole;
        Debug.Log(isCancelBlackhole);

    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentBlackhole == null)
        {
            GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

            currentBlackhole = newBlackhole.GetComponent<Blackhole>();

            currentBlackhole.SetUpBlackHole(maxSize, growSpeed, skrinkSpeed,
                blackholeExistTime, targetIndicatorPrefap, flyTime);
        }
    }

    public bool HasExitBlackhole()
    {
        if (currentBlackhole == null)
        {
            return false;
        }

        if (currentBlackhole.canExitBlackhole)
        {
            currentBlackhole.SkrinkBlackhole();
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    private void CancelBlackhole()
    {
        Debug.Log("Cancel");
        if (currentBlackhole != null)
        {
            currentBlackhole.SkrinkBlackhole();
            currentBlackhole.canExitBlackhole = false;
            currentBlackhole = null;
        }
    }
}
