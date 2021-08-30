using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawnTest : MonoBehaviour
{
    public GameObject prop;
    void Start()
    {
        for (int i = 0; i < 1; i++) {
            int numChildren = gameObject.transform.childCount;
            for (int j = 0; j < numChildren; j++) {
                Transform collider = gameObject.transform.GetChild(j);
                float height = collider.GetComponent<BoxCollider>().size.y * 2;
                float centerZ = collider.GetComponent<BoxCollider>().center.z;
                float centerX = collider.GetComponent<BoxCollider>().center.x;

                Debug.Log(collider.GetComponent<BoxCollider>().center.z);
                //float height = gameObject.transform.GetComponent<BoxCollider>().size.y * 2;
                float width = UnityEngine.Random.Range(-1.5f, 1.5f); 
                float length = UnityEngine.Random.Range(-1.5f, 1.5f);
                Vector3 propSpawnCoordinates = new Vector3(centerX + width, height, centerZ + length);
                Debug.Log(propSpawnCoordinates);
                Quaternion propSpawnRotation = new Quaternion(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);

                GameObject spawnedProp = Instantiate(prop, propSpawnCoordinates, propSpawnRotation);
                spawnedProp.transform.SetParent(collider);
                spawnedProp.transform.localPosition = new Vector3(centerX + width, height, centerZ + length);
            }
            
        }
    }
}
