using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : Movement
{

    protected override void HandleInput()
    {
        _inputDirection = new Vector2(x: Input.GetAxis("Horizontal"), y: Input.GetAxis("Vertical"));
    }

    protected override void HandleRotation()
    {
        //if (_weaponHandler == null || _weaponHandler.CurrentWeapon == null)
        //{
        //    return;
        //}

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos = new Vector3(mousePos.x, mousePos.y, transform.position.z);

        Vector2 direction = (Vector2)(mousePos - transform.position);

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
