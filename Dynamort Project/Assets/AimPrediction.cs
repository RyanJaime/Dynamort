using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AimPrediction : MonoBehaviour
{
    public GameObject aimPS;
    public Camera cam;
    public Image maxChargebar;
    public Image currentChargebar;
    public GameObject ratioTextGO;
    private TextMeshProUGUI ratioTextTMPUGUI;
    private ParticleSystem predictionPS;
    private List<ParticleCollisionEvent> collisionEvents;
    private int bounceAim =0;
    private ParticleSystem bouncePS;
    public GameObject AimPredictionGO;
    public GameObject aimBounceGO;
    private Vector2 dir = Vector2.zero;
    private float spawnTime = 0f;
    //Quaternion firstHitAngle = Quaternion.identity;
    void Awake(){
        addBounceAim();
    }
    void Start()
    {
        predictionPS = AimPredictionGO.GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        ratioTextTMPUGUI = ratioTextGO.GetComponent<TMPro.TextMeshProUGUI>();
        currentChargebar.rectTransform.localScale = new Vector3(1,0,1);
        
    }

    public void UpdateChargeBar(Vector2 originWorld, Vector2 endWorld, float playerChargeTime, float chargeRatio){
        if (spawnTime <= 0){
            predictionPS.Emit(new ParticleSystem.EmitParams(), 1);
            spawnTime = 0.2f;
        }
        else{ spawnTime -= Time.deltaTime; }

        Vector2 originScreen = cam.WorldToScreenPoint(new Vector3(originWorld.x, originWorld.y, cam.nearClipPlane));
        dir = (endWorld-originScreen).normalized;
        var sh = predictionPS.shape; // -39.081
        Quaternion firstHitAngle = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,dir));
        //sh.rotation = new Vector3(0,0,Vector2.SignedAngle(Vector2.up,dir));
        predictionPS.transform.rotation = firstHitAngle;
        var m = predictionPS.main;
        currentChargebar.rectTransform.localScale = new Vector3(1,chargeRatio,1);
        ratioTextTMPUGUI.text = (chargeRatio*100).ToString("F0") + '%';
    }
    public void clearParticles(){
        AimPredictionGO.SetActive(false);
        aimBounceGO.SetActive(false);
    }
    public void enableParticles(){
        AimPredictionGO.SetActive(true);
        aimBounceGO.SetActive(true);
    }
    public void spawnParticles(float playerChargeTime){
        var m = predictionPS.main;
        m.startLifetime = 0;
        m.startLifetime = playerChargeTime/m.startSpeed.constant;
    }
    void OnParticleCollision(GameObject other){
        predictionPS.GetCollisionEvents(other, collisionEvents);
        print(collisionEvents[collisionEvents.Count-1].intersection);
        Vector3 nor = collisionEvents[collisionEvents.Count-1].normal;
        bouncePS.transform.position = collisionEvents[collisionEvents.Count-1].intersection + nor;
        Quaternion hitAngle = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,Vector2.Reflect(dir,nor)));
        bouncePS.transform.rotation = hitAngle;
        bouncePS.Emit(new ParticleSystem.EmitParams(), 1);
    }
    void addBounceAim(){
        aimBounceGO = Instantiate(aimPS, new Vector2(100,100), Quaternion.identity);
        aimBounceGO.transform.parent = gameObject.transform;
        bouncePS = aimBounceGO.GetComponent<ParticleSystem>();
        bounceAim++;
    }
}
