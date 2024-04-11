using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    [Tooltip("Whether or not this Rotator is actively running.")]
    public bool on = true;
    [Tooltip("How many Rotations Per Second around the X Axis this should Rotate.")]
    public float xRate = 0.0f;
    [Tooltip("How many Rotations Per Second around the Y Axis this should Rotate.")]
    public float yRate = 0.0f;
    [Tooltip("How many Rotations Per Second around the Z Axis this should Rotate.")]
    public float zRate = 0.0f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (on) {
            this.transform.Rotate(new Vector3(xRate, yRate, zRate) * 360.0f * Time.deltaTime, Space.Self);
        }
    }
}
