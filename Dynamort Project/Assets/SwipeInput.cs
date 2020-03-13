using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SwipeInput : MonoBehaviour
{
    public int numZaps = 5;
    public GameObject Reticle;
    public TextMeshProUGUI numZapsText;
    public bool PCdebug = true;
    public Camera cam;
    private int maxPower = 100;
    [SerializeField] private Lightning _Lightning;
    private Vector2 origin;
    private float lastTap, playerChargeTime, magnitude;
    
    private AimPrediction _AimPrediction;
    private FadeReticle _FadeReticle;
    void Start() {
        _AimPrediction = GetComponentInChildren<AimPrediction>();
        _FadeReticle = Reticle.GetComponent<FadeReticle>();
    }
    public void setOriginForNewLevel(Vector2 pos){
        transform.position = origin = pos;
        print("origin: " + origin);
    }

    void Update() {
#if UNITY_EDITOR
    if(PCdebug) UpdateStandalone();
    else UpdateMobile();
#else
    UpdateMobile();
#endif
    }

    private void UpdateStandalone() {
        Vector3 touchWorldPosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0)) { // Record initial mouse position.
            lastTap = Time.time;
            _AimPrediction.toggleParticles(true);
            Reticle.transform.position = Input.mousePosition;
            _FadeReticle.fade(true);
        }        
        else if (Input.GetMouseButton(0)) { // Determine direction by comparing the current mouse position with the initial one.
            someting(touchWorldPosition);
            Reticle.transform.position = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) { // LMB released.
            _FadeReticle.fade(false);
            playerChargeTime = (Time.time - lastTap) * 50;
            magnitude = (playerChargeTime > maxPower) ? maxPower : playerChargeTime;
            StartCoroutine(_Lightning.zap(transform.position, cam.ScreenToWorldPoint(new Vector3(touchWorldPosition.x, touchWorldPosition.y, cam.nearClipPlane)), magnitude));
            _AimPrediction.toggleParticles(false);
            numZapsText.text = (--numZaps).ToString();
        }
    }
    private void increaseMaxPower(){}
    private void someting(Vector2 tp){
        Vector2 touchWorldPoint = cam.ScreenToWorldPoint(new Vector3(tp.x, tp.y, cam.nearClipPlane));
        playerChargeTime = (Time.time - lastTap) * 50;
        magnitude = (playerChargeTime > maxPower) ? maxPower : playerChargeTime;
        _AimPrediction.UpdateChargeBar(origin, tp, playerChargeTime, (magnitude/maxPower));
    }
    private void UpdateMobile(){
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            Vector3 touchWorldPosition = touch.position;
            // Handle finger movements based on touch phase.
            switch (touch.phase) {
                case TouchPhase.Began: // Record initial touch position.
                    lastTap = Time.time;
                    _AimPrediction.toggleParticles(true);
                    Reticle.transform.position = touchWorldPosition;
                    _FadeReticle.fade(true);
                    break;

                case TouchPhase.Stationary:
                    someting(touch.position);
                    break;
                
                case TouchPhase.Moved: // Determine direction by comparing the current touch position with the initial one.
                    someting(touch.position);
                    Reticle.transform.position = touchWorldPosition;
                    break;

                case TouchPhase.Ended: // Finger released.
                    _FadeReticle.fade(false);
                    playerChargeTime = (Time.time - lastTap) * 50;
                    magnitude = (playerChargeTime > maxPower) ? maxPower : playerChargeTime;
                    StartCoroutine(_Lightning.zap(transform.position, cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane)), magnitude));
                    _AimPrediction.toggleParticles(false);
                    numZapsText.text = (--numZaps).ToString();
                    break;
            }
                //Vector2 touchWorld = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));
                //Vector2 touchDeltaPosWorld = cam.ScreenToWorldPoint(new Vector3((touch.position - touch.deltaPosition).x, (touch.position - touch.deltaPosition).y, cam.nearClipPlane));
                //Debug.DrawLine(touchWorld, touchDeltaPosWorld, Color.red, 2f);
        }
    }
}
