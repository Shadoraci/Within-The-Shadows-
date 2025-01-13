using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider Object)
    {
        if (Object.gameObject.tag == "Player") 
        {
            Debug.Log("Wendigo BOO!");
        }
    }
}
