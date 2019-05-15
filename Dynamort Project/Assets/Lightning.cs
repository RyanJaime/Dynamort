using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public bool debug = true;
    public ParticleSystem lightningPrefab;
    private ParticleSystem lightningPS;
    public Camera cam;
    public float fdelay = 0.1f;
    private WaitForSeconds wait, holdOn;
    private Vector2 randomDirection, randomPoint, mixedPointDirection, previousDrawnPoint, drawnPoint, hit2dPoint, hit2dNormal,
            currentMixedPoint,//   = headWorld, 
            previousMixedPoint,//  = headWorld, 
            previousRandomPoint;// = headWorld,
    bool endPlayerInput = false;
    float mixAmount;
    float swipeSpeed = 0.0f;
    void Start(){
        wait = new WaitForSeconds(fdelay);
        holdOn = new WaitForSeconds(fdelay*10);
    }

    //public void zap(List<Vector2> playerInputPoint, float playerHoldDownChargeTime, float maxChargeTime){  StartCoroutine(zapCo(playerInputPoint, playerHoldDownChargeTime, maxChargeTime)); }
    public void zap(Vector2 playerInputPoint, float magnitude){  StartCoroutine(zapCo(playerInputPoint, magnitude)); }
    struct RayData {
    public Vector2 origin, direction;
    public float magnitude;
    public bool hit;
    public RayData(Vector2 origin, Vector2 direction, float magnitude) {
        this.origin = origin;
        this.direction = direction;
        this.magnitude = magnitude;
        this.hit = false;
    }
 }
    public IEnumerator zapCo(Vector2 playerInputPoint, float magnitude){
        Vector2 origin = new Vector2(35,0);

        lightningPS = Instantiate(lightningPrefab, origin, Quaternion.identity);
        emitLightningPathParticle(lightningPS, origin);

        Vector2 direction = (playerInputPoint - origin).normalized;
        Debug.DrawLine(origin, playerInputPoint, Color.red, 5f);
        RayData rd = new RayData(origin, direction, magnitude);
        rd = drawPlayerChosenDirection(rd);
        yield return wait;
        for (int i = 0; i < 10; i++) {
            rd = drawRandomPathAndBounces(rd);
            yield return wait;
        }
        yield return null;
    }
    private RayData drawPlayerChosenDirection(RayData rd){
        RaycastHit2D hit = Physics2D.Raycast(rd.origin, rd.direction, rd.magnitude);

        if(hit.collider != null) {
            print("WEEW");
            rd.hit = true;
            rd.direction = Vector2.Reflect((hit.point - rd.origin).normalized, hit.normal);
            rd.origin = hit.point + hit.normal * 0.01f;
            emitLightningPathParticle(lightningPS, rd.origin);// + hit2dNormal * 0.1f);
            //if(debug) { displayDebugDrawsAndLogs(1); }
        } else {
            rd.origin += (rd.direction * rd.magnitude);
            emitLightningPathParticle(lightningPS, rd.origin);
        }
        return rd;
    }
    private RayData drawRandomPathAndBounces(RayData rd){
        print("rd.hit: " + rd.hit);
        if(rd.hit == true){ // handle reflection
            RaycastHit2D hit = Physics2D.Raycast(rd.origin, rd.direction, rd.magnitude);

            if(hit.collider != null) { // bounced off one wall into another wall
                Debug.DrawLine(rd.origin, rd.origin + rd.direction * rd.magnitude, Color.red, 3f); print("bounced off one wall into another wall");
                rd.direction = Vector2.Reflect((hit.point - rd.origin).normalized, hit.normal);
                emitLightningPathParticle(lightningPS, hit.point);
                rd.origin = hit.point + hit.normal * 0.01f;
                //if(debug) { displayDebugDrawsAndLogs(2); }
            } else { // bounced off wall into space
                Debug.DrawLine(rd.origin, rd.origin + rd.direction * rd.magnitude, Color.green, 3f); print("bounced off wall into space");
                rd.origin += (rd.direction * rd.magnitude);
                emitLightningPathParticle(lightningPS, rd.origin);
                rd.hit = false;
            }
        }
        else{ // draw random ray 
            rd.direction = (Random.insideUnitCircle + rd.direction).normalized;
            RaycastHit2D hit = Physics2D.Raycast(rd.origin, rd.direction, rd.magnitude);

            if(hit.collider != null) {
                print("random ray hit wall");
                rd.direction = Vector2.Reflect((hit.point - rd.origin).normalized, hit.normal);
                rd.origin = hit.point + hit.normal * 0.01f;
                emitLightningPathParticle(lightningPS, rd.origin);
                rd.hit = true;
            } else {
                print("random ray hit space");
                rd.origin += (rd.direction * rd.magnitude);
                emitLightningPathParticle(lightningPS, rd.origin);
            }
        }
        return rd;
    }
    private void emitLightningPathParticle(ParticleSystem lightningPS, Vector2 particlePos){
        ParticleSystem.EmitParams emit = new ParticleSystem.EmitParams();   // Create head EmitParams,
        emit.position = particlePos;
        lightningPS.Emit(emit, 1);
    }
    /*
    private void displayDebugDrawsAndLogs(int c){
        switch (c) {
        case 0:
            Debug.DrawLine(previousMixedPoint, drawnPoint, Color.red, 3f);
            Debug.DrawLine(previousMixedPoint, currentMixedPoint, Color.yellow, 3f);
            Debug.DrawLine(previousRandomPoint, randomPoint, Color.green, 3f);
            break;
        case 1:
            //print("hit2dNormal: (" +hit2dNormal.x + ", " + hit2dNormal.y + "). Mag: " + hit2dNormal.magnitude);
            Debug.DrawLine(previousMixedPoint, hit2dPoint, Color.gray, 3f); // ray to be reflected
            Debug.DrawLine(hit2dPoint, hit2dPoint + hit2dNormal, Color.magenta, 3f); // normal to reflect over
            Debug.DrawLine(hit2dPoint, hit2dPoint + reflectionDirection, Color.white, 3f); // reflection
            break;
        case 2:
            Debug.DrawLine(currentMixedPoint, hit2dPoint, Color.gray, 3f); // ray to be reflected
            Debug.DrawLine(hit2dPoint, hit2dPoint + hit2dNormal, Color.magenta, 3f); // normal to reflect over
            Debug.DrawLine(hit2dPoint, hit2dPoint + reflectionDirection, Color.white, 3f); // reflection
            break;
        }

    }
    */
}
