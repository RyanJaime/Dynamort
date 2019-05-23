using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public bool debug = true;
    public ParticleSystem lightningPrefab;
    public GameObject sparkPrefab;
    public GameObject lightPrefab;
    private ParticleSystem sparkPS;
    private ParticleSystem lightningPS;
    
    private Screenshake _screenshake;
    public Camera cam;
    public float fdelay = 0.1f;
    private WaitForSeconds wait, holdOn;
    void Start(){
        wait = new WaitForSeconds(fdelay);
        holdOn = new WaitForSeconds(fdelay*5);
        _screenshake = cam.GetComponent<Screenshake>();
    }

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


        emitLightningPathParticle(lightningPS, origin, 30);

        Vector2 direction = (playerInputPoint - origin).normalized;
        Debug.DrawLine(origin, playerInputPoint, Color.red, 5f);
        RayData rd = new RayData(origin, direction, magnitude);
        rd = drawPlayerChosenDirection(rd);
        yield return holdOn;
        for (int i = 0; i < 10; i++) {
            rd = drawRandomPathAndBounces(rd);
            yield return holdOn;
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
            emitLightningPathParticle(lightningPS, rd.origin, 1);// + hit2dNormal * 0.1f);
            _screenshake.increaseTrauma(0.5f, -hit.normal);
            GameObject sparkGO = Instantiate(sparkPrefab, hit.point, Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,hit.normal)));
            StartCoroutine(spawnLight(hit.point));
            
            
            ParticleSystem spark = sparkGO.GetComponent<ParticleSystem>();
            spark.Play();
            Destroy(sparkGO,spark.main.duration + spark.main.startLifetime.constantMax);

        } else {
            rd.origin += (rd.direction * rd.magnitude);
            emitLightningPathParticle(lightningPS, rd.origin, 30);
        }
        return rd;
    }
    private IEnumerator spawnLight(Vector3 hit){
        GameObject lightGO = Instantiate(lightPrefab, new Vector3(hit.x, hit.y, -10), Quaternion.identity);
        Light light = lightGO.GetComponent<Light>();
        float t = 0;
        float startIntensity = light.intensity;
        while(light.intensity > 0){
            light.intensity = Mathf.SmoothStep(startIntensity, 0, t);
            t += Time.deltaTime;
            yield return wait;
        }
        Destroy(lightGO);
        yield return null;
    }
    private RayData drawRandomPathAndBounces(RayData rd){
        print("rd.hit: " + rd.hit);
        if(rd.hit == true){ // handle reflection
            RaycastHit2D hit = Physics2D.Raycast(rd.origin, rd.direction, rd.magnitude);

            if(hit.collider != null) { // bounced off one wall into another wall
                Debug.DrawLine(rd.origin, rd.origin + rd.direction * rd.magnitude, Color.red, 3f); print("bounced off one wall into another wall");
                rd.direction = Vector2.Reflect((hit.point - rd.origin).normalized, hit.normal);
                emitLightningPathParticle(lightningPS, hit.point, 1);
                rd.origin = hit.point + hit.normal * 0.01f;
                _screenshake.increaseTrauma(0.5f, -hit.normal);

                StartCoroutine(spawnLight(hit.point));
                GameObject sparkGO = Instantiate(sparkPrefab, hit.point, Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,hit.normal)));
                ParticleSystem spark = sparkGO.GetComponent<ParticleSystem>();
                spark.Play();
                Destroy(sparkGO,spark.main.duration + spark.main.startLifetime.constantMax);
            } else { // bounced off wall into space
                Debug.DrawLine(rd.origin, rd.origin + rd.direction * rd.magnitude, Color.green, 3f); print("bounced off wall into space");
                rd.origin += (rd.direction * rd.magnitude);
                emitLightningPathParticle(lightningPS, rd.origin, 30);
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
                emitLightningPathParticle(lightningPS, rd.origin, 1);
                _screenshake.increaseTrauma(0.5f, -hit.normal);

                StartCoroutine(spawnLight(hit.point));
                GameObject sparkGO = Instantiate(sparkPrefab, hit.point, Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,hit.normal)));
                ParticleSystem spark = sparkGO.GetComponent<ParticleSystem>();
                spark.Play();
                Destroy(sparkGO,spark.main.duration + spark.main.startLifetime.constantMax);
                rd.hit = true;
            } else {
                print("random ray hit space");
                rd.origin += (rd.direction * rd.magnitude);
                emitLightningPathParticle(lightningPS, rd.origin, 30);
            }
        }
        return rd;
    }
    private void emitLightningPathParticle(ParticleSystem lightningPS, Vector2 particlePos, float scale){
        ParticleSystem.EmitParams emit = new ParticleSystem.EmitParams(); 
        emit.position = particlePos;
        //emit.startSize3D = new Vector2(1,scale);
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
