using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.UIElements;


[CustomEditor(typeof(GraffitiSystem))]
public class GraffitiSystemCustomInspector : Editor {

    GraffitiSystem GS;
    bool showPosition = false;
    void OnEnable(){
        GS = target as GraffitiSystem;

    }

    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        showPosition = EditorGUILayout.BeginFoldoutHeaderGroup(showPosition, "SkillCommands");
        for (int i = 0; i < GS.skillList.Count; i++)
        {
            if (showPosition && Selection.activeTransform)
            {
                EditorGUILayout.LabelField("skill"+i);
                float maxX = 0;
                for (int j = 0; j < GS.skillList[i].skillCommand.Count; j++)
                    if (maxX < GS.skillList[i].skillCommand[j].x)
                        maxX = GS.skillList[i].skillCommand[j].x;
                float maxY = 0;
                for (int j = 0; j < GS.skillList[i].skillCommand.Count; j++)
                    if (maxY < GS.skillList[i].skillCommand[j].y)
                        maxY = GS.skillList[i].skillCommand[j].y;

                EditorGUILayout.BeginVertical();
                for (int y = (int)maxY; y >= 0; y--)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Box(""+y);
                    for (int x = 0; x <= maxX; x++)
                    {
                        
                        bool temp = true;
                        for (int j = 0; j < GS.skillList[i].skillCommand.Count; j++)
                        {
                            if (GS.skillList[i].skillCommand[j].x == x
                             && GS.skillList[i].skillCommand[j].y == y)
                            {GUILayout.Box(Resources.Load<Texture2D>("ui/whiteSquare1")); temp = false;}
                        }
                        if (temp)
                            GUILayout.Box(Resources.Load<Texture2D>("ui/whiteSquare0"));
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                for (int x = 0; x <= maxX; x++)
                {
                    GUILayout.Box(""+x);
                    GUILayout.Space(9);
                }
                EditorGUILayout.EndHorizontal();

            }
            
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}