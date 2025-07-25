using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CustomShapedFirework))]
public class ShapedFireworkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update Shape"))
        {
            CustomShapedFirework t = target as CustomShapedFirework;
            t.UpdateChildObjects();
        }
    }
}
