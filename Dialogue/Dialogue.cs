using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue")]
public class Dialogue : ScriptableObject
{
    public bool hasOptions;
    // if it doesn't have options
    public string speaker;
    public Texture speakerSprite;
    [TextArea(3, 10)] public string dialogueText;
    // if it does have options
    public Option [] options;
    
    public bool lastDialogue;
    // if not last dialogue
    public Dialogue nextDialogue;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Dialogue))] public class DialogueEditor : Editor 
{
    private SerializedProperty options;
    private SerializedProperty dialogueTextArea;
    private SerializedProperty hasOptions;

    private void OnEnable()
    {
        options = serializedObject.FindProperty("options");
        dialogueTextArea = serializedObject.FindProperty("dialogueText");
        hasOptions = serializedObject.FindProperty("hasOptions");
    }

    public override void OnInspectorGUI() 
    {
        Dialogue dialogue = target as Dialogue;

        Debug.Log(dialogue.hasOptions);

        EditorGUILayout.PropertyField(hasOptions);

        Debug.Log(dialogue.hasOptions);

        if(dialogue.hasOptions == true)
        {
            EditorGUILayout.PropertyField(options);
        } else 
        {
            dialogue.speaker = EditorGUILayout.TextField("Speaker Name:", dialogue.speaker);
            dialogue.speakerSprite = EditorGUILayout.ObjectField("Speaker Sprite:", dialogue.speakerSprite, typeof(Texture), true) as Texture;
            EditorGUILayout.PropertyField(dialogueTextArea);
            
            GUILayout.Space(20);

            dialogue.lastDialogue = GUILayout.Toggle(dialogue.lastDialogue, "Is This The Final Dialogue?");

            if(!dialogue.lastDialogue)
            {
                dialogue.nextDialogue = EditorGUILayout.ObjectField("Next Dialogue:", dialogue.nextDialogue, typeof(Dialogue), true) as Dialogue;
            }
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(serializedObject.targetObject);
    }
}
#endif
