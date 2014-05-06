using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class InPlayerViewTrigger : BaseTrigger
{
	private Camera mainCam = null;


	protected override void Init()
	{
		mainCam = Camera.mainCamera;

		if ( mainCam == null )
		{
			Debug.LogWarning("No main camera found, 'InPlayerViewTrigger' will not work without a main camera.");
		}
	}


	protected override bool Evaluate(AIBehaviors fsm)
	{
		if ( CheckIfInPlayerCameraView(fsm.transform.position) )
		{
			fsm.ChangeActiveState(transitionState);
			return true;
		}

		return false;
	}


	public bool CheckIfInPlayerCameraView(Vector3 fsmPosition)
	{
		if ( mainCam == null )
		{
			mainCam = Camera.mainCamera;
		}

		if ( mainCam != null )
		{
			Vector3 screenPoint = mainCam.WorldToScreenPoint(fsmPosition);

			if ( screenPoint.z > 0.0f )
			{
				if ( screenPoint.x > 0.0f && screenPoint.x < Screen.width )
				{
					if ( screenPoint.y > 0.0f && screenPoint.x < Screen.height )
					{
						Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
						RaycastHit hit;

						// Make sure there's nothing between the AI and the main camera
						if ( !Physics.Raycast(ray, out hit, 1000.0f) )
						{
							Transform checkTFM = transform;
							Transform hitTFM = hit.transform;

							while ( checkTFM != null )
							{
								if ( checkTFM == hitTFM )
								{
									return false;
								}

								checkTFM = transform.parent;
							}

							return true;
						}
					}
				}
			}
		}

		return false;
	}
}