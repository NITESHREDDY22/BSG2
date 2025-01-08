using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectDeliverableItem : MonoBehaviour
{
    public GameObject Prefab;
    public Transform SpawnParent;
    public Transform SpawnPrefabPos;
    bool PickedUp = false;

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "DeliverableItem")
        {
            if (!PickedUp)
            {
                GameObject Go = Instantiate(Prefab, SpawnPrefabPos.position, Quaternion.identity);
                Go.transform.parent = SpawnParent.transform;
                PickedUp = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        /*if (collision.gameObject.tag == "DeliverableItem")
        {
            PickedUp = false;
        }*/
    }
}
