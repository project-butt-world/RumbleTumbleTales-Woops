using UnityEngine;
using System.Collections;

public class PlayerTouch : MonoBehaviour {
	Animator anim;
	public float speed = 0.1F;
//	KeyCode right;
//	KeyCode left;
//	public bool walking=false;
	Transform curTransform;
	GameObject forflipping;
	GameObject fizzpoz;
	Transform fizzTransform;
	Vector2 position;
	Vector2 flipWhichWay;



//	public GameObject arse; 
	//public bool facingRight = true;	
//	public Transform objToPickUp;






	void Start(){
		anim = GetComponent<Animator>();
//		arse = GameObject.Find("FizzFlip");
//		objToPickUp = this.transform;
		//right = Input.GetKey (KeyCode.RightArrow);
	//	left = Input.GetKey (KeyCode.LeftArrow);
	//	walking = false;
	//	curTransform = Fizz_Global_CTRL.GetComponent(localscale);
		forflipping = GameObject.Find("Princess Fizz/Fizz_Global_CTRL");
		fizzpoz= GameObject.Find("Princess Fizz");
		Transform fizzTransform = fizzpoz.transform;
		// get player position
		position = fizzTransform.position;
		//flipWhichWay = Input.GetTouch (0).deltaPosition.x;


	



		}


	void Update() {
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
						Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;
	//		Debug.Log (touchDeltaPosition);
						transform.Translate (-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
				
		

			}

	//	flipWhichWay = Input.GetTouch(0).position;
	//	Debug.Log (flipWhichWay);
	//	Debug.Log (Screen.width/2);
	//	if (flipWhichWay.x < position.x) {
	//				forflipping.transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	//					anim.SetBool ("walk_dammit", true);

		

		//		}



		//			anim.SetBool ("walk_dammit", true);
			        //    arse = GameObject.Find("FizzFlip");
			 //           transform.localScale=(-1,1,0);
		//	flip ();
		//	OnAnimatorIK();
	//	Debug.Log (position);


	//else
		 if(Input.GetKey(KeyCode.UpArrow)) {
	//		Debug.Log ("left presssed");
//			Puppet2D_GlobalControl.flip = true; 
		//	walking=true;
		//	flip ();
		//	hand.transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
			anim.SetBool("walk_dammit",true);


		}

		 else if(Input.GetKey(KeyCode.DownArrow)) {
			
		//	Debug.Log ("right pressed");
		//	Puppet2D_GlobalControl.flip = false; 
			forflipping.transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
			anim.SetBool("walk_dammit",true);

		//	testMove();
		//	walking=true;
			//	flip ();
			
		}


		else  {
						anim.SetBool ("walk_dammit", false);
						anim.SetBool ("walk_left", false);
				}
	}

	//void flip(){
				// Switch the way the player is labelled as facing.
			//	facingRight = !facingRight;
	
				// Multiply the player's x local scale by -1.
		//		Vector3 theScale = arse.transform.localScale;
		//		theScale.x *= -1;
			//	transform.localScale = theScale;
		     //   
	//	}
	//number should be int layerindex
//	void OnAnimatorIK() {
//		float reach = anim.GetFloat("RightHandReach");
//		anim.SetIKPositionWeight(AvatarIKGoal.RightHand, reach);
//		anim.SetIKPosition(AvatarIKGoal.RightHand, objToPickUp.position);
//	}

//	void testMove(){
	//	Puppet2D_GlobalControl.flip = true; 
	
//		Debug.Log ("testmove");
//		    anim.SetIKRotation = -1;

//		}
	//
//	void otherFunc(){


//		Debug.Log ("movenoflip");
		
		
		
		//		    anim.SetIKRotation = -1;
		
//	}
/* imported from global control
 * 	public bool flip = false;
 * 
 * if(flip)
				transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);


*/


}