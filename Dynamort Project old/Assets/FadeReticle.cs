using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeReticle : MonoBehaviour
{
    private bool interupt = false;
    public void fade(bool In){
        if (In) StartCoroutine(fadeIn());
        else StartCoroutine(fadeOut());
    }
    public IEnumerator fadeIn(){
        Color reticleCol = gameObject.GetComponent<SVGImage>().color;
        float startColAlpha = reticleCol.a;
        float endColAlpha = 1f;
        float t = 0;
        
        while(reticleCol.a != endColAlpha) {
            if(!interupt) {
                reticleCol.a = Mathf.SmoothStep(startColAlpha, endColAlpha, t);
                gameObject.GetComponent<SVGImage>().color = new Color(1,1,1,reticleCol.a );
                t += Time.deltaTime * 3;
                yield return null;
            }
            else break;
        }
        yield return null;
    }
    public IEnumerator fadeOut(){
        Color reticleCol = gameObject.GetComponent<SVGImage>().color;
        float startColAlpha = reticleCol.a;
        float endColAlpha = 0f;
        float t = 0;
        interupt = true;
        while(reticleCol.a != endColAlpha) {
            reticleCol.a = Mathf.SmoothStep(startColAlpha, endColAlpha, t);
            gameObject.GetComponent<SVGImage>().color = new Color(1,1,1,reticleCol.a );
            t += Time.deltaTime * 3;
            yield return null;
        }
        interupt = false;
        yield return null;
    }
}
