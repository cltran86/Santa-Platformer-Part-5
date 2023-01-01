using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector2 minimumValue;
    [SerializeField]
    private Vector2 maximumValue;

    // Update is called once per frame
    void Update()
    {
        // Set a variable to the target's transform position
        Vector3 v = target.transform.position;

        // Clamp the x and y values to whatever the minimum and maximum's are, and set the z to this position's z
        v.x = Mathf.Clamp(v.x, minimumValue.x, maximumValue.x);
        v.y = Mathf.Clamp(v.y, minimumValue.y, maximumValue.y);
        v.z = transform.position.z;

        // Assign this position to that new value
        transform.position = v;
    }
}
