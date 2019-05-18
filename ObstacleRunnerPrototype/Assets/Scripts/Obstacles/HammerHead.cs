using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHead : MonoBehaviour
{
    private int layerID;
    private int inversedLayerID;
    public float range;

    private void Awake()
    {
        layerID = LayerMask.NameToLayer("Pathway");
        inversedLayerID = 1 << layerID;
    }

    public bool RaycastPathway()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position,Vector3.down,out hit, range, inversedLayerID))
        {
            if(hit.collider.gameObject.layer == layerID)
                return true;
        }
        return false;
    }

}
