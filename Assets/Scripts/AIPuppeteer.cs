using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIPuppeteer : MonoBehaviour {
    private List<AIPuppet> puppets;
    private List<AISpot> spots;

    // Start is called before the first frame update
    void Start() {
        spots = new List<AISpot>(FindObjectsOfType<AISpot>());
        puppets = new List<AIPuppet>(FindObjectsOfType<AIPuppet>());
        foreach (AIPuppet p in puppets) {
            p.HardReset();
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public AISpot NextSpot(AIPuppet p, AISpot s, bool movingUp) {
        //Debug.Log(p.gameObject.name + (movingUp ? " ADVANCING" : " RETREATING"));
        List<AISpot> candidates = spots.Where(a => !a.occupied() && a.gameObject.name != s.gameObject.name).ToList();
        if (movingUp) {
            candidates = candidates.Where(a => a.mvalue() <= s.mvalue()).OrderByDescending(a => a.mvalue()).Reverse().ToList();
        } else {
            candidates = candidates.Where(a => a.mvalue() >= s.mvalue() && !a.critical).OrderByDescending(a => a.mvalue()).ToList();
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
        return Random.Range(90, 180) / 10.0f;
    }
}
