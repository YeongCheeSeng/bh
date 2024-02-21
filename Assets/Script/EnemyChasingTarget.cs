using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyChasingTarget : Movement
{
    public Transform Target;

    protected override void HandleInput()
    {
        if (Target == null)
        {
            return;
        }
 
        Target = GameObject.FindWithTag("Player").transform;

        _inputDirection = (Target.position - transform.position).normalized;
    }
}
