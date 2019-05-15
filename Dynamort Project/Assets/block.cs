using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        initializePositionAndMaterial(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initializePositionAndMaterial(Vector2 pos){
        transform.position = pos;
        material.SetVector("_offset", pos/10);


    }
}
