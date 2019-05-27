using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Battery : MonoBehaviour
{
    public float chargePercent;
    public GameObject Fill, EnergyLevel;
    private Material FillMat;
    private Slider EnergySlider;
    void Start() {
        chargePercent = 0.0f;
        FillMat = Fill.GetComponent<Image>().material;
        EnergySlider = EnergyLevel.GetComponent<Slider>();
    }

    public IEnumerator increaseCharge(float percent) {
        float startPercent = chargePercent;
        percent = startPercent + percent;
        float t = 0.0f;
        while ( chargePercent < percent) {
            chargePercent = EnergySlider.value = Mathf.SmoothStep(startPercent, percent, t);
            FillMat.SetFloat("_chargePercent", chargePercent);
            t += 0.05f;
            yield return null;
            //yield return new WaitForSeconds(0.1f);
        }
        chargePercent = percent;
        EnergySlider.value = chargePercent;
        FillMat.SetFloat("_chargePercent", chargePercent);
        yield return null;
    }
}
