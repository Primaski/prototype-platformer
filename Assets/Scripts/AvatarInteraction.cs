using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvatarInteraction : MonoBehaviour{

    //Point of detection
    [SerializeField] private Transform detectionPoint;
    //Layer for detection
    [SerializeField] private LayerMask automaticDetectionLayer;
    [SerializeField] private LayerMask interactableDetectionLayer;
    [SerializeField] private GameObject dialogueWindow;
    private const float detectionRadius = 0.2f;
    private GameObject item;
    private GameObject interactableItem;
    bool activatedDialogueWindow = false;

    private void Update() {
        if (DetectAutomaticItem()) {
            item.GetComponent<ItemAutomatic>().Interact();
        }
        if (DetectInteractableItem()) {
            if (Interact()) {
                interactableItem.GetComponent<ItemInteractable>().Interact();
                ExamineItem(interactableItem.GetComponent<Item>());
            }
        }
    }

    bool Interact() {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectAutomaticItem() {
        Collider2D detected = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, automaticDetectionLayer);
        if (detected != null) {
            item = detected.gameObject;
            return true;
        }
        return false;
    }

    bool DetectInteractableItem() {
        Collider2D detected = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, interactableDetectionLayer);
        if (detected != null) {
            interactableItem = detected.gameObject;
            return true;
        }
        return false;
    }

    public void ExamineItem(Item item) {
        string itemName = item.itemName;
        string itemDescription = item.descriptionText;
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
        DisplayItemDialogueWindow(itemName, itemDescription, sprite);
    }

    public void DisplayItemDialogueWindow(string title, string description, Sprite sprite) {
        //dialogueWindow.GetComponent<DialogueController>().DisplayDialogue(title, description, sprite, DialogueController.Source.ITEM_EXAMINE);
        activatedDialogueWindow = true;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, 0.05f);
    }
}