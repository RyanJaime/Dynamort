using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class Battery : MonoBehaviour
{
    public float fillSpeed = 0.05f;
    public float chargePercent;
    public Transform UICharge;
    public TextMeshProUGUI UIChargeText;
    public SVGImage FillColor;
    public GameObject uicharge, uichargetext;
    private Material energyGradientMat; //tint
    public Color lowestEnergy, lowerEnergy, lowEnergy, highEnergy;
    void Start(){
        initializeStuff(uicharge, uichargetext);
    }
    //public void initializeStuff(Transform uicharge, TextMeshProUGUI uichargetext, SVGImage fillcolorsvgimg){
    public void initializeStuff(GameObject uicharge, GameObject uichargetext){
        chargePercent = 0.0f;
        energyGradientMat = gameObject.GetComponent<Image>().material;
        UICharge = uicharge.GetComponent<RectTransform>();
        UIChargeText = uichargetext.GetComponent<TextMeshProUGUI>();
        FillColor = uicharge.GetComponent<SVGImage>();
        //tint = FillColor.GetComponent<Material>();
        print(UICharge + " " + UIChargeText + " " + FillColor);
    }
    public IEnumerator increaseCharge(float percent) {
        float startPercent = chargePercent;
        percent += startPercent;
        float t = 0.0f;
        while ( chargePercent < percent) {
            chargePercent = Mathf.SmoothStep(startPercent, percent, t);
            if  (chargePercent <= 1.0f) {
                gameObject.transform.localScale = new Vector3(1, chargePercent, 1);
                uicharge.transform.localScale = new Vector3(1, chargePercent, 1);
                UICharge.localScale = new Vector3(1, chargePercent, 1);
                UIChargeText.text = (chargePercent * 100).ToString("F0") +'%';
                setEnergyColor(chargePercent);
                
            } else { chargePercent = percent; }
            t += fillSpeed;
            yield return null;
        }
        chargePercent = percent;
        if  (chargePercent <= 1.0f) {
            gameObject.transform.localScale = new Vector3(1, chargePercent, 1);
            uicharge.transform.localScale = new Vector3(1, chargePercent, 1);
            UICharge.localScale = new Vector3(1, chargePercent, 1);
            UIChargeText.text = (chargePercent * 100).ToString("F0") +'%';
            setEnergyColor(chargePercent);
        }
        yield return null;
    }
    private void setEnergyColor(float chargePercent){
        if (chargePercent < 0.2) FillColor.color = lowestEnergy;
        else if (chargePercent < 0.4) FillColor.color = lowerEnergy;
        else if (chargePercent < 0.6) FillColor.color = lowEnergy;
        else FillColor.color = highEnergy;
        energyGradientMat.SetFloat("_chargePercent",chargePercent);
    }
}
