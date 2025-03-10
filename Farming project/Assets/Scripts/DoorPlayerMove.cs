using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPlayerMove : MonoBehaviour
{
    public GameObject player;
    public Transform newPosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            player.transform.position = newPosition.position;
        }
    }
}
