using UnityEngine;
using UnityEditor;


public class AIBehaviorsMenuItems : Editor
{
	const int defaultPriority = 0;
	const int aiBehaviorsPriority = 1;
	const int characterAnimatorPriority = 2;
	const int aboutPriority = 100;
	const int reportBugPriority = 101;


	// ===== Add Default Setup ===== //

	[MenuItem("Component/AI/AI Behaviors Default Setup", true, defaultPriority)]
	public static bool AddDefaultSetupValidator()
	{
		return Selection.activeObject != null;
	}


	[MenuItem("Component/AI/AI Behaviors Default Setup", false, defaultPriority)]
	public static void AddDefaultSetup()
	{
		GameObject go = Selection.activeObject as GameObject;

		go.AddComponent<AIBehaviors>();
		go.AddComponent<AIBehaviorsCharacterAnimator>();
		go.AddComponent<NavMeshAgent>();
	}


	[MenuItem("Component/AI/AI Behaviors Setup (No Nav Mesh Agent)", true, defaultPriority)]
	public static bool Add3rdPartySetupValidator()
	{
		return Selection.activeObject != null;
	}


	[MenuItem("Component/AI/AI Behaviors Setup (No Nav Mesh Agent)", false, defaultPriority)]
	public static void Add3rdPartySetup()
	{
		GameObject go = Selection.activeObject as GameObject;

		go.AddComponent<AIBehaviors>();
		go.AddComponent<AIBehaviorsCharacterAnimator>();
	}


	[MenuItem("Component/AI/AI Behaviors Component", true, aiBehaviorsPriority)]
	public static bool AddSnapComponentValidator()
	{
		return Selection.activeObject != null;
	}


	[MenuItem("Component/AI/AI Behaviors Component", false, aiBehaviorsPriority)]
	public static void AddSnapComponent()
	{
		((Selection.activeObject) as GameObject).AddComponent(typeof(AIBehaviors));
	}


	[MenuItem("Component/AI/Add Default Character Animator", true, characterAnimatorPriority)]
	public static bool AddDefaultCAComponentValidator()
	{
		return Selection.activeObject != null && (Selection.activeObject as GameObject).GetComponent<AIBehaviors>();
	}


	[MenuItem("Component/AI/Add Default Character Animator", false, characterAnimatorPriority)]
	public static void AddCharacterAnimatorComponent()
	{
		((Selection.activeObject) as GameObject).AddComponent(typeof(AIBehaviors));
	}


	[MenuItem("Component/AI/About AI Behaviors...", false, aboutPriority)]
	public static void About()
	{
		AIBehaviorsAboutWindow.ShowAboutWindow();
	}


	[MenuItem("Component/AI/Report a Bug (via your email client)", false, reportBugPriority)]
	public static void ReportBug()
	{
		Application.OpenURL("mailto:webmaster@walkerboystudio.com?subject=[Bug Report] AI Behaviors Made Easy!&cc=bugs@nathanwarden.com");
	}
}