using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GraffitiSystem))]
public class GraffitiSystemCustomInspector : Editor
{

    GraffitiSystem GS;
    bool showPosition = false;
    void OnEnable()
    {
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
                EditorGUILayout.LabelField("skill" + i);
                float maxX = 0;
                for (int j = 0; j < GS.skillList[i].skillCommand.Count; j++)
                    if (maxX < GS.skillList[i].skillCommand[j].x)
                        maxX = GS.skillList[i].skillCommand[j].x;
                float maxY = 0;
                for (int j = 0; j < GS.skillList[i].skillCommand.Count; j++)
                    if (maxY < GS.skillList[i].skillCommand[j].y)
                        maxY = GS.skillList[i].skillCommand[j].y;

                EditorGUILayout.BeginVertical();
                for (int y = (int)maxY + 1; y >= 0; y--)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int x = 0; x <= maxX + 1; x++)
                    {

                        bool temp = true;
                        for (int j = 0; j < GS.skillList[i].skillCommand.Count; j++)
                        {
                            if (GS.skillList[i].skillCommand[j].x == x && GS.skillList[i].skillCommand[j].y == y)
                            {
                                if (GUILayout.Button(Resources.Load<Texture2D>("ui/whiteSquare1"),
                                GUILayout.Width(30), GUILayout.Height(30)))
                                {
                                    GS.skillList[i].skillCommand.Remove(new Vector2(x, y));
                                }
                                temp = false;
                            }
                        }
                        if (temp)
                        {
                            if (GUILayout.Button(Resources.Load<Texture2D>("ui/whiteSquare0"),
                            GUILayout.Width(30), GUILayout.Height(30)))
                            {
                                GS.skillList[i].skillCommand.Add(new Vector2(x, y));
                            }
                        }

                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}