using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISpot : MonoBehaviour {
    public bool critical;
    private float m;
    private AIPuppet p;

    // Start is called before the first frame update
    void Start() {
        m = transform.position.magnitude;
        vacate();
    }

    // Update is called once per frame
    void Update() {}

    public float mvalue() {
        return m;
    }

    public bool occupied() {
        return p != null;
    }

    public void occupy(AIPuppet p) {
        p = p;
    }

    public void vacate() {
        p = null;
    }
}
