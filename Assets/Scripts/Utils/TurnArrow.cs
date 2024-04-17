using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnArrow : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, 5f * Time.deltaTime);
    }
}
