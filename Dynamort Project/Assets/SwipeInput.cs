using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    public bool PCdebug = true;
    public Camera cam;
    public int maxPower = 500;
    [SerializeField] private Lightning Lightning;
    [SerializeField] private float deadzone = 100.0f;
    [SerializeField] private float doubleTapDelta = 0.5f;
    private bool tap, doubleTap;
    private Vector2 swipeDelta, startTouch, endTouch;
    private float lastTap;
    private float sqrDeadzone;

    public bool Tap { get { return tap; } }
    public bool DoubleTap { get { return doubleTap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }

    Vector2 origin;
    float playerChargeTime, magnitude;
    
    private AimPrediction _AimPrediction;
    void Start()
    {
        sqrDeadzone = deadzone * deadzone;
        origin = new Vector2(35,0);
        _AimPrediction = GetComponent<AimPrediction>();
    }

    // Update is called once per frame
    void Update()
    {
        tap = doubleTap = false;
//#if UNITY_EDITOR
if(PCdebug){ UpdateStandalone(); }
//#else
else{ UpdateMobile(); }
//#endif
        
    }
    private void UpdateStandalone(){
        /* 
            if (Input.GetMouseButtonDown(0)){
                tap = true;
                startTouch = Input.mousePosition;
                path.Add(startTouch);
                //doubleTap = Time.time - lastTap < doubleTapDelta;
                lastTap = Time.time;
            }
            else if (Input.GetMouseButton(0)){ // Input held down
                // mod 0.05 comes out to about every 0.1s
                if((Time.time)%0.05 < 0.01 && (Time.time)%0.05 > -0.01){
                    path.Add(Input.mousePosition);
                }
            }
            else if (Input.GetMouseButtonUp(0)){
                print("up");
                endTouch = Input.mousePosition;
                path.Add(endTouch);
                drawSwipeRay(startTouch, endTouch);
                float tapDeltaTime = Time.time - lastTap;
                //Lightning.zap(path, tapDeltaTime);
                startTouch = swipeDelta = Vector2.zero;
                path.Clear();
            }
            swipeDelta = Vector2.zero;

            if(startTouch != Vector2.zero && Input.GetMouseButton(0)){
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
                //print("swipeDelta: " + swipeDelta);
            }
            if(swipeDelta.sqrMagnitude > sqrDeadzone){
                //float x = swipeDelta.x, y = swipeDelta.y; print("swipe(x,y): (" + x + "," + y + ")");
                //print("swipeDelta: " + swipeDelta);
            }
            */
    }
    private void increaseMaxPower(){
        //maxPower
    }
    private void someting(Vector2 tp){
        Vector2 touchWorldPoint = cam.ScreenToWorldPoint(new Vector3(tp.x, tp.y, cam.nearClipPlane));
        Vector2 dir = (touchWorldPoint - origin).normalized;
        playerChargeTime = (Time.time - lastTap) * 10;
        print("playerChargeTime: " + playerChargeTime);
        magnitude = (playerChargeTime > maxPower) ? maxPower : playerChargeTime;
        float chargeRatio = magnitude/maxPower;
        _AimPrediction.UpdateChargeBar(origin, tp, chargeRatio);
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, magnitude);
        if(hit.collider != null){
            Debug.DrawLine(origin, hit.point , Color.red, 3f);
        } else {
            Debug.DrawLine(origin, origin + dir * magnitude , Color.red, 3f);
        }
    }
    private void UpdateMobile(){
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            //Vector3 touchWorldPosition = cam.ScreenToViewportPoint(touch.position);
            Vector3 touchWorldPosition = touch.position;
            // Handle finger movements based on touch phase.
            switch (touch.phase) {
                case TouchPhase.Began: // Record initial touch position.
                    //startPos = touch.position;
                    //inputPath.Add(touch.position);
                    lastTap = Time.time;
                    break;

                case TouchPhase.Stationary:
                    someting(touch.position);
                    /* 
                    Vector2 touchWorldPoint = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));
                    Vector2 dir = (touchWorldPoint - origin).normalized;
                    playerChargeTime = (Time.time - lastTap) * 10;
                    print("playerChargeTime: " + playerChargeTime);
                    magnitude = (playerChargeTime > maxPower) ? maxPower : playerChargeTime;
                    RaycastHit2D hit = Physics2D.Raycast(origin, dir, magnitude);
                    if(hit.collider != null){
                        Debug.DrawLine(origin, hit.point , Color.red, 3f);
                    } else {
                        Debug.DrawLine(origin, origin + dir * magnitude , Color.red, 3f);
                    }*/
                    break;
                
                case TouchPhase.Moved: // Determine direction by comparing the current touch position with the initial one.
                    //speedSqrMag = (touch.deltaPosition/touch.deltaTime).sqrMagnitude;
                    //vals += speedSqrMag; numVals++;
                    someting(touch.position);
                    //_AimPrediction.UpdateChargeBar();

                    /* 
                    Vector2 touchWorldPoint = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));
                    Vector2 dir = (touchWorldPoint - origin).normalized;
                    playerChargeTime = (Time.time - lastTap) * 10;
                    print("playerChargeTime: " + playerChargeTime);
                    magnitude = (playerChargeTime > maxPower) ? maxPower : playerChargeTime;
                    RaycastHit2D hit = Physics2D.Raycast(origin, dir, magnitude);
                    if(hit.collider != null){
                        Debug.DrawLine(origin, hit.point , Color.red, 3f);
                    } else {
                        Debug.DrawLine(origin, origin + dir * magnitude , Color.red, 3f);
                    }
                    */
                    //inputPath.Add(touch.position);
                    break;

                case TouchPhase.Ended: // Finger released.
                    //speedSqrMag = (touch.deltaPosition/touch.deltaTime).sqrMagnitude;
                    //vals += speedSqrMag; numVals++;

                    //inputPath.Add(touch.position);
                    float tapDeltaTime = Time.time - lastTap;
                    print("tapDeltaTime: " + tapDeltaTime);
                    playerChargeTime = (Time.time - lastTap) * 10;
                    magnitude = (playerChargeTime > maxPower) ? maxPower : playerChargeTime;
                    Lightning.zap(cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane)), magnitude);
                    //InputPath.Clear(); // THIS FUCKS EVERYTHING UP

                    ///print("Avg speed: " + vals/numVals); vals = 0f; numVals =0;
                    break;
            }
                Vector2 touchWorld = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));
                Vector2 touchDeltaPosWorld = cam.ScreenToWorldPoint(new Vector3((touch.position - touch.deltaPosition).x, (touch.position - touch.deltaPosition).y, cam.nearClipPlane));
                Debug.DrawLine(touchWorld, touchDeltaPosWorld, Color.red, 2f);
        }
    }
}
