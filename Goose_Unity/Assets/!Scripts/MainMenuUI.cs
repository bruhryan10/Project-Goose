using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Canvas mainUI;
    [SerializeField] GameObject credits;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Credits()
    {
        if (credits.activeInHierarchy)
            credits.SetActive(false);
        else
            credits.SetActive(true);
    }
}
