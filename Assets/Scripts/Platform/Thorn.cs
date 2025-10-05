using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        
        Attack attack = GetComponent<Attack>();
        bool isGround = collision.collider.GetComponent<PhysicsCheck>()?.isGround ?? false;
        if(isGround)
            collision.collider.GetComponent<Character>()?.TakeDamage(attack);
    }
}
