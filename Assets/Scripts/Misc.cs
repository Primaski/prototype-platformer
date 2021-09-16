using UnityEngine;
using System.Collections;

public class Misc : MonoBehaviour {

    public GameObject UI;
    private bool isShowing;

    private void Awake() {
        UI.SetActive(true);
    }
}