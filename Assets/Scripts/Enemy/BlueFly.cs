using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFly : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        rb.gravityScale = 0;
    }

    protected override void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        if (!isDead)
        {
            if (physicsCheck.touchLeftWall)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (physicsCheck.touchRightWall)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        base.Move();
        //Debug.Log("BlueFly Move");
    }
    public override void EnemyDead()
    {
        base.EnemyDead();
        rb.gravityScale = 1;
    }
}
