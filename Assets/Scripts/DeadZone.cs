using System;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(("Player")))
            // if (!other.GetComponent<PlayerPossess>().IsPossessing)
            // {
            //     other.GetComponent<PlayerController>().IsDead();
            // }
            other.GetComponent<PlayerController>().IsDead();
    }
}
