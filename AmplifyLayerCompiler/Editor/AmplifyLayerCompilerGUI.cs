using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class AmplifyShaderEditorGUI : ShaderGUI
{
    static GUIStyle headingStyle;
    bool initialized = false;

    public void AutoInit()
    {
        if (initialized) {
            return;
        }

        headingStyle = new GUIStyle(GUI.skin.box);
        headingStyle.normal.textColor = Color.white;

        initialized = true;
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        AutoInit();

        var material = materialEditor.target as Material;
        var materials = materialEditor.targets;

        if (materials.Length > 1) {
            Debug.Log("Doesn't work on multiple selections yet");
            return;
        }

        for (var i = 0; i < 4; i++) {
            DrawLayer(material, materialEditor, props, i);
        }
    }

    private void DrawLayer(Material material, MaterialEditor materialEditor, MaterialProperty[] props, int index)
    {
        string layerNum = (index == 0) ? "" : index.ToString();
        string layerName = (index == 0) ? "Amplify Main Layer" : "Amplify Layer " + index.ToString();

        DrawSectionToggle(material, layerName, "_ENABLELAYER" + layerNum);
        materialEditor.TexturePropertySingleLine(new GUIContent("Albedo"), FindProperty("_BaseMap" + layerNum, props));
        materialEditor.TextureScaleOffsetProperty(FindProperty("_BaseMap" + layerNum, props));
        materialEditor.ShaderProperty(FindProperty("_Opacity" + layerNum, props), "Opacity");
    }

    public void DrawSectionToggle(Material material, string title, string keyword)
    {
        EditorGUILayout.BeginHorizontal(headingStyle);

        var oldState = material.IsKeywordEnabled(keyword);
        var newState = EditorGUILayout.Toggle(oldState, GUILayout.Width(18));
        if (newState != oldState) {
            if (newState) {
                Debug.Log("Enabling keyword " + keyword);
                material.EnableKeyword(keyword);
            } else {
                Debug.Log("Disabling keyword " + keyword);
                material.DisableKeyword(keyword);
            }
            EditorUtility.SetDirty(material);
        }

        GUILayout.Label(title, headingStyle, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(20) });
        EditorGUILayout.EndHorizontal();
    }
}
