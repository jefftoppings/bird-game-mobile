using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();

    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    private Rigidbody2D rigidBody;
    private Quaternion downRotation;
    private Quaternion forwardRotation;

    private GameManager game;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0,0,-90);
        forwardRotation = Quaternion.Euler(0,0,35);
        game = GameManager.Instance;
    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (game.GameOver) return;   
        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = forwardRotation;
            rigidBody.velocity = Vector3.zero;
            rigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "ScoreZone":
                // register score event, maybe play a sound
                OnPlayerScored(); // send event to GameManager
                break;
            case "DeadZone":
                rigidBody.simulated = false;
                OnPlayerDied(); // send event to GameManager
                // register dead event, play sound
                break;
            default:
                break;
        }
    }
}
