using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyPickup : MonoBehaviour
{
    public float Health = 10f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerBehaviourController>().health.Heal(Health);
            Destroy(gameObject);
        }
    }
}
