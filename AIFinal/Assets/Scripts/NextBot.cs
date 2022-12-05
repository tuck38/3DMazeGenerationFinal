using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NextBot : MonoBehaviour
{

    private Rigidbody rb;

    [SerializeField] GameObject player;

    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //FacePlayer();
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    void FacePlayer()
    {
        if(player)
        {

            Vector3 playerDirection = (player.transform.position - transform.position).normalized;

            float face = Mathf.Atan2(playerDirection.z, playerDirection.z) * Mathf.Rad2Deg;

            transform.Rotate(Vector3.forward, face);
        }
    }

    void MoveToPlayer()
    {
        if(player)
        {
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;

            rb.velocity = (playerDirection * moveSpeed);
        }
    }
}
