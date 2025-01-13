using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class GameManager : MonoBehaviour
{
    private GameObject Player;
    private bool HealthStatus; 
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("MainPlayer");
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthStatus = Player.gameObject.GetComponent<PlayerBehavior>().Death();
        Debug.Log(HealthStatus);
        if (HealthStatus)
            SceneManager.LoadScene(0);
            Debug.Log("Death Works");
    }
}
