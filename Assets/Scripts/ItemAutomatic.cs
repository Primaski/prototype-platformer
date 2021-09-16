using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAutomatic : Item {

    private void Start() {
        this.itemType = ItemType.AUTOMATIC;
        if(this.interactionType == InteractionType.EXAMINE) {
            Debug.LogError("Destroying object " + this.itemName + ". It is an automatic item with an examine only attribute. " +
                "This would result in an uncloseable dialogue box. Please change its Interaction Type.");
            Destroy(gameObject);
        }
    }
    private void Reset() {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 6;
    }

    /*public override void Interact() {

    }
    */
}
