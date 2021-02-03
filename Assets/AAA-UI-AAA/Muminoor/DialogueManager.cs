using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;   // where the Tank Mania text is displayed in the UI.
    public Text dialogueText; // where the story line is displayed in the UI
    public Animator animator;
    public Queue<string> sentences;
    public AudioSource continue_Sound; //sound for the audio
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        continue_Sound = GetComponent<AudioSource>();
    }
    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        continue_Sound.Play();
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            yield return new WaitForSeconds(0.02f);
            dialogueText.text += letter;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
    }
}
