using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    public Transform moveTo;

    void Update(){
        transform.position = moveTo.position;
        transform.rotation = moveTo.rotation;
    }
}
