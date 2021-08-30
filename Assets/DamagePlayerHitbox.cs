using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerHitbox : MonoBehaviour
{
	
	public int damage;
	
    // Start is called before the first frame update
    void Start()
    {
		
        
    }

    void OnTriggerStay(Collider col) {
		if(col.gameObject.tag == "Player") {
			Debug.Log("Ok!");
			CommandInvoker.AddCommand(new DealDamageToPlayerCommand(damage));
		}
		
		
	}
}
