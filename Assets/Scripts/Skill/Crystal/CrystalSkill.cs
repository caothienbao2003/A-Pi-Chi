using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrystalAbility
{
    Basic,
    Mirage,
    Explode,
    MoveTowardsEnemy,
    MultiStack
}

public class CrystalSkill : Skill
{
    [Header("Ability")]
    [SerializeField] private CrystalAbility crystalAbility = CrystalAbility.Basic;

    [Header("Setup Crystal")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;


    [Header("Explode")]
    [SerializeField] private Vector2 explodeKnockbackDir;
    [SerializeField] private float explodeKnockbackForce;

    [Header("Moving Crystal")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectEnemyDistance = 10f;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private int amountOfStack;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float multiStackResetTime;
    [SerializeField] private List<GameObject> crystalList = new List<GameObject>();

    private GameInput gameInput;
    private bool canUseMultiCrystal;
    
    private void Start()
    {
        gameInput = GameInput.instance;
        gameInput.OnCrystalSkillPress += GameInput_OnCrystalSkillPress;

        RefillCrystal();
    }

    private void GameInput_OnCrystalSkillPress(object sender, System.EventArgs e)
    {
        if (CanUseSkill())
        {
            UseSkill();
        }
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);

            CrystalController crystal = currentCrystal.GetComponent<CrystalController>();
            crystal.SetUpCrystal(crystalDuration);
            switch (crystalAbility)
            {
                case CrystalAbility.Mirage:
                    crystal.SetUpMirage();
                    break;
                case CrystalAbility.Explode:
                    crystal.SetUpExplode(explodeKnockbackDir, explodeKnockbackForce);
                    break;
                case CrystalAbility.MoveTowardsEnemy:
                    Transform closestEnemy = FindClosestEnemy(currentCrystal.transform, detectEnemyDistance);
                    crystal.SetUpExplode(explodeKnockbackDir, explodeKnockbackForce);
                    crystal.SetUpMoveTowardEnemy(moveSpeed, closestEnemy);
                    break;
            }
        }
        else
        {
            currentCrystal.GetComponent<CrystalController>().Teleport(player.transform);
        }
    }

    private void RefillCrystal()
    {
        int amountToAdd = amountOfStack - crystalList.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalList.Add(crystalPrefab);
        }
    }

    private bool CanUseMultiCrystal()
    {
        if (crystalAbility == CrystalAbility.MultiStack)
        {
            if (crystalList.Count > 0)
            {
                if(crystalList.Count == amountOfStack)
                {
                    Invoke("ResetMultiStackAbility", multiStackResetTime);
                }
                coolDown = 0;
                ResetCoolDownTimer();

                GameObject crystalToSpawn = crystalList[crystalList.Count - 1];
                GameObject newCrystalObject = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalList.Remove(crystalToSpawn);

                CrystalController newCrystal = newCrystalObject.GetComponent<CrystalController>();
                newCrystal.SetUpCrystal(crystalDuration);
                newCrystal.SetUpExplode(explodeKnockbackDir, explodeKnockbackForce);
                Transform closestEnemy = FindClosestEnemy(newCrystal.transform, detectEnemyDistance);
                newCrystal.SetUpMoveTowardEnemy(moveSpeed, closestEnemy);

                if (crystalList.Count <= 0)
                {
                    coolDown = multiStackCooldown;
                    ResetCoolDownTimer();
                    RefillCrystal();
                }

            }
            return true;

        }

        return false;
    }

    private void ResetMultiStackAbility()
    {
        coolDown = multiStackCooldown;
        ResetCoolDownTimer();
        RefillCrystal();
    }
}
