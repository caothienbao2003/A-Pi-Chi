using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utilities
{
    public static void IgnoreLayerCollisionBetween(LayerMask layerMask1, LayerMask layerMask2, bool active)
    {
        int layer1 = (int) Mathf.Log(layerMask1.value, 2);
        int layer2 = (int) Mathf.Log(layerMask2.value, 2);

        Physics2D.IgnoreLayerCollision(layer1, layer2, active);
    }

    public static void SetActiveMultipleObjects(List<GameObject> gameObjects, bool active)
    {
        foreach (GameObject obj in gameObjects)
        {
            if(obj.activeSelf != active)
            {
                obj.SetActive(active);
            }
        }
    }

    public static void ChangeImageColor(GameObject image, Color color)
    {
        image.GetComponent<Image>().color = color;
    }

    public static void ClampVelocity(Rigidbody2D rb, float min, float max)
    {
        Vector2 clampedVelocity = rb.velocity;
        clampedVelocity.y = Mathf.Clamp(rb.velocity.y, min, max);
        rb.velocity = clampedVelocity;
    }
}
