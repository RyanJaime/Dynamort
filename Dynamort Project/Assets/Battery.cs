using UnityEngine;
using UnityEngine.UI;
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

    public void increaseCharge(float percent){
        chargePercent += percent;
        FillMat.SetFloat("_chargePercent", chargePercent);
        EnergySlider.value = chargePercent;
    }
}
