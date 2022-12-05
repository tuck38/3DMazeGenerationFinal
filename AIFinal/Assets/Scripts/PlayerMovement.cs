using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Camera cam;
    [SerializeField] float moveSpeed;

    [SerializeField] Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(Input.GetKey(KeyCode.W)) 
        {
            gameObject.transform.position += (cam.transform.forward.normalized * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) 
        {
            gameObject.transform.position += -(cam.transform.forward.normalized * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) 
        {
            gameObject.transform.position += -(cam.transform.right.normalized * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) 
        {
            gameObject.transform.position += (cam.transform.right.normalized * moveSpeed * Time.deltaTime);
        }
        
    }
}
