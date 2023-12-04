using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ProjectileLauncher : NetworkBehaviour
{

[Header("References:")]

[SerializeField] private Collider2D playerCollider;
[SerializeField] private GameObject muzzleFlash;
[SerializeField] private InputReader inputReader;
[SerializeField] private Transform projectileSpawnPoint;
[SerializeField] private GameObject serverProjectilePrefab;
[SerializeField] private GameObject clientProjectilePrefab;

[Header("Settings:")]

[SerializeField] private float projectileSpeed;
[SerializeField] private float fireRate;

[SerializeField] private float muzzleFlashDuration;


private bool shouldFire;
private float previousFireTime;
private float muzzleFlashTimer;


public override void OnNetworkSpawn()
{
    if(!IsOwner){return;}

    inputReader.PrimaryFireEvent += HandlePrimaryFire;
}

public override void OnNetworkDespawn()
{
    if(!IsOwner){return;}

    inputReader.PrimaryFireEvent -= HandlePrimaryFire;
}


private void  HandlePrimaryFire(bool shouldFire)
{
    this.shouldFire = shouldFire;

}


private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
{
    Debug.Log("Test0");
    muzzleFlash.SetActive(true);
    muzzleFlashTimer = muzzleFlashDuration;
    
    GameObject projectileInstance = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
    projectileInstance.transform.up = direction;
    
    Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

    if(projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
    {
        rb.velocity = rb.transform.up * projectileSpeed;
       Debug.Log("Test1");
    }
}

[ServerRpc]
private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
{
    Debug.Log("Test2");
    GameObject projectileInstance = Instantiate(
        serverProjectilePrefab,
        spawnPos, 
        Quaternion.identity);
    projectileInstance.transform.up = direction;

     Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

     if(projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
    {
        rb.velocity = rb.transform.up * projectileSpeed;
        Debug.Log("Test3");
    }

    Debug.Log("Test4");
    SpawnDummyProjectileClientRpc(spawnPos, direction);
    Debug.Log("Test6");
    
}

[ClientRpc]
private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
{
    if(IsOwner){return;}
    Debug.Log("Test5");
    SpawnDummyProjectile(spawnPos, direction);
    

}
 
   private void Update()
    {
        if(muzzleFlashTimer > 0f )
        {
            muzzleFlashTimer = muzzleFlashTimer - Time.deltaTime;

            if (muzzleFlashTimer <= 0f){muzzleFlash.SetActive(false);}
        }

        if(!IsOwner){return;}
        if(!shouldFire){return;}

        
        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);

    }
}
