using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] PlayerInput input;
    InputAction jump;
    InputAction move;
    InputAction pause;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
            Destroy(gameObject);
        player = GameObject.Find("Player");
        input = GetComponent<PlayerInput>();
        jump = input.actions.FindAction("jump");
        move = input.actions.FindAction("move");
        //pause = input.actions.FindAction("pause");
        jump.performed += ctx => Debug.Log("Jump Ran!");
    }

    void Update()
    {
        Vector2 moveDir = move.ReadValue<Vector2>();
        Debug.Log("Test Move: " + moveDir);
        player.GetComponent<PlayerMovement>().MovePlayer(moveDir);
        //move.performed += ctx => player.GetComponent<PlayerMovement>().MovePlayer(moveDir);

    }
    void OnEnable()
    {
        jump.Enable();
        move.Enable();
        Debug.Log("Enabled!");
        //pause.Enable();
    }

    void OnDisable()
    {
        jump.Disable();
        move.Disable();
        //pause.Disable();
    }



    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("InputManager");
                    _instance = singletonObject.AddComponent<InputManager>();
                }
            }
            return _instance;
        }
    }
}
