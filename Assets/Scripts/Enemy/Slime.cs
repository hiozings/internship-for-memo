using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public override void Move()
    {
        base.Move();
        Jump();
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * 0.1f, ForceMode2D.Impulse);
    }
}
