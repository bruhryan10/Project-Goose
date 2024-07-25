using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    bool test;
    float timer;
    [SerializeField] Transform playerTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Restart"))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }
        if (other.CompareTag("test1"))
        {
            test = true;
        }
        if (other.CompareTag("test2"))
            test = false;
    }
    private void Update()
    {
        if (test)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
        }
/*        if (!test)
            Debug.Log(timer);*/
    }
}
