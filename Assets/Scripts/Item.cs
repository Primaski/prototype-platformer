using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Item : MonoBehaviour{

    public enum InteractionType { NONE, PICKUP, EXAMINE, EXAMINEANDPICKUP, EXAMINEANDDESTROY }
    public enum ItemType { NONE, AUTOMATIC, INTERACTABLE }
    public InteractionType interactionType;
    private protected ItemType itemType;

    [Header("Details")]
    public string itemName = "";
    public string descriptionText = "Seems pretty ordinary to me.";
    public Sprite customImage = null;

    [Header("Custom events")]
    public UnityEvent customEvents;

    private void Start() {
        if (this.itemName == "") { this.itemName = gameObject.name; }
        }

    public virtual  
        void Interact() {
        switch (interactionType) {
            case InteractionType.PICKUP:
                Destroy(gameObject);
                break;
            case InteractionType.EXAMINE:
                if(this.itemType == ItemType.AUTOMATIC) {
                    Debug.LogError("Destroying object " + this.itemName + ". It is an automatic item with an examine only attribute. " +
                        "This would result in an uncloseable dialogue box. Please change its Interaction Type.");
                    Destroy(gameObject);
                    return;
                }
                FindObjectOfType<AvatarInteraction>().ExamineItem(this);
                break;
            case InteractionType.EXAMINEANDPICKUP:
                FindObjectOfType<AvatarInteraction>().ExamineItem(this);
                Destroy(gameObject);
                break;
            case InteractionType.EXAMINEANDDESTROY:
                FindObjectOfType<AvatarInteraction>().ExamineItem(this);
                Destroy(gameObject);
                break;
            default:
                Debug.LogWarning(itemName + " has no type assigned.");
                break;
        }
        customEvents.Invoke();
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
