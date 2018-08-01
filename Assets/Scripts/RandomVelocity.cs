using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVelocity : MonoBehaviour {
	public float velocity, directionChangeTimer = 3, gravity = 0.9f;
	Vector3 velocityToApply;

	// run indefinitely till closed
	IEnumerator ChangeDirection(float waitTime){
		while(true){
			
			velocityToApply = RandomVector(-1,1);
			velocityToApply = velocityToApply.normalized * velocity;

			yield return new WaitForSeconds (waitTime);
		}
	}

	Vector3 RandomVector(float rangeMin, float rangeMax){
		return new Vector3(
			Random.Range(rangeMin,rangeMax),
			Random.Range(rangeMin,rangeMax),
			Random.Range(rangeMin,rangeMax));
	}

	void Start () {
		StartCoroutine(ChangeDirection(directionChangeTimer));
	}

	void FixedUpdate(){
		float gravityVelocity = GetComponent<Rigidbody>().velocity.y;
		GetComponent<Rigidbody>().velocity = velocityToApply;
		velocityToApply.y = gravityVelocity;
	}

	void OnApplicationQuit(){
		StopAllCoroutines();
	}

}
