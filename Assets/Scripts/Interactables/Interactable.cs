using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
    
    /// <summary>
    /// The GameObject (Canvas) for interaction.
    /// </summary>
    public GameObject interactionPromptPrefab;

    /// <summary>
    /// The GameObject instance of the Interaction Prompt.
    /// </summary>
    private GameObject interactionPrompt;

    private UnityEvent interaction = new UnityEvent();

    private Transform interactionOrigin;

    /// <summary>
    /// Prompts the interaction.
    /// </summary>
    private void PromptInteraction()
    {
        if (interactionPrompt != null)
            return;

        interactionPrompt = Instantiate(interactionPromptPrefab, (Vector2)transform.position + Vector2.up * .5f, 
            Quaternion.identity, transform);
    }

    /// <summary>
    /// Leaves the Interaction Prompt.
    /// </summary>
    private void LeavePromptInteraction()
    {
        Destroy(interactionPrompt);

        interactionPrompt = null;
    }

    /// <summary>
    /// Interacts with the Interactable.
    /// </summary>
	public void Interact()
    {
        //Interact here.
        interaction.Invoke();
    }

    public void AddInteraction(UnityAction action)
    {
        interaction.AddListener(action);
    }

    public void DeSelect()
    {
        interactionOrigin = null;
        LeavePromptInteraction();
    }

    public void Select(Transform origin)
    {
        interactionOrigin = origin;
        PromptInteraction();
    }

    public Transform GetInteractor()
    {
        return interactionOrigin;
    }
}
