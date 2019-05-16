using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AimPrediction : MonoBehaviour
{
    public Camera cam;
    public Image maxChargebar;
    public Image currentChargebar;
    public GameObject ratioTextGO;
    private TextMeshProUGUI ratioTextTMPUGUI;
    private ParticleSystem predictionPS;
    void Start()
    {
        predictionPS = GetComponent<ParticleSystem>();
        ratioTextTMPUGUI = ratioTextGO.GetComponent<TMPro.TextMeshProUGUI>();
        currentChargebar.rectTransform.localScale = new Vector3(1,0,1);
    }

    public void UpdateChargeBar(Vector2 originWorld, Vector2 endWorld, float playerChargeTime, float chargeRatio){
        Vector2 originScreen = cam.WorldToScreenPoint(new Vector3(originWorld.x, originWorld.y, cam.nearClipPlane));
        //maxChargebar.rectTransform.position = originScreen;
        Vector2 dir = (endWorld-originScreen).normalized;
        //maxChargebar.rectTransform.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,dir));
        var sh = predictionPS.shape;
        sh.rotation = new Vector3(0,0,Vector2.SignedAngle(Vector2.up,dir));
        var m = predictionPS.main;
        //print("playerChargeTime: " + playerChargeTime);
        //m.startLifetime = playerChargeTime/m.startSpeed.constant;
        currentChargebar.rectTransform.localScale = new Vector3(1,chargeRatio,1);
        ratioTextTMPUGUI.text = (chargeRatio*100).ToString("F0") + '%';
    }
    public void clearParticles(){
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[0];
        predictionPS.SetParticles(p);
        var m = predictionPS.main;
        m.startLifetime = 0;
    }
    public void spawnParticles(float playerChargeTime){
        var m = predictionPS.main;
        m.startLifetime = 0;
        m.startLifetime = playerChargeTime/m.startSpeed.constant;
    }
}
