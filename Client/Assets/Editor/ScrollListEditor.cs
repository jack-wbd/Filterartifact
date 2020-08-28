using Filterartifact;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Introduction: ScrollListEditor
/// Author: 	Cheng
/// Time: 
/// </summary>
[CustomEditor(typeof(ScrollList))]
public class ScrollListEditor : Editor
{
    ScrollList script;

    void OnEnable()
    {
        script = (ScrollList)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("���з�ʽ");
        script.arrangement = (ScrollList.Arrangement)EditorGUILayout.EnumPopup(script.arrangement);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("���뷽ʽ");
        EditorGUILayout.BeginVertical();
        script.verticalAlign = (ScrollList.VerticalAlign)EditorGUILayout.EnumPopup(script.verticalAlign);
        script.horizontalAlign = (ScrollList.HorizontalAlign)EditorGUILayout.EnumPopup(script.horizontalAlign);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("�о�");
        script.rowSpace = EditorGUILayout.FloatField(script.rowSpace);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("�о�");
        script.columuSpace = EditorGUILayout.FloatField(script.columuSpace);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        if (script.arrangement == ScrollList.Arrangement.Horizontal)
            EditorGUILayout.PrefixLabel("ÿ�и���");
        else
            EditorGUILayout.PrefixLabel("ÿ�и���");
        int num = EditorGUILayout.IntField(script.maxPerLine);
        if (num > 0 && num != script.MaxPerLine)
            script.MaxPerLine = num;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("��Ե����");
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("��");
        script.marginTop = EditorGUILayout.FloatField(script.marginTop);
        GUILayout.Label("��");
        script.marginBottom = EditorGUILayout.FloatField(script.marginBottom);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("��");
        script.marginLeft = EditorGUILayout.FloatField(script.marginLeft);
        GUILayout.Label("��");
        script.marginRight = EditorGUILayout.FloatField(script.marginRight);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("�ܸ���");
        script.ChildCount = EditorGUILayout.IntField(script.childCount);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("��ʾ����");
        script.ViewPort = EditorGUILayout.Vector2Field("", script.viewPort);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("�ӽڵ�");
        script.Item = (GameObject)EditorGUILayout.ObjectField(script.item, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
            script.ReBuild();
    }
}