using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;                   // Reference to the animator component.
    public NavMeshAgent agent;                  // Reference to the nav mesh agent component.
    private SaveLoad saveLoad;
    public float turnSmoothing = 15f;           // The amount of smoothing applied to the player's turning using spherical interpolation.
    public float speedDampTime = 0.1f;          // The approximate amount of time it takes for the speed parameter to reach its value upon being set.
    public float slowingSpeed = 0.175f;         // The speed the player moves as it reaches close to it's destination.
    public float turnSpeedThreshold = 0.5f;     // The speed beyond which the player can move and turn normally.
    public float inputHoldDelay = 0.5f;         // How long after reaching an interactable before input is allowed again.

    private InteractionObject currentInteractable;   // The interactable that is currently being headed towards.
    public Vector3 destinationPosition;        // The position that is currently being headed towards, this is the interactionLocation of the currentInteractable if it is not null.
    public bool handleInput = true;            // Whether input is currently being handled.
    private WaitForSeconds inputHoldWait;       // The WaitForSeconds used to make the user wait before input is handled again.
	private Vector3 destinationRotation;

    public TrackCamManager camManager=null;


    private readonly int hashSpeedPara = Animator.StringToHash("Speed");
                                                // An hash representing the Speed animator parameter, this is used at runtime in place of a string.
    private readonly int hashLocomotionTag = Animator.StringToHash("Locomotion");
                                                // An hash representing the Locomotion tag, this is used at runtime in place of a string.


    public const string startingPositionKey = "starting position";
                                                // The key used to retrieve the starting position from the playerSaveData.


    private const float stopDistanceProportion = 0.1f;
                                                // The proportion of the nav mesh agent's stopping distance within which the player stops completely.
    private const float navMeshSampleDistance = 4f;
    // The maximum distance from the nav mesh a click can be to be accepted.

    GameState gs;
    TextManager tm;
    CamManager cm;

    bool comeFromForced = false;

    public void StopAll()
    {
        StopHighlight();
        StopInput();
    }

    public void ResumeAll()
    {
        ResumeHighlight();
        ResumeInput();
    }

    public void StopHighlight()
    {
        if(Camera.main.GetComponent<HighlightingSystem.Demo.RaycastController>()!=null)
            Camera.main.GetComponent<HighlightingSystem.Demo.RaycastController>().enabled = false;
    }

    public void ResumeHighlight()
    {
        if (Camera.main.GetComponent<HighlightingSystem.Demo.RaycastController>() != null)
            Camera.main.GetComponent<HighlightingSystem.Demo.RaycastController>().enabled = true;
    }

    public void StopInput() {
		handleInput=false;
	}

	public void ResumeInput() {
		handleInput=true;
	}

    private void Start()
    {
        saveLoad = FindObjectOfType<SaveLoad>();
        if (!saveLoad)
            throw new UnityException("SaveLoad could not be found, ensure that it exists in the Persistent scene.");

        gs = FindObjectOfType<GameState>();
        if (!gs)
            throw new UnityException("GameState could not be found, ensure that it exists in the Persistent scene.");

        tm = FindObjectOfType<TextManager>();
        if (!tm)
            throw new UnityException("TextManager could not be found, ensure that it exists in the scene.");

        cm = FindObjectOfType<CamManager>();
        if (!cm)
            throw new UnityException("CamManager could not be found, ensure that it exists in the Persistent scene.");

        agent.updateRotation = false;


        // Create the wait based on the delay.
		destinationPosition = transform.position;
        inputHoldWait = new WaitForSeconds (inputHoldDelay);

        // Load the starting position from the save data and find the transform from the starting position's name.
        string startingPositionName = "DefaultStartingPosition";

        string aux="";
        if (saveLoad.Load(startingPositionKey, ref aux))
            startingPositionName = aux;
        Debug.Log("FOUND "+startingPositionName);
        Transform startingPosition = GameObject.Find(startingPositionName).transform;

        // Set the player's position and rotation based on the starting position.
        transform.position = startingPosition.position;
        transform.rotation = startingPosition.rotation;

        // Set the initial destination as the player's current position.
        destinationPosition = transform.position;
        agent.Warp(transform.position);
    }

    public void ForceDestination(Vector3 dest)
    {
        destinationPosition = dest;
        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
        comeFromForced = true;
        handleInput = false;
    }

    public void ForcePosition(Vector3 pos, Quaternion rot)
    {
        // Set the player's position and rotation based on the starting position.
        transform.position = pos;
        transform.rotation = rot;

        // Set the initial destination as the player's current position.
        destinationPosition = pos;
        agent.Warp(pos);
        animator.SetInteger("Status", 2);
    }

    public void ForcePosition(Transform pos)
    {
        // Set the player's position and rotation based on the starting position.
        transform.position = pos.position;
        transform.rotation = pos.rotation;

        // Set the initial destination as the player's current position.
        destinationPosition = pos.position;
        agent.Warp(pos.position);
        animator.SetInteger("Status", 2);
    }

    private void OnAnimatorMove()
    {
        // Set the velocity of the nav mesh agent (which is moving the player) based on the speed that the animator would move the player.
        agent.velocity = animator.deltaPosition / Time.deltaTime;
    }


    private void Update()
    {
        // If the nav mesh agent is currently waiting for a path, do nothing.
        if (agent.pathPending)
            return;

        // Cache the speed that nav mesh agent wants to move at.
        float speed = agent.desiredVelocity.magnitude;
        
        // If the nav mesh agent is very close to it's destination, call the Stopping function.
        if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)
            Stopping (out speed);
        // Otherwise, if the nav mesh agent is close to it's destination, call the Slowing function.
        else if (agent.remainingDistance <= agent.stoppingDistance)
            Slowing(out speed, agent.remainingDistance);
        // Otherwise, if the nav mesh agent wants to move fast enough, call the Moving function.
        else if (speed > turnSpeedThreshold)
            Moving ();
        
        // Set the animator's Speed parameter based on the (possibly modified) speed that the nav mesh agent wants to move at.
        animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);
    }


    // This is called when the nav mesh agent is very close to it's destination.
    private void Stopping (out float speed)
    {
        if (comeFromForced)
        {
            comeFromForced = false;
            handleInput = true;
        }
        // Stop the nav mesh agent from moving the player.
        agent.isStopped = true;

        // Set the player's position to the destination.
        transform.position = destinationPosition;

        // Set the speed (which is what the animator will use) to zero.
        speed = 0f;

        // If the player is stopping at an interactable...
        if (currentInteractable)
        {
            // ... set the player's rotation to match the interactionLocation's.
            if(currentInteractable.interactionLocation!=null)
                transform.rotation = currentInteractable.interactionLocation.rotation;

            // Interact with the interactable and then null it out so this interaction only happens once.
            currentInteractable.Interact();
            currentInteractable = null;

            // Start the WaitForInteraction coroutine so that input is ignored briefly.
            StartCoroutine (WaitForInteraction ());
        }
    }


    // This is called when the nav mesh agent is close to its destination but not so close it's position should snap to it's destination.
    private void Slowing (out float speed, float distanceToDestination)
    {
        // Although the player will continue to move, it will be controlled manually so stop the nav mesh agent.
        agent.isStopped = true;

        // Find the distance to the destination as a percentage of the stopping distance.
        float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;

        // The target rotation is the rotation of the interactionLocation if the player is headed to an interactable, or the player's own rotation if not.
        Quaternion targetRotation = currentInteractable ? currentInteractable.interactionLocation.rotation : transform.rotation;

        // Interpolate the player's rotation between itself and the target rotation based on how close to the destination the player is.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, proportionalDistance);

        // Move the player towards the destination by an amount based on the slowing speed.
        transform.position = Vector3.MoveTowards(transform.position, destinationPosition, slowingSpeed * Time.deltaTime);

        // Set the speed (for use by the animator) to a value between slowing speed and zero based on the proportional distance.
        speed = Mathf.Lerp(slowingSpeed, 0f, proportionalDistance);
    }


    // This is called when the player is moving normally.  In such cases the player is moved by the nav mesh agent, but rotated by this function.
    private void Moving ()
    {
        // Create a rotation looking down the nav mesh agent's desired velocity.
        Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);

        // Interpolate the player's rotation towards the target rotation.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
    }


    // This function is called by the EventTrigger on the scene's ground when it is clicked on.
    public void OnGroundClick(BaseEventData data)
    {
        // If the handle input flag is set to false then do nothing.
        if (!handleInput)
            return;

        
        if (cm.CamZoomed && cm.LockWhenZoom)
            return;

        tm.ClearMessage();
        
        // The player is no longer headed for an interactable so set it to null.
        currentInteractable = null;

        // This function needs information about a click so cast the BaseEventData to a PointerEventData.
        PointerEventData pData = (PointerEventData)data;

        //Debug.Log("click on: " + pData.position);

        // Try and find a point on the nav mesh nearest to the world position of the click and set the destination to that.
        NavMeshHit hit;
        if (NavMesh.SamplePosition (pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
            destinationPosition = hit.position;
        else
            // In the event that the nearest position cannot be found, set the position as the world position of the click.
            destinationPosition = pData.pointerCurrentRaycast.worldPosition;

        // Set the destination of the nav mesh agent to the found destination position and start the nav mesh agent going.
        agent.SetDestination(destinationPosition);
        agent.isStopped = false;

        if(camManager!=null)
            camManager.OnGroundClick(destinationPosition.z, pData.position.y);
    }

    // This function is called by the EventTrigger on an Interactable, the Interactable component is passed into it.
    public void OnInteractableClick(InteractionObject interactable)
    {
        // If the handle input flag is set to false then do nothing.
        if(!handleInput)
            return;

        
        tm.ClearMessage();

        // Store the interactble that was clicked on.
        currentInteractable = interactable;

        if (currentInteractable.interactionLocation != null)
        {
            // Set the destination to the interaction location of the interactable.
            destinationPosition = currentInteractable.interactionLocation.position;

            // Set the destination of the nav mesh agent to the found destination position and start the nav mesh agent going.
            agent.SetDestination(destinationPosition);
            agent.isStopped = false;
        }
    }


    private IEnumerator WaitForInteraction ()
    {
        // As soon as the wait starts, input should no longer be accepted.
        handleInput = false;

        // Wait for the normal pause on interaction.
        yield return inputHoldWait;

        // Until the animator is in a state with the Locomotion tag, wait.
        while (animator.GetCurrentAnimatorStateInfo (0).tagHash != hashLocomotionTag)
        {
            yield return null;
        }

        // Now input can be accepted again.
        handleInput = true;
    }
}
