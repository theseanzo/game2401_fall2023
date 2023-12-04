using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyxPower : Unit
{
    public Animator animator; // Reference to the animator component
    public string animationName = "Attack1"; // Name of the animation to play
    public float speedMultiplier = 1.0f; // Speed multiplier for the animation
    public MeshRenderer[] bearRenderers;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bearRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the "1" key is pressed.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Trigger the animation.
            PlayAnimationAndTurnOffMeshRenderer();
        }
    }

    public void PlayAnimationAndTurnOffMeshRenderer()
    {
        // Play the animation.
        animator.Play(animationName);

        // Turn off the mesh renderer.
        foreach (MeshRenderer renderer in bearRenderers)
        {
            renderer.enabled = false;
        }
    }
}