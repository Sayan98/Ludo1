using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	void Start(){
		iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
		//iTween.MoveFrom(gameObject, iTween.Hash("y", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", 0.1));
	}
}

