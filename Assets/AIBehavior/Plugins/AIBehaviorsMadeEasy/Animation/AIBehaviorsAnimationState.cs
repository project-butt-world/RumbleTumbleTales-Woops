using UnityEngine;


public class AIBehaviorsAnimationState : MonoBehaviour
{
	public new string name = "Untitled";

	public int startFrame = 0;
	public int endFrame = 0;

	public float speed = 1.0f;

	public WrapMode animationWrapMode = WrapMode.Loop;

	public bool foldoutOpen = false;
}