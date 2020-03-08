using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float DieInThisManySeconds = 0.5f;
    void Start(){ Destroy(gameObject, DieInThisManySeconds); }
}
