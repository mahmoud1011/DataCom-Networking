using Mirror;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    PlayerMovement player;

    private void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!player.isServer)
        {
            Debug.Log("Not the server");
            return;
        }

        // Get the NetworkIdentity component of the collided object
        NetworkIdentity otherNetworkIdentity = other.GetComponent<NetworkIdentity>();

        // Check if the collided object has a NetworkIdentity
        if (otherNetworkIdentity != null && otherNetworkIdentity.CompareTag("Player"))
        {
            // Try to get the MyPlayerHealth component from the collided object
            if (other.TryGetComponent<MyPlayerHealth>(out var otherPlayerHealth))
            {
                // Reduce the health of the other player
                //otherPlayerHealth.TakeDamage(player.damageAmount);
            }
            else
            {
                // Log a message if the MyPlayerHealth component is not found
                Debug.LogWarning("Other player does not have MyPlayerHealth component.");
            }
        }
    }
}
