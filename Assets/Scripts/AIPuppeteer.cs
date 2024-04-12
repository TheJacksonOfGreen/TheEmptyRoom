using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIPuppeteer : MonoBehaviour {
    private List<AIPuppet> puppets;
    private List<AISpot> spots;
    private int pressure;

    // Start is called before the first frame update
    void Start() {
        spots = new List<AISpot>(FindObjectsOfType<AISpot>());
        puppets = new List<AIPuppet>(FindObjectsOfType<AIPuppet>());
        foreach (AIPuppet p in puppets) {
            p.HardReset();
        }
        pressure = 10;
    }

    // Update is called once per frame
    void Update() {

    }

    public AISpot NextSpot(AIPuppet p, AISpot s, bool movingUp) {
        //Debug.Log(p.gameObject.name + (movingUp ? " ADVANCING" : " RETREATING"));
        List<AISpot> candidates = spots.Where(a => !a.occupied() && a.gameObject.name != s.gameObject.name).ToList();
        if (movingUp) {
            pressure -= Random.Range(0, 1);
            candidates = candidates.Where(a => a.mvalue() <= s.mvalue()).OrderByDescending(a => a.mvalue()).Reverse().ToList();
        } else {
            pressure += Random.Range(0, 1);
            candidates = candidates.Where(a => a.mvalue() >= s.mvalue() && !a.critical).OrderByDescending(a => a.mvalue()).ToList();
        }
        if (pressure < 0) {
            pressure = 0;
        }
        if (pressure > 20) {
            pressure = 20;
        }
        candidates = candidates.OrderByDescending(a => Vector3.Distance(s.transform.position, a.transform.position)).Reverse().ToList();
        //Debug.Log(p.gameObject.name + " FOUND " + candidates.Count + " CANDIDATES");
        if (candidates.Count > 10) {
            candidates = candidates.GetRange(0, candidates.Count / 10);
        }
        s.vacate();
        AISpot target = candidates[0];
        target.occupy(p);
        return target;
    }

    public AISpot ResetSpot(AIPuppet p) {
        List<AISpot> candidates = spots.Where(a => !a.occupied() && !a.critical).ToList();
        candidates = candidates.OrderByDescending(a => a.mvalue()).ToList();
        AISpot target = candidates[Random.Range(0, candidates.Count / 10)];
        target.occupy(p);
        return target;
    }

    public float NextTime() {
        return (Random.Range(20, 120) + (10 * pressure)) / 10.0f;
    }
}
