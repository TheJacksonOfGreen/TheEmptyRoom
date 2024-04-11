using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPuppet : MonoBehaviour {
    private NavMeshAgent agent;
    private AIPuppeteer boss;
    private AISpot spot;
    private Player player;
    private bool visible;
    private float tick;
    private float elapsed;
    private float watched;
    private bool traveling;

    void Awake() {
        boss = FindObjectOfType<AIPuppeteer>();
        spot = null;
    }

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
        tick = 100.0f;
        elapsed = 0.0f;
        watched = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        if (!traveling && spot != null) {
            elapsed += Time.deltaTime;
            if (spot.critical) {
                if (visible) {
                    elapsed -= Time.deltaTime / 2.0f;
                }
                if (elapsed >= tick) {
                    if (player.isMortal()) {
                        player.Jump();
                    } else {
                        spot.vacate();
                        spot = boss.ResetSpot(this);
                        //Debug.Log(gameObject.name + " MOVING TO " + spot.gameObject.name);
                        traveling = true;
                        agent.destination = spot.transform.position;
                        tick = boss.NextTime();
                    }
                }
            } else {
                if (visible) {
                    watched += Time.deltaTime;
                }
                if (elapsed >= tick) {
                    spot = boss.NextSpot(this, spot, watched <= tick / 3.0f);
                    //Debug.Log(gameObject.name + " MOVING TO " + spot.gameObject.name);
                    traveling = true;
                    agent.destination = spot.transform.position;
                    elapsed = 0.0f;
                    watched = 0.0f;
                    if (spot.critical) {
                        tick = 10.0f;
                    } else {
                        tick = boss.NextTime();
                    }
                }
            }
        }
    }

    public void HardReset() {
        if (spot != null) {
            spot.vacate();
        }
        spot = boss.ResetSpot(this);
        //Debug.Log(gameObject.name + " MOVING TO " + spot.gameObject.name);
        agent.destination = spot.transform.position;
        traveling = true;
        tick = boss.NextTime();
    }

    public void SetTarget(Vector3 target) {
        agent.destination = target;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == spot.gameObject.name) {
            traveling = false;
            //Debug.Log(gameObject.name + " ARRIVED AT " + spot.gameObject.name);
        }
        if (other.gameObject.name == "Vision Cone") {
            visible = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "Vision Cone") {
            visible = false;
        }
    }
}
