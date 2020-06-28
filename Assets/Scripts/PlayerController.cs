using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public CharacterController2D controller;	// Reference for our Character Controller2D component
	public BoxCollider2D boxCollider; 			// Reference for the players Block Collider
	public CircleCollider2D circleCollider;		// Reference for the players Circle Collider
	public Animator animator;					// Reference for our animator, we will use this to manipulate the animations later

	public Text scoreText;			// Reference to the UI text object

    public float movementSpeed;		// The movement speed of our player, the variable is public so that it can be altered in the inspector

    int score; 						// a private score variable, to keep track of our score and write the value to the UI score object
    float horizontalMovementSpeed;	// The value of our horizontalMov speed
    bool isJumping = false; 		// Is the player jumping>?
    bool isHurt = false;			// Is the player hurt?

	// Use this for initialization
	void Start () {

		// Grab the components in case they weren't loaded properly
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();

		// initialize the start values
        isJumping = false;
        isHurt = false;
        score = 0;
	}

	// Update is called once per frame
	void Update () {

		// Update the score
		scoreText.text = string.Format("Score: {0}", score.ToString());

		// Get the movement speed value depending on our input multiplied by the movement speed
		horizontalMovementSpeed = Input.GetAxisRaw("Horizontal") * movementSpeed;

        // Cast the value to the animatorator, so that we can swap animations
        animator.SetFloat("speed", Mathf.Abs(horizontalMovementSpeed));

        // Did the player hit the jump key?
        if (Input.GetButtonDown("Jump")) {
            isJumping = true;
            animator.SetBool("isJumping", true);
        }
	}

	// Called when we hit the ground, set isjumping to false, tell the animator to update the animations
    public void OnLanding() {
	    isJumping = false;
	    animator.SetBool("isJumping", false);
    }

	// Check for collisions with other actors
    void OnTriggerEnter2D(Collider2D collider) {
		// Are we colliding with a gem? if so, increase our score, destroy the gem.
        if (collider.gameObject.tag == "Gem") {
            score++;
            Destroy(collider.gameObject);
        }
		// Are we colliding with an enemy?
        else if (collider.gameObject.tag == "Enemy") {
	        // If we are jumping, destroy the enemy, increase our score.
            if (isJumping) {
                Destroy(collider.gameObject);
                score++;
            }
            // Else set our hurt animation to true, and disable collisions.
            else {
                animator.SetBool("isHurt", true);
                boxCollider.enabled = false;
                circleCollider.enabled = false;
            }
        }
    }

	// Used to calculate our movement, fixedUpdate() will make sure the player moves the same on all machines regardless of the framerate
    void FixedUpdate() {
	    controller.Move(horizontalMovementSpeed * Time.fixedDeltaTime, false, isJumping);
    }
}
