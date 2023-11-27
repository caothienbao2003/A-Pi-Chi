using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blackhole : MonoBehaviour
{
    private enum BlackholeState
    {
        Grow,
        Skrink
    }

    private GameInput gameInput;

    private float maxSize;
    private float growSpeed;
    private float skrinkSpeed;
    private float blackholeExistTime;
    private GameObject targetIndicatorPrefap;

    private float blackholeTimer;

    private List<Transform> enemyList = new List<Transform>();
    private Dictionary<Enemy, GameObject> indicatorDictionary = new Dictionary<Enemy, GameObject>();

    private BlackholeState state;
    public bool canExitBlackhole { get; set; } = false;

    private void Start()
    {
        blackholeTimer = blackholeExistTime;

        state = BlackholeState.Grow;
    }

    private void OnEnable()
    {
        gameInput = GameInput.instance;
        gameInput.OnTouchScreen += GameInput_OnTouchScreen;
    }

    private void OnDisable()
    {
        gameInput.OnTouchScreen -= GameInput_OnTouchScreen;
    }

    private void GameInput_OnTouchScreen(object sender, System.EventArgs e)
    {
        GameObject touchedObject = gameInput.GetTouchedGameObject(GameManager.instance.enemyLayer);

        if (touchedObject != null && touchedObject.CompareTag("Indicator"))
        {
            if (touchedObject.GetComponentInParent<Enemy>() != null)
            {
                Enemy enemy = touchedObject.GetComponentInParent<Enemy>();
                if (enemy.isEffectedByUltimate)
                {
                    float randomOffset = Random.Range(0, 2) == 0 ? 1 : -1;
                    SkillManager.instance.cloneSkill.CreateClone(enemy.transform, new Vector3(randomOffset, 0));
                }
            }
        }
    }

    public void SetUpBlackHole(float maxSize, float growSpeed, float skrinkSpeed,
        float blackHoleExistTime, GameObject targetIndicatorPrefap, float flyTime)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.skrinkSpeed = skrinkSpeed;
        this.blackholeExistTime = blackHoleExistTime;
        this.targetIndicatorPrefap = targetIndicatorPrefap;
    }

    private void Update()
    {
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer <= 0)
        {
            state = BlackholeState.Skrink;
        }

        HandleGrowAndSkrink();
    }

    private void HandleGrowAndSkrink()
    {
        switch (state)
        {
            case BlackholeState.Grow:
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
                break;
            case BlackholeState.Skrink:
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), skrinkSpeed * Time.deltaTime);
                if (transform.localScale.x <= 0.1f)
                {
                    canExitBlackhole = true;
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.FreezeTime(true);

            enemy.isEffectedByUltimate = true;

            enemyList.Add(enemy.transform);

            GameObject indicator = Instantiate(targetIndicatorPrefap, enemy.transform.position, Quaternion.identity, enemy.transform);
            indicatorDictionary.Add(enemy, indicator);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.FreezeTime(false);
            enemy.isEffectedByUltimate = false;

            Destroy(indicatorDictionary[enemy]);
            indicatorDictionary.Remove(enemy);
        }
    }

    public void AddEnemyToList(Transform enemyTransform)
    {
        enemyList.Add(enemyTransform);
    }

    public void SkrinkBlackhole()
    {
        Debug.Log("Skrink");
        state = BlackholeState.Skrink;
    }
}
