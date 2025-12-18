using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FloatingCoinToast))]
public class FloatingCoinToastEditor : Editor
{
    private int testAmount = 20;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FloatingCoinToast popup = (FloatingCoinToast)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);

        if (GUILayout.Button("Play Preview"))
        {
            popup.EditorPreview();
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Test AddCoins", EditorStyles.boldLabel);

        // Amount field
        testAmount = EditorGUILayout.IntField("Amount", testAmount);

        if (GUILayout.Button("Add Coins Popup"))
        {
            popup.AddCoins(testAmount);
        }
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox(
                "Preview runs even outside Play Mode, but movement may not animate fully because Time.deltaTime doesn't update normally.",
                MessageType.Info);
        }
    }
}
