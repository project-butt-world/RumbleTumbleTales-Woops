#pragma strict

var fountain : GameObject;
	fountain = GameObject.FindGameObjectWithTag("Fountain");
var valveAnim : Animator;


function Start () 
{
	
	
	
}




function Update () 
{

valveAnim.SetBool("twistin", false);

	if (Input.GetMouseButtonDown(0))
	{
		valveAnim.SetBool("twistin", true);
		Debug.Log ("Mouse pressed");
		
		Destroy(fountain);
		
	}
}






	