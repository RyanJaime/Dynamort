using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    private float trauma = 1.0f;
    private float maxShake = 7.0f;
    private Vector3 startPos;
    private Quaternion startRot;
    private float xInfluence = 0;
    private float yInfluence = 0;
    void Start()
    {
         startPos = gameObject.transform.position;
         startRot = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if (trauma > 0){
            float t = Time.time;
            float noiseX = Mathf.PerlinNoise(t, t) * 2 - 1;
            float noiseY = Mathf.PerlinNoise(t*2, t*2) * 2 - 1;
            float offsetX = maxShake * trauma*trauma * noiseX + xInfluence;
            float offsetY = maxShake * trauma*trauma * noiseY + yInfluence;
            gameObject.transform.position = startPos + new Vector3(offsetX,offsetY,0);
            trauma -= Time.deltaTime*1.5f;
            xInfluence *= trauma;// -= Time.deltaTime*1.5f;
            yInfluence *= trauma;//-= Time.deltaTime*1.5f;

            float noiseRotZ = Mathf.PerlinNoise(t*3, t*3) * 2 - 1;
            float offsetRotZ = maxShake * trauma * noiseRotZ;
            gameObject.transform.rotation = Quaternion.Euler(0,0,offsetRotZ);
        }
        else{
            gameObject.transform.position = startPos;
            gameObject.transform.rotation = Quaternion.identity;
            xInfluence = 0.0f;
            yInfluence = 0.0f;       
        }
    }
    public void increaseTrauma(float amount, Vector2 bounceDirection){
        if(trauma + amount < 1) { trauma += amount; }
        if (Mathf.Abs(xInfluence) < 1) { xInfluence += bounceDirection.x * 0.5f; }
        if (Mathf.Abs(yInfluence) < 1) { yInfluence += bounceDirection.y * 0.5f; }
    }
}
