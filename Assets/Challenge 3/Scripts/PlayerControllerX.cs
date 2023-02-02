using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;

    //GAME variables
    public int playerCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        //Missing to assing rigidbody component to playerRb
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }
        //Check if player in bounds
        StayInBound();
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            GameOver();
            Destroy(other.gameObject); //Destroy bomb
        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerCounter++; //Increase player score
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Debug.Log($"Current score: {playerCounter}");
            Destroy(other.gameObject); //Collect coin
        }
        // if player collides with ground and not game over, game over
        else if (other.gameObject.CompareTag("Ground") && !gameOver) {
            GameOver();
        }

    }

    //Function that make the player stay in bounds
    private void StayInBound() {
        if (transform.position.y > 15) {
            playerRb.velocity = new Vector3(0, 0, 0);
        }
    }

    //Function that manages Game Over
    private void GameOver() {
        explosionParticle.Play();
        playerAudio.PlayOneShot(explodeSound, 1.0f);
        gameOver = true;
        Debug.Log("Game Over!");
    }

}
