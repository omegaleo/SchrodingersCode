using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager), true)]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var gm = (GameManager) target;

        if (GUILayout.Button("Toggle ScanLines"))
        {
            gm.ToggleScanLines();
        }
        
        base.OnInspectorGUI();
    }
}