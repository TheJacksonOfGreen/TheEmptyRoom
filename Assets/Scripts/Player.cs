using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private Rigidbody playerRigidbody;
    private Renderer playerRenderer;
    private Camera playerCam;
    private Camera tubCam;
    private GameObject drone;
    private Text controlPrompt;
    public float moveSpeed = 1.0f;
    private bool wDown = false;
    private bool aDown = false;
    private bool sDown = false;
    private bool dDown = false;
    private bool spaceDown = false;
    private bool droneMode = false;
    private bool hidingInTub = false;

    private MouseLook droneSwivelA;
    private MouseLook droneSwivelB;
    private MouseLook playerSwivelA;
    private MouseLook playerSwivelB;
    private bool inRangeOfMonitor;
    private bool inRangeOfTub;
    private bool quit;
    private GameObject gameOverScreen;

    private const float DIAG_CONSTANT = 0.70710678118f;

    // Start is called before the first frame update
    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        playerCam = transform.Find("Main Camera").gameObject.GetComponent<Camera>();
        tubCam = GameObject.Find("Bathtub").transform.Find("TubCam").gameObject.GetComponent<Camera>();
        tubCam.enabled = false;
        drone = GameObject.Find("Drone");
        droneSwivelA = drone.GetComponent<MouseLook>();
        droneSwivelA.enabled = false;
        droneSwivelB = drone.transform.Find("CameraHead").Find("DroneCam").gameObject.GetComponent<MouseLook>();
        droneSwivelB.enabled = false;
        playerSwivelA = GetComponent<MouseLook>();
        playerSwivelB = playerCam.gameObject.GetComponent<MouseLook>();
        controlPrompt = GameObject.Find("UI").transform.Find("Control Prompts").gameObject.GetComponent<Text>();
        inRangeOfMonitor = false;
        gameOverScreen = GameObject.Find("UI").transform.Find("GameOverScreen").gameObject;
        gameOverScreen.SetActive(false);
        quit = false;
    }

    // Update is called once per frame
    void Update() {
        wDown = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        aDown = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        sDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        dDown = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        spaceDown = Input.GetKey(KeyCode.Space);

        if (!quit) {
            droneSwivelA.enabled = droneMode;
            droneSwivelB.enabled = droneMode;
            playerSwivelA.enabled = !droneMode;
            playerSwivelB.enabled = !droneMode;
            playerCam.enabled = true;
            tubCam.enabled = false;
            playerRenderer.enabled = true;

            if (droneMode) {
                if (wDown) {
                    if (aDown) {
                        drone.transform.Translate(new Vector3((moveSpeed * DIAG_CONSTANT), 0, (moveSpeed * DIAG_CONSTANT)) * Time.deltaTime);
                    } else if (dDown) {
                        drone.transform.Translate(new Vector3((moveSpeed * DIAG_CONSTANT), 0, (moveSpeed * -1 * DIAG_CONSTANT)) * Time.deltaTime);
                    } else {
                        drone.transform.Translate(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
                    }
                } else if (sDown) {
                    if (aDown) {
                        drone.transform.Translate(new Vector3((moveSpeed * -1 * DIAG_CONSTANT), 0, (moveSpeed * DIAG_CONSTANT)) * Time.deltaTime);
                    } else if (dDown) {
                        drone.transform.Translate(new Vector3((moveSpeed * -1 * DIAG_CONSTANT), 0, (moveSpeed * -1 * DIAG_CONSTANT)) * Time.deltaTime);
                    } else {
                        drone.transform.Translate(new Vector3((moveSpeed * -1), 0, 0) * Time.deltaTime);
                    }
                } else if (aDown) {
                    drone.transform.Translate(new Vector3(0, 0, moveSpeed) * Time.deltaTime);
                } else if (dDown) {
                    drone.transform.Translate(new Vector3(0, 0, (-1 * moveSpeed)) * Time.deltaTime);
                }
                if (spaceDown) {
                    drone.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                } else if (!Physics.Raycast(drone.transform.position, Vector3.down, 0.1f)) {
                    drone.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime * 0.50f);
                }
                if (Input.GetKeyDown(KeyCode.Q)) {
                    droneMode = false;
                }
            } else if (hidingInTub) {
                playerCam.enabled = false;
                tubCam.enabled = true;
                playerRenderer.enabled = false;
                if (Input.GetKeyDown(KeyCode.Q)) {
                    hidingInTub = false;
                }
            } else {
                if (dDown) {
                    if (wDown) {
                        transform.Translate(new Vector3((moveSpeed * DIAG_CONSTANT), 0, (moveSpeed * DIAG_CONSTANT)) * Time.deltaTime);
                    } else if (sDown) {
                        transform.Translate(new Vector3((moveSpeed * DIAG_CONSTANT), 0, (moveSpeed * -1 * DIAG_CONSTANT)) * Time.deltaTime);
                    } else {
                        transform.Translate(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
                    }
                } else if (aDown) {
                    if (wDown) {
                        transform.Translate(new Vector3((moveSpeed * -1 * DIAG_CONSTANT), 0, (moveSpeed * DIAG_CONSTANT)) * Time.deltaTime);
                    } else if (sDown) {
                        transform.Translate(new Vector3((moveSpeed * -1 * DIAG_CONSTANT), 0, (moveSpeed * -1 * DIAG_CONSTANT)) * Time.deltaTime);
                    } else {
                        transform.Translate(new Vector3((moveSpeed * -1), 0, 0) * Time.deltaTime);
                    }
                } else if (wDown) {
                    transform.Translate(new Vector3(0, 0, moveSpeed) * Time.deltaTime);
                } else if (sDown) {
                    transform.Translate(new Vector3(0, 0, (-1 * moveSpeed)) * Time.deltaTime);
                }
                if (Input.GetKeyDown(KeyCode.Space)) {
                    if (inRangeOfMonitor) {
                        droneMode = true;
                    } else if (inRangeOfTub) {
                        hidingInTub = true;
                    }
                }
            }

            if (inRangeOfMonitor) {
                if (droneMode) {
                    controlPrompt.text = "[Q] Stop Controlling Drone";
                } else {
                    controlPrompt.text = "[SPACE] Control Drone";
                }
            } else if (inRangeOfTub) {
                if (hidingInTub) {
                    controlPrompt.text = "[Q] Leave Bathtub";
                } else {
                    controlPrompt.text = "[SPACE] Hide in Bathtub";
                }
            } else {
                controlPrompt.text = "";
            }
        } else {
            if (spaceDown) {
                Application.Quit();
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "DroneMonitorTrigger") {
            inRangeOfMonitor = true;
        } else if (other.gameObject.name == "BathtubHideTrigger") {
            inRangeOfTub = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "DroneMonitorTrigger") {
            inRangeOfMonitor = false;
        } else if (other.gameObject.name == "BathtubHideTrigger") {
            inRangeOfTub = false;
        }
    }

    public bool isMortal() {
        return !hidingInTub;
    }

    public void Jump() {
        gameOverScreen.SetActive(true);
        quit = true;
    }
}
