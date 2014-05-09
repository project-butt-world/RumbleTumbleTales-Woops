#pragma strict

public var target : Transform;
public var distance : float;

function Start () {

}


function Update(){
 
    transform.position.z = -165.883;
   // transform.position.y = target.position.y-distance;
   transform.position.y = 40.10676;
    transform.position.x = target.position.x-distance;
 
}