using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Skill))]
public class GraffitiSkillCustomInspector : Editor
{

    Skill skill;
    bool showPosition = false;
    void OnEnable()
    {
        skill = target as Skill;

    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("SkillCommand");
        float maxX = 0;
        for (int j = 0; j < skill.skillCommand.Count; j++)
            if (maxX < skill.skillCommand[j].x)
                maxX = skill.skillCommand[j].x;
        float maxY = 0;
        for (int j = 0; j < skill.skillCommand.Count; j++)
            if (maxY < skill.skillCommand[j].y)
                maxY = skill.skillCommand[j].y;

        EditorGUILayout.BeginVertical();
        for (int y = (int)maxY + 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x <= maxX + 1; x++)
            {

                bool temp = true;
                for (int j = 0; j < skill.skillCommand.Count; j++)
                {
                    if (skill.skillCommand[j].x == x && skill.skillCommand[j].y == y)
                    {
                        if (GUILayout.Button(Resources.Load<Texture2D>("ui/whiteSquare1"),
                        GUILayout.Width(30), GUILayout.Height(30)))
                        {
                            skill.skillCommand.Remove(new Vector2(x, y));
                        }
                        temp = false;
                    }
                }
                if (temp)
                {
                    if (GUILayout.Button(Resources.Load<Texture2D>("ui/whiteSquare0"),
                    GUILayout.Width(30), GUILayout.Height(30)))
                    {
                        skill.skillCommand.Add(new Vector2(x, y));
                    }
                }

            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
}