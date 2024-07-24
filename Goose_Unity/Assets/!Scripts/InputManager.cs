using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerMovement playerMovement;
    [SerializeField] PlayerInput input;
    InputAction jump;
    InputAction move;

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
        playerMovement = player.GetComponent<PlayerMovement>();

        input = GetComponent<PlayerInput>();
        jump = input.actions.FindAction("jump");
        move = input.actions.FindAction("move");
        jump.performed += ctx => playerMovement.PlayerJump();
        jump.canceled += ctx => playerMovement.SetFloatState(false);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main_Menu")
            return;

        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        if (jump.IsPressed())
            playerMovement.SetFloatState(true);

        if (player != null && move != null)
        {
            Vector2 moveDir = move.ReadValue<Vector2>();
            playerMovement.MovePlayer(moveDir);
        }
        else
            Debug.LogWarning("Player or move action is not set!");

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
