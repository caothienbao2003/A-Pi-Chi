using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : EntityAnimationTrigger
{
    private void CanBeStuned()
    {
        entity.GetComponent<Enemy>().CanBeStuned();
    }

    private void CannotBeStunned()
    {
        entity.GetComponent<Enemy>().CannotBeStuned();
    }

    private void SelfDestroy()
    {
        Destroy(entity.gameObject);
    }
}
