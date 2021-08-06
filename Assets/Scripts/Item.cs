using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Item : MonoBehaviour{


    public InteractionType itemType;
    public enum InteractionType { NONE, PICKUP, EXAMINE, EXAMINEANDPICKUP }


    public virtual void Interact() {
        switch (itemType) {
            case InteractionType.PICKUP:
            Debug.Log("Okay, I'll pick it up!");
            Destroy(gameObject);
            break;
            case InteractionType.EXAMINE:
            Debug.Log("Okay, I'll examine it!");
            Destroy(gameObject);
            break;
            case InteractionType.EXAMINEANDPICKUP:
            Debug.Log("Okay, I'll examine it, then give the choice of picking it up!");
            Destroy(gameObject);
            break;
            default:
            Debug.Log("Item has no type assigned.");
            break;
        }
    }

    private void Reset() {
        hideFlags = HideFlags.HideInHierarchy;
        hideFlags = HideFlags.HideInInspector;
        if(this.GetType() == typeof(Item)) {
            Debug.LogError("Item is a superclass of ItemAutomatic and ItemInteractable. The Avatar interaction system " +
                "will not know how\nto behave if it's unsure of what kind of item this Gameobject is. Instead, please add ItemAutomatic " +
                "or ItemInteractable.");
            DestroyImmediate(this);
        }
    }
}
