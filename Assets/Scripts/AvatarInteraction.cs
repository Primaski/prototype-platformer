using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    /*Necessary placeholders since Unity doesn't like randomly floating Monobehavior scripts.*/
    private TextMeshProUGUI titleForDialogueWindow;
    private TextMeshProUGUI contentForDialogueWindow;

    private void Update() {
        if (DetectAutomaticItem()) {
            item.GetComponent<ItemAutomatic>().Interact();
        }
        if (DetectInteractableItem()) {
            if (Interact()) {
                interactableItem.GetComponent<ItemInteractable>().Interact();
            }
        }
    }

    bool Interact() {
        return Input.GetButtonDown("Submit");
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
        Sprite sprite = (item.customImage == null) ? item.GetComponent<SpriteRenderer>().sprite : item.customImage;
        DisplayItemDialogueWindow(itemName, itemDescription, sprite);
    }

    public void DisplayItemDialogueWindow(string title, string description, Sprite sprite) {
        DialogueFrame frame = new DialogueFrame(title, description, sprite);
        List<DialogueFrame> result = new List<DialogueFrame>() { frame };
        dialogueWindow.GetComponent<DialogueController>().DisplayDialogueFrames(result, DialogueController.Source.ITEM_EXAMINE);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, 0.05f);
    }
}