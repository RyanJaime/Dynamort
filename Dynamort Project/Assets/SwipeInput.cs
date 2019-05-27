using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    //public GameObject marker;
    public bool PCdebug = true;
    public Camera cam;
    private int maxPower = 100;
    [SerializeField] private Lightning _Lightning;
    private Vector2 origin;
    private float lastTap, playerChargeTime, magnitude;
    
    private AimPrediction _AimPrediction;
    void Start()
    {
        origin = new Vector2(35,0);
        _AimPrediction = GetComponentInChildren<AimPrediction>();
    }

    void Update()
    {
//#if UNITY_EDITOR
if(PCdebug){ UpdateStandalone(); }
//#else
else{ UpdateMobile(); }
//#endif
        
    }
    private void UpdateStandalone(){}
    private void increaseMaxPower(){}
    private void someting(Vector2 tp){
        Vector2 touchWorldPoint = cam.ScreenToWorldPoint(new Vector3(tp.x, tp.y, cam.nearClipPlane));
        //marker.transform.position = touchWorldPoint;
        Vector2 dir = (touchWorldPoint - origin).normalized;
        playerChargeTime = (Time.time - lastTap) * 50;
        //print("playerChargeTime: " + playerChargeTime);
        magnitude = (playerChargeTime > maxPower) ? maxPower : playerChargeTime;
        _AimPrediction.UpdateChargeBar(origin, tp, playerChargeTime, (magnitude/maxPower));
        /* RaycastHit2D hit = Physics2D.Raycast(origin, dir, magnitude);
        if(hit.collider != null){ Debug.DrawLine(origin, hit.point , Color.red, 3f); }
        else { Debug.DrawLine(origin, origin + dir * magnitude , Color.red, 3f); } */
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
                    break;

                case TouchPhase.Stationary:
                    someting(touch.position);
                    break;
                
                case TouchPhase.Moved: // Determine direction by comparing the current touch position with the initial one.
                    someting(touch.position);
                    break;

                case TouchPhase.Ended: // Finger released.
                    float tapDeltaTime = Time.time - lastTap;
                    //print("tapDeltaTime: " + tapDeltaTime);
                    playerChargeTime = (Time.time - lastTap) * 50;
                    magnitude = (playerChargeTime > maxPower) ? maxPower : playerChargeTime;
                    StartCoroutine(_Lightning.zap(cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane)), magnitude));
                    _AimPrediction.toggleParticles(false);
                    break;
            }
                //Vector2 touchWorld = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));
                //Vector2 touchDeltaPosWorld = cam.ScreenToWorldPoint(new Vector3((touch.position - touch.deltaPosition).x, (touch.position - touch.deltaPosition).y, cam.nearClipPlane));
                //Debug.DrawLine(touchWorld, touchDeltaPosWorld, Color.red, 2f);
        }
    }
}
