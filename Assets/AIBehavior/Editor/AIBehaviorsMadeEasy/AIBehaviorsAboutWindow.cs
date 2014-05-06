using UnityEngine;
using UnityEditor;



public class AIBehaviorsAboutWindow : EditorWindow
{
	GUIStyle titleStyle = new GUIStyle(EditorStyles.whiteLargeLabel);
	GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
	GUIStyle linkStyle = new GUIStyle(EditorStyles.boldLabel);


	public static void ShowAboutWindow()
	{
		AIBehaviorsAboutWindow window = AIBehaviorsAboutWindow.GetWindow<AIBehaviorsAboutWindow>(true, "About...");

		window.minSize = window.maxSize = new Vector2(300.0f, 200.0f);

		window.linkStyle.alignment = window.labelStyle.alignment = window.titleStyle.alignment = TextAnchor.MiddleCenter;

		window.Show();
	}


	void OnGUI()
	{
		DrawCenteredLabel("AI Behaviors Made Easy!", titleStyle);
		EditorGUILayout.Separator();

		DrawCenteredLabel("Overall Design and Training by:");
		DrawCenteredLabel("Walker Boys Studio");
		DrawCenteredLabel("Chad Walker");
		DrawCenteredLabel("Eric Walker");

		EditorGUILayout.Separator();

		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("");
			if ( GUILayout.Button("www.walkerboystudio.com", linkStyle) )
			{
				Application.OpenURL("http://www.walkerboystudio.com/");
			}
			GUILayout.Label("");
		}
		GUILayout.EndHorizontal();

		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();

		DrawCenteredLabel("System Design Authored by:");
		DrawCenteredLabel("Nathan Warden");
		EditorGUILayout.Separator();
	}


	private void DrawCenteredLabel(string label)
	{
		DrawCenteredLabel(label, labelStyle);
	}


	private void DrawCenteredLabel(string label, GUIStyle guiStyle)
	{
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("");
			GUILayout.Label(label, guiStyle);
			GUILayout.Label("");
		}
		GUILayout.EndHorizontal();
	}
}