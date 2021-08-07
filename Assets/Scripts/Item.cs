using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Item : MonoBehaviour{


    public InteractionType itemType;
    public enum InteractionType { NONE, PICKUP, EXAMINE, EXAMINEANDPICKUP }

    [Header("Details")]
    public string itemName;
    public string descriptionText;
    public Sprite image;


    public virtual void Interact() {
        switch (itemType) {
            case InteractionType.PICKUP:
            Destroy(gameObject);
            break;
            case InteractionType.EXAMINE:
            Destroy(gameObject);
            break;
            case InteractionType.EXAMINEANDPICKUP:
            Destroy(gameObject);
            break;
            default:
            Debug.LogWarning("Item has no type assigned.");
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
