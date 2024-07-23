using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EventManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("EventManager");
                    _instance = singletonObject.AddComponent<EventManager>();
                }
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            Debug.Log("Instance!");
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
            Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
