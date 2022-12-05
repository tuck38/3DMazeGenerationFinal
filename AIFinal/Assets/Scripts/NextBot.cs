using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBot : MonoBehaviour
{

    [SerializeField] GameObject player;

    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveToPlayer();
    }

    void moveToPlayer()
    {
        Vector3 playerVector = transform.position - player.transform.position;

        transform.position += playerVector.normalized * moveSpeed * Time.deltaTime;
    }
}
