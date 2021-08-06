using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarInteraction : MonoBehaviour{

    //Point of detection
    [SerializeField] private Transform detectionPoint;
    //Layer for detection
    [SerializeField] private LayerMask automaticDetectionLayer;
    [SerializeField] private LayerMask interactableDetectionLayer;
    private const float detectionRadius = 0.2f;
    private GameObject item;
    private GameObject interactableItem;

    private void Update() {
        if (DetectAutomaticObject()) {
            item.GetComponent<ItemAutomatic>().Interact();
        }
        if (DetectInteractableObject()) {
            if (Interact()) {
                interactableItem.GetComponent<ItemInteractable>().Interact();
            }
        }
    }

    bool Interact() {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectAutomaticObject() {
        Collider2D detected = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, automaticDetectionLayer);
        if (detected != null) {
            item = detected.gameObject;
            return true;
        }
        return false;
    }

    bool DetectInteractableObject() {
        Collider2D detected = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, interactableDetectionLayer);
        if (detected != null) {
            interactableItem = detected.gameObject;
            return true;
        }
        return false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, 0.05f);
    }
}