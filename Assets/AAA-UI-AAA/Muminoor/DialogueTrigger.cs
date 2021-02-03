using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {   // for the startGamebUtton.
    public AudioSource start_Sound;
    public void Start() {
        start_Sound = GetComponent<AudioSource>();
    }
    public Dialogue dialogue;
    public void TriggerDialogue()
    {
        start_Sound.Play();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
