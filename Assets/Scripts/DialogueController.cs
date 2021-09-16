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
    List<DialogueFrame> frames = null; //frames to display, populated via public DisplayDialogueFrames()
    private int activeFrameNumber = 0;
    private int totalFrames;

    public void Awake() {
        defaultImage = image;
        defaultTitle = title;
        defaultContent = content;
        DisableWindow();
    }

    public void Update() {
        if (windowIsActive) {
            if (Interact()) {
                if(activeFrameNumber < totalFrames - 1) {
                    //more frames to display, on with the show!
                    activeFrameNumber++;
                    DisplayFrame(frames[activeFrameNumber]);
                } else {
                    //the show has come to a conclusion
                    DisableWindow();
                    ResetToDefaults();
                }
            }
        }
    }

    public void DisplayDialogueFrames(List<DialogueFrame> frames, Source requestSource) {
        if (frames == null || frames.Count < 1) {
            throw new Exception("Must pass at least one frame to the Dialogue Controller.");
        }
        this.requestSource = requestSource;
        this.frames = frames;
        DisplayFrame(frames[0]);
        EnableWindow();
    }

    private void DisplayFrame(DialogueFrame frame) {
        this.title.text = frame.title;
        this.content.text = frame.content;
        if(frame.image != null) {
            this.image = frame.image;
            return;
        }
        if(frame.sprite != null) {
            //if image is NULL, then it will default to using a sprite. override in DialogueFrame allows for either to be null.
            Image temp = defaultImage;
            temp.overrideSprite = frame.sprite;
            this.image = temp;
            return;
        }
        this.image = defaultImage;
    }

    private void EnableWindow() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
        windowIsActive = true;
        DisableInputBoundClasses();
    }

    private void DisableWindow() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        windowIsActive = false;
        EnableInputBoundClasses();
    }

    private void ResetToDefaults() {
        this.image = defaultImage;
        this.title = defaultTitle;
        this.content = defaultContent;
        frames = null;
        activeFrameNumber = 0;
        requestSource = Source.NONE;
    }

    /* Classes like AvatarInteraction like to "share" inputs with DialogueController. Pressing submit to conclude the
     * dialogue, but that submit press also registering in AvatarInteraction will result in the dialogue window
     * being called up again, due to the speed at which Unity executes these commands, and the fact that a single
     * class cannot "absorb" an input. As such, disable any classes that utilize the "Submit" button here. */
    private void DisableInputBoundClasses() {
        FindObjectOfType<AvatarInteraction>().enabled = false;
    }

    private void EnableInputBoundClasses() {
        FindObjectOfType<AvatarInteraction>().enabled = true;
    }

    private bool Interact() {
        return Input.GetButtonDown("Submit");
    }
}

public class DialogueFrame {
    public string title { get; private set; } = null;
    public string content { get; private set; } = null;
    public Image image { get; private set; } = null; //takes priority, if null, defaults to sprite
    public Sprite sprite { get; private set; } = null;


    public DialogueFrame(string title, string content, Image image = null) {
        this.title = title ?? "";
        this.content = content ?? "";
        this.image = image;
    }

    public DialogueFrame(string title, string content, Sprite image = null) {
        this.title = title ?? "";
        this.content = content ?? "";
        this.sprite = image;
    }
}
