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
    void Start()
    {
        ratioTextTMPUGUI = ratioTextGO.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void UpdateChargeBar(Vector2 originWorld, Vector2 endWorld, float chargeRatio){
        Vector2 originScreen = cam.WorldToScreenPoint(new Vector3(originWorld.x, originWorld.y, cam.nearClipPlane));
        Vector2 endScreen = cam.WorldToScreenPoint(new Vector3(endWorld.x, endWorld.y, cam.nearClipPlane));
        maxChargebar.rectTransform.position = originScreen;
        print("Vector2.Angle(originWorld,endWorld): " + Vector2.Angle(originWorld,endWorld) +
        " Vector2.SignedAngle(originWorld,endWorld)): " + Vector2.SignedAngle(originWorld,endWorld) +
        " Vector2.Angle(originScreen,endWorld)"  + Vector2.Angle(originScreen,endScreen) +
        " Vector2.SignedAngle(originScreen,endScreen)" + Vector2.SignedAngle(originScreen,endScreen));
        maxChargebar.rectTransform.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(originScreen,endScreen)-90);
        currentChargebar.rectTransform.localScale = new Vector3(1,chargeRatio,1);
        ratioTextTMPUGUI.text = (chargeRatio*100).ToString() + '%';
    }
}
