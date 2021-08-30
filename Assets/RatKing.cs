using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatKing : MonoBehaviour
{
	
	GameObject player;
	public int idleTimer;
	public int chargeTimer;
	public int chargeSpeed;
	public int chargeDistance;
	public int slamTimer;
	public int slamDuration;
	Vector3 playerPos;
	Vector3 oldPos;
	int timer;
	int state;
	Animator animator;
	Vector3 lookPos;
	Quaternion rotation;
	
	
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
		state = 0;
		timer = idleTimer;
		animator = GetComponent<Animator>();
		
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		// Base idle
		if(state == 0) {
			timer--;
			if(timer < 0) { 
				float choice = Mathf.Floor(Random.Range(0,2)) + 1.0f;
				
				if(choice == 1) {
					lookPos = player.transform.position - transform.position;
					lookPos.y = 0;
					rotation = Quaternion.LookRotation(lookPos);
					

					playerPos = player.transform.position;
					state = 1; //
					timer = chargeTimer;
					oldPos = transform.position;
					
					animator.SetTrigger("ChargeWindUp");
				}
				
				if(choice == 2) {
					state = 2;
					timer = slamTimer;
					animator.SetTrigger("Slamming");
				}
			}
		}
		
		// Charge
		if(state == 1) {
			/*state = 0;*/			
			timer--;
			
			if(timer > 0) {
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
			}
			
			if(timer == 0) {
				animator.SetTrigger("Charging");
			}
			
			if(timer < 0) {
				GetComponent<Rigidbody>().velocity = transform.forward * chargeSpeed;
				this.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
			
			if(Vector3.Distance(oldPos, transform.position) > chargeDistance || timer < -150) {
				state = 0;
				timer = idleTimer;
				this.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
			}

		}
		
		// Slam
		if(state == 2) {
			/*state = 0;*/			
			timer--;
			
			
			
			if(timer < 0) {
				Debug.Log("SLAM");
				this.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
			}
			
			if(timer < -1 * slamDuration) {
				state = 0;
				timer = idleTimer;
				this.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
			}

		}
		
		
    }
}
