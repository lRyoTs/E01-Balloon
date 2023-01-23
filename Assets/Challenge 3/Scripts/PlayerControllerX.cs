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
        if (Input.GetKey(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse); //Missing ForceMode.Impulse
        }
        StayInBounds(1,15);
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            GameOver();
            Destroy(other.gameObject); //Destroy object collided with
        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }

    }

    //Function that makes the player stay in bounds
    private void StayInBounds(float lowBound,float upBound) {
        //Check if gameOver
        if (!gameOver)
        {
            if (transform.position.y > upBound)
            {
                transform.position = new Vector3(-3, upBound, 0);
            }
            else if (transform.position.y < lowBound)
            {
                transform.position = new Vector3(-3, lowBound, 0);
            }
        }
        /*
        else {
            if (transform.position.y < 0) {
                Destroy(gameObject); //Destroy player once gameOver
            }
        }
        */
    }

    //Function that manages Game Over
    private void GameOver() {
        explosionParticle.Play();
        playerAudio.PlayOneShot(explodeSound, 1.0f);
        gameOver = true;
        Debug.Log("Game Over!");
    }

}
