using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
[CanEditMultipleObjects]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var gM = (GameManager)target;

        if (GUILayout.Button("Next Generation"))
        {
            gM.NextGeneration();
        }
    }
}
