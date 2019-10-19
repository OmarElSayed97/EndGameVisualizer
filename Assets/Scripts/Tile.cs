using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    
    protected Material TileMaterial;
    // Start is called before the first frame update
    void Start()
    {
        TileMaterial = GetComponent<Material>();
    }

    
}
