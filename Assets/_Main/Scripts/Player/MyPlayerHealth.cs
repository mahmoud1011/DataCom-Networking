using Mirror;
using MyBox;
using NaughtyAttributes;
using UnityEngine;

public class MyPlayerHealth : MonoBehaviour
{
    //[SyncVar(hook = nameof(OnHealthChanged))]
    //public int currentHealth = 100;

    [InfoBox(@"MyNetworkPlayer is required to make the health system work!")]
    [SerializeField] private MyNetworkPlayer networkPlayer;

    public void TakeDamage(int damageAmount)
    {
        if (!networkPlayer.isServer)
            return;

        networkPlayer.currentHealth -= damageAmount;
        if (networkPlayer.currentHealth <= 0)
        {
            // Perform actions upon player death, like disabling GameObject, respawning, etc.
            RpcHandlePlayerDeath(); // Notify all clients about the player's death
        }
    }

    private void RpcHandlePlayerDeath()
    {
        // Perform actions upon player death (this method is called on all clients)
        // Example: Disable the GameObject, trigger a death animation, etc.

        Destroy(gameObject);
        //gameObject.SetActive(false); // Disable the player GameObject
    }
}
