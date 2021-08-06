using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class ItemInteractable : Item {

    private void Reset() {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 7;
    }
}
