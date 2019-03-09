//============================================
//
//Copyright (C) Bathur Lu All rights reserved.
//
//Author:   Bathur Lu
//Date:     2019.3.9
//Website:  http://bathur.cn/
//
//============================================

using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SingleGifPlayer))]
public class GifPlayerEditorExtension : Editor
{
    #region Member Variables
    private SerializedObject gifPlayer;
    private SerializedProperty
        gifPath,
        renderMode,
        targetImage,
        targetRenderer;
    #endregion

    void OnEnable()
    {
        gifPlayer = new SerializedObject(target);
        gifPath = gifPlayer.FindProperty("gifPath");
        renderMode = gifPlayer.FindProperty("renderMode");
        targetImage = gifPlayer.FindProperty("targetImage");
        targetRenderer = gifPlayer.FindProperty("targetRenderer");
    }

    public override void OnInspectorGUI()
    {
        gifPlayer.Update();
        EditorGUILayout.HelpBox(
            "Please input the GIF file path which under the StreamingAssets folder.\n" +
            "You do not need to include the StreamingAssets folder path in the input.", MessageType.Info);
        EditorGUILayout.PropertyField(gifPath);
        EditorGUILayout.PropertyField(renderMode);
        switch (renderMode.enumValueIndex)
        {
            case 0:
                EditorGUILayout.PropertyField(targetImage);
                break;
            case 1:
                EditorGUILayout.PropertyField(targetRenderer);
                break;
        }

        foreach (var obj in targets)
        {
            SingleGifPlayer player = obj as SingleGifPlayer;
            player.gifPath = gifPath.stringValue;
            player.renderMode = (SingleGifPlayer.RenderMode)renderMode.enumValueIndex;
            EditorUtility.SetDirty(obj);
        }

        gifPlayer.ApplyModifiedProperties();
    }
}
