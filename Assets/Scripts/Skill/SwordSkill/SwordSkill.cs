using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{

    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float swordGravity;
    [SerializeField] private float throwForce;
    [SerializeField] private float dontHitTime;
    [SerializeField] private float catchSwordForce;


    [Header("Aim Dots")]
    [SerializeField] private int numberofDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;

    [Header("Return Sword")]
    [SerializeField] private float returnSpeed;

    [Header("Sword Number")]
    [SerializeField] private int maxSwordNumber;

    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceRadius;
    [SerializeField] private float bounceThrowForce;

    [Header("Pierce Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;
    [SerializeField] private float pierceThrowForce;

    [Header("Spin Info")]
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinHitCooldown;
    [SerializeField] private float spinGravity;
    [SerializeField] private float spinThrowForce;

    public int swordCount { get; set; }
    private Vector2 throwDir;
    private List<SwordSkillController> swordList;

    private GameObject[] dots;

    public SwordType swordType = SwordType.Regular;


    private void Start()
    {
        swordCount = 0;
        swordList = new List<SwordSkillController>();

        GenerateDots();
        SetUpSwordWhenStart();
    }

    public void CreateSword()
    {
        throwDir = AimDirection();
        GameObject newSword = GameObject.Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        SwordSkillController swordSkillController = newSword.GetComponent<SwordSkillController>();

        swordList.Add(swordSkillController);
        SetUpSwordType(swordSkillController);

        swordSkillController.SetUpSword(throwDir, throwForce, swordGravity, returnSpeed, player, dontHitTime, catchSwordForce, swordType);

        DotsActive(false);
        swordCount++;
    }

    private void SetUpSwordType(SwordSkillController swordSkillController)
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordSkillController.SetUpBounce(bounceAmount, bounceSpeed, bounceRadius);
                break;
            case SwordType.Pierce:
                swordSkillController.SetUpPierce(pierceAmount);
                break;
            case SwordType.Spin:
                swordSkillController.SetUpSpin(maxTravelDistance, spinDuration, spinHitCooldown);
                break;
            default:
                break;
        }
    }

    private void SetUpSwordWhenStart()
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                throwForce = bounceThrowForce;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                throwForce = pierceThrowForce;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                throwForce = spinThrowForce;
                break;
            default:
                break;
        }
    }

    #region Aim
    private Vector2 AimDirection()
    {
        Vector2 direction = player.aimSwordDirectionInput;
        return direction.normalized;
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberofDots];
        for (int i = 0; i < numberofDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position
            + AimDirection() * throwForce * t
            + 0.5f * (Physics2D.gravity * swordGravity) * t * t;

        return position;
    }

    public void SetupDotsPosition()
    {
        for (int i = 0; i < dots.Length; ++i)
        {
            dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
        }
    }
    #endregion

    #region Utils
    public bool IsAllSwordThrown()
    {
        return swordCount >= maxSwordNumber;
    }

    public bool IsAllSwordReturned()
    {
        return swordCount <= 0;
    }

    public void ReturnAllSword()
    {
        foreach (var sword in swordList)
        {
            sword.ReturnSword();
        }
    }

    public void DestroySword(SwordSkillController sword)
    {
        swordCount--;
        swordList.Remove(sword);
        Destroy(sword.gameObject);
    }
    #endregion
}
