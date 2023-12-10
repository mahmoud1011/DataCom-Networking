using Mirror;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Components")]
    [HorizontalLine]
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Animator animator;

    [Header("Camera Settings")]
    [HorizontalLine]
    public GameObject cameraHolder;
    public GameObject camPrefab;
    public Vector3 targetOffset;

    // Private Vars =============================================
    private Camera playerCamera;

    #region ==Server==
    [Command]
    private void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            return;
        }

        agent.SetDestination(hit.position);
    }
    #endregion

    #region ==Cient==
    //start method for the client who owns the object
    public override void OnStartAuthority()
    {
        if (!isLocalPlayer)
            return;

        GameObject newCamera = Instantiate(camPrefab, transform.position + targetOffset, Quaternion.identity);
        playerCamera = newCamera.GetComponent<Camera>();

        // Attach the camera to the player's cameraHolder GameObject
        newCamera.transform.SetParent(cameraHolder.transform);
        newCamera.transform.localPosition = Vector3.zero;

        cameraHolder.SetActive(true);

        // Modify the parent's rotation and position
        cameraHolder.transform.localRotation = Quaternion.Euler(90f, 0f, 0f); // X = 90 degrees
        cameraHolder.transform.localPosition = new Vector3(0f, 10f, -9f);
    }

    [ClientCallback] //makes it a client only update (all clients)
    private void Update()
    {
        //make sure object belongs to the client
        if (!isOwned || !NetworkClient.ready) //if (!hasAuthority) is the old function
        {
            return;
        }
        
        #region ==Movement WASD==
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (moveDirection.sqrMagnitude >= 0.1f * 0.1f)
        {
            // Convert input to world space direction
            Vector3 desiredMove = transform.TransformDirection(moveDirection);
            desiredMove *= agent.speed;

            // Move the player
            agent.Move(desiredMove * Time.deltaTime);

            // Update server with the new position
            CmdMove(transform.position + desiredMove);
            // Set the "isWalking" parameter in the Animator to true
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop the player's movement
            CmdMove(transform.position);
            animator.SetBool("isWalking", false);
        }
        #endregion
    }
    #endregion
}