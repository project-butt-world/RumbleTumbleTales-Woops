#pragma strict

var moveUp : KeyCode;
var moveDown : KeyCode;
var moveLeft : KeyCode;
var moveRight : KeyCode;
var anim: Animator;

var speed : float = 50;

function Update ()
{
	if (Input.GetKey(moveUp))
	{
		rigidbody2D.velocity.y = speed;
		anim.SetBool("walk_dammit",true);
	}
	else if (Input.GetKey(moveDown))
	{
		rigidbody2D.velocity.y = speed *-1;
		anim.SetBool("walk_dammit",true);
	}
	else if (Input.GetKey(moveLeft))
	{
		rigidbody2D.velocity.x = speed *-1;
		anim.SetBool("walk_dammit",true);
	}
	else if (Input.GetKey(moveRight))
	{
		rigidbody2D.velocity.x = speed ;
		anim.SetBool("walk_dammit",true);
	}
	else
	{
		rigidbody2D.velocity.y = 0;
		rigidbody2D.velocity.x = 0;
		anim.SetBool("walk_dammit",false);
	}
}