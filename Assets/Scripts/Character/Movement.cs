using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    /// <summary>
    /// The rigidbody2D's instance.
    /// </summary>
    Rigidbody2D rb = null;

    /// <summary>
    /// The animator.
    /// </summary>
    Animator anim = null;

    /// <summary>
    /// The Aiming Direction.
    /// </summary>
    public Vector2 AimDir { get; set; }

    /// <summary>
    /// The Movement Direction.
    /// </summary>
    Vector2 moveDir;

    /// <summary>
    /// The character's movement speed.
    /// </summary>
    [SerializeField]
    float moveSpeed = 4;

    /// <summary>
    /// The current selected interactable if any.
    /// </summary>
    private Interactable currentSelectedInteractable;

    /// <summary>
    /// The interaction radius for the collection of interactables.
    /// </summary>
    public float interactionRadius;

    /// <summary>
    /// Determines if the character can dash
    /// </summary>
    private bool canDash = true;

    public float dashCooldown;

    private float dashDuration = 0.2f;

    private bool canMove = true;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

       
        AimDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        AimDir = AimDir.normalized; moveDir = new Vector2(h, v).normalized;

        anim.SetFloat("moveX", h);
        anim.SetFloat("moveY", v);

        if (canMove)
        {
            if (moveDir.magnitude > 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) && canDash)
                {
                    canDash = false;
                    StartCoroutine(DashCooldown());
                    StartCoroutine(Dash(moveDir));
                }
                else
                    Move(moveDir.normalized, moveSpeed * Time.deltaTime);
            }

            

            ManageInteraction();
        }

    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator Dash(Vector2 direction)
    {
        canMove = false;

        float currentTime = Time.time;
        do
        {
            rb.MovePosition(rb.position + direction * Time.deltaTime * moveSpeed * 3);
            yield return new WaitForEndOfFrame();
        } while ((Time.time - currentTime) < dashDuration);

        canMove = true;
    }

    /// <summary>
    /// Manages the interaction (collision and key).
    /// Current key is 'E' for interaction.
    /// </summary>
    private void ManageInteraction()
    {
        Collider2D[] potentialInteractables = new Collider2D[5];

        if (Physics2D.OverlapCircleNonAlloc(transform.position, interactionRadius, potentialInteractables) > 0)
        {
            float curDistance = float.MaxValue;
            Interactable newInteractable = null;

            foreach (Collider2D col in potentialInteractables)
            {
                if (col != null)
                {
                    Interactable interactableComponent = col.GetComponent<Interactable>();
                    float newDistance = Vector2.Distance(transform.position, col.transform.position);

                    if (interactableComponent != null && newDistance < curDistance)
                    {
                        newInteractable = interactableComponent;
                        curDistance = newDistance;
                    }
                }
            }

            if (newInteractable == null)
            {
                if (currentSelectedInteractable != null)
                    currentSelectedInteractable.DeSelect();

                currentSelectedInteractable = null;
            }

            if (currentSelectedInteractable != newInteractable)
            {
                if (currentSelectedInteractable != null)
                {
                    currentSelectedInteractable.DeSelect();
                }
                currentSelectedInteractable = newInteractable;
                currentSelectedInteractable.Select(transform);
            }

        }

        if (Input.GetKeyDown(KeyCode.E) && currentSelectedInteractable != null)
            currentSelectedInteractable.Interact();
    }

    /// <summary>
    /// Moves the character to specified vector and speed.
    /// </summary>
    /// <param name="moveDir">the 2d vector representing the direction.</param>
    /// <param name="speed">the speed to move the direction to.</param>
    private void Move(Vector2 direction, float speed)
    {
        rb.MovePosition(rb.position + direction * speed);

        anim.SetFloat("dirX", moveDir.x);
        anim.SetFloat("dirY", moveDir.y);
    }
}
