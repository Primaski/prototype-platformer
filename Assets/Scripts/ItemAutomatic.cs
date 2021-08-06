using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAutomatic : Item {

    private void Reset() {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 6;
    }

    /*public override void Interact() {

    }
    */
}
