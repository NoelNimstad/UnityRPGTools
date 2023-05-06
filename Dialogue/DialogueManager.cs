// NOT OPTIMISED
// though feel free to optimise it

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueGroup;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private RawImage speakerSprite;
    [SerializeField] private GameObject dialogueIsDoneIndicator;
    [SerializeField] private GameObject optionsGroup;
    [SerializeField] private TextMeshProUGUI [] options;
    [SerializeField] private GameObject arrowSelector;
    [SerializeField] private float showCheckSpeed = 0.25f;
    private Dialogue currentDialogue;
    public bool dialogueIsActive = false;
    private bool isDisplayingOptions = false;
    private string currentText;
    private int currentTextIndex = -1;
    private int currentlySelectedOption = 0;
    private char [] punctuation = { '!', '.', ',', '?', ':', ';' };

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;

        dialogueIsActive = true;

        DisplayCorrectGroup(currentDialogue.hasOptions);

        if(currentDialogue.hasOptions)
        {
            isDisplayingOptions = true;
            DisplayOptions();
        } else 
        {
            DisplayDialogue();
        }
    }

    private void DisplayCorrectGroup(bool hasOptions)
    {
        if(hasOptions)
        {
            optionsGroup.SetActive(true);
            dialogueGroup.SetActive(false);
        } else 
        {
            optionsGroup.SetActive(false);
            dialogueGroup.SetActive(true);
        }
    }

    private void DisplayDialogue()
    {
        isDisplayingOptions = false;
        currentTextIndex = -1;
        currentText = "";
        dialogueIsActive = true;
        dialogueIsDoneIndicator.SetActive(false);
        speakerName.text = currentDialogue.speaker;
        speakerSprite.texture = currentDialogue.speakerSprite;
        StartCoroutine(Typewrite());
    }

    private IEnumerator Typewrite()
    {
        currentTextIndex++;

        char targetChar = currentDialogue.dialogueText[currentTextIndex];

        currentText += targetChar;
        dialogueText.text = currentText;

        if(System.Array.IndexOf(punctuation, targetChar) > -1)
        {
            yield return new WaitForSeconds(0.4f);
        } else 
        {
            if(Input.GetKey(KeyCode.C))
            {
                yield return new WaitForSeconds(0.005f);
            } else yield return new WaitForSeconds(0.02f);
        }

        if(currentText == currentDialogue.dialogueText)
        {
            yield return new WaitForSeconds(showCheckSpeed);
            dialogueIsDoneIndicator.SetActive(true);
        } else 
        {
            StartCoroutine(Typewrite());
        }
    }

    private void DisplayOptions() 
    {
        isDisplayingOptions = true;

        for(int i = 0; i < 4; i++)
        {
            if(i < currentDialogue.options.Length)
            {
                options[i].text = currentDialogue.options[i].optionText;
            } else 
            {
                options[i].text = "";
            }
        }
    }

    private void Update()
    {
        if(dialogueIsActive)
        {
            if(isDisplayingOptions)
            {
                Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if(direction != Vector2.zero)
                {
                    for(int i = 0; i < options.Length; i++)
                    {
                        options[i].color = Color.gray;
                    }

                    if(direction.x == -1 && currentDialogue.options.Length > 0)
                    {
                        arrowSelector.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                        options[0].color = Color.white;
                        currentlySelectedOption = 0;
                    } else if(direction.x == 1 && currentDialogue.options.Length > 1)
                    {
                        arrowSelector.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90f));
                        options[1].color = Color.white;
                        currentlySelectedOption = 1;
                    } else if(direction.y == -1 && currentDialogue.options.Length > 2)
                    {
                        arrowSelector.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f));
                        options[2].color = Color.white;
                        currentlySelectedOption = 2;
                    } else if(direction.y == 1 && currentDialogue.options.Length > 3)
                    {
                        arrowSelector.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
                        options[3].color = Color.white;
                        currentlySelectedOption = 3;
                    }
                }

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    currentDialogue = currentDialogue.options[currentlySelectedOption].branchDialogue;
                    DisplayCorrectGroup(currentDialogue.hasOptions);

                    if(currentDialogue.hasOptions)
                    {
                        DisplayOptions();
                    } else 
                    {
                        DisplayDialogue();
                    }
                }
            } else 
            {
                if(Input.GetKeyDown(KeyCode.Space) && dialogueIsDoneIndicator.activeInHierarchy)
                {
                    if(currentDialogue.lastDialogue) 
                    {
                        dialogueIsActive = false;
                        dialogueGroup.SetActive(false);
                        return;
                    }

                    currentDialogue = currentDialogue.nextDialogue;
                    DisplayCorrectGroup(currentDialogue.hasOptions);

                    if(currentDialogue.hasOptions)
                    {
                        DisplayOptions();
                    } else 
                    {
                        DisplayDialogue();
                    }
                }
            }
        }
    }
}
