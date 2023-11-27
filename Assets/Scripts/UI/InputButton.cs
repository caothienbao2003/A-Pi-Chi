using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputButton : MonoBehaviour
{
    [SerializeField] protected List<GameObject> gameObjectsToSetActive;

    //Do not use in update
    protected void SetActiveButtons(bool active)
    {
        Utilities.SetActiveMultipleObjects(gameObjectsToSetActive, active);
    }
}
