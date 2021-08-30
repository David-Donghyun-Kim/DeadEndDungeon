using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Vector3 travelDirection;
    public float travelSpeed;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        //travelDirection = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        travelDirection.Normalize();
        transform.position += Time.deltaTime * travelSpeed * travelDirection;
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
