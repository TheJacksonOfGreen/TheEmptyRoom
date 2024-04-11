using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        this.transform.Rotate(Vector3.up * (Random.Range(0, 36000) / 100.0f), Space.Self);
    }

    // Update is called once per frame
    void Update() {}
}
