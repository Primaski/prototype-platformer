using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour{

    /*TODO Finish this class*/

    [Header("Fields")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI content;

    public bool windowIsActive = false;
    public enum Source { NONE, CUTSCENE, ITEM_EXAMINE, DIALOGUE }
    private Source requestSource = Source.NONE; 

    private Image defaultImage;
    private TextMeshProUGUI defaultTitle;
    private TextMeshProUGUI defaultContent;

    public void Awake() {
        defaultImage = image;
        defaultTitle = title;
        defaultContent = content;
    }

    public void FixedUpdate() {
        if (windowIsActive) {
            //if (Interact()) {

            //}
        }
    }

    public void DisplayDialogue(TextMeshPro title, TextMeshPro content, Source requestSource, Sprite image = null) {
        throw new NotImplementedException();
    }

    public bool DisplayDialogueFrames(List<DialogueFrame> frames, Source requestSource) {
        throw new NotImplementedException();
    }

    private void EnableWindow() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
        windowIsActive = true;
    }

    private void DisableWindow() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        windowIsActive = false;
    }
}

public class DialogueFrame {
    public TextMeshProUGUI title = null;
    public TextMeshProUGUI description = null;
    public Image image = null;
}
