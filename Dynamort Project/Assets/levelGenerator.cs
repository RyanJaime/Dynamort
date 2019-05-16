using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour
{
    public GameObject block;
    void Start()
    {
        //FF = 255 = 1111 1111
        //byte[] bytes = new byte[]{0xff, 0xfe, 0xfc, 0xf8};
        //byte[] bytes = new byte[]{0x0,0x0,0x0,0x0,0x0,0x0,0xf8,0xfc,0xfe, 0xff};
        //byte[] bytes = new byte[]{0xE7, 0x81, 0x81, 0x81, 0x81, 0x81, 0x81, 0x81, 0x81, 0x81, 0x81, 0x81, 0xff};
        byte[] bytes = new byte[]{0x00, 0x00, 0x02, 0x02, 0x02, 0x02, 0x40, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00};
        for(int i=0; i < bytes.Length; i++){
            for(int j=0; j < 8; j++){
                if(getBit(bytes[i], j)){
                    Instantiate(block, new Vector2((7-j)*10,i*10),Quaternion.identity);
                }
            }
        }
    }

    public bool getBit(byte b, int bitNumber)
    {
        return ((b & (1 << bitNumber)) != 0);

    }
}
