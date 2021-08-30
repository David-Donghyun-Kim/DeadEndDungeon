using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject player;
	public GameObject cinematicCamera;
	public GameObject mainCamera;
	public GameObject realCamera;
	public Vector3 location;
	public float border;
	
	public bool isSpawned;
	
	GameObject playerInstance;
	GameObject cinematicCameraInstance;
	GameObject mainCameraInstance;
	int maxDelay = 20;
	int delay;

    public GameObject managementMenu;
    public GameObject buildingMenu;
	
	void Start()
    {
		isSpawned = false;
	}
	
	//void FixedUpdate() {
	//	delay--;
	//	if(Input.GetKey(KeyCode.O) && delay < 0) {
	//		if(isSpawned) {
	//			Destroy();
 //               managementMenu.SetActive(true);
 //               buildingMenu.SetActive(true);
 //           } else {
	//			Spawn();
 //               managementMenu.SetActive(false);
 //               buildingMenu.SetActive(false);
	//		}
	//		delay = maxDelay;
	//	}
		
 //       if (isSpawned)
 //       {
 //           Vector3 trans = playerInstance.transform.position;
 //           if (trans.x > border) { playerInstance.transform.position = new Vector3(trans.x * -1.0f, trans.y, trans.z); }
 //           if (trans.x < border * -1.0f) { playerInstance.transform.position = new Vector3(trans.x * -1.0f, trans.y, trans.z); }
 //           if (trans.z > border) { playerInstance.transform.position = new Vector3(trans.x, trans.y, trans.z * -1.0f); }
 //           if (trans.z < border * -1.0f) { playerInstance.transform.position = new Vector3(trans.x, trans.y, trans.z * -1.0f); }
 //       }
	//}
	
	public void Spawn() {
		isSpawned = true;
		playerInstance = Instantiate(player);
		cinematicCameraInstance = Instantiate(cinematicCamera);
		mainCameraInstance = Instantiate(mainCamera);
		player.transform.position = location;
		cinematicCameraInstance.GetComponent<CinemachineVirtualCamera>().Follow = playerInstance.transform;
		//playerInstance.transform.localScale = new Vector3 (0.25f,0.25f,0.25f);
		realCamera.SetActive(false);
	}
	
	public void Destroy() {
		isSpawned = false;
		Destroy(playerInstance);
		Destroy(cinematicCameraInstance);
		Destroy(mainCameraInstance);
		Cursor.lockState = CursorLockMode.Confined;
		realCamera.SetActive(true);
	}
}
