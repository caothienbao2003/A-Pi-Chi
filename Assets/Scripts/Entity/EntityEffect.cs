using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEffect : MonoBehaviour
{
    private Material original;
    [SerializeField] private Material hitEffect;
    [SerializeField] private float effectTime;

    private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

    private void Start()
    {
        original = spriteRenderer.material;
    }

    private IEnumerator HitEffectCo()
    {
        spriteRenderer.material = hitEffect;
        yield return new WaitForSeconds(effectTime);
        spriteRenderer.material = original;
    }

    public void HitEffect()
    {
        StartCoroutine(HitEffectCo());
    }
}
