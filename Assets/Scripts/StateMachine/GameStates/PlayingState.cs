using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : State
{
    public PlayerMovement myPlayer;
    public List<Enemy> myEnemies = new List<Enemy>();
    public List<GunController> myGunControllers;

    private RayGunAttack myRayGunAttack;
    private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public override void enterState()
    {
        base.enterState();

        if (myPlayer == null)
        {
            Debug.LogError("PlayerMovement reference (myPlayer) is not assigned!");
            return;
        }
        myGunControllers = new List<GunController>(myPlayer.GetComponentsInChildren<GunController>());

        if (myRayGunAttack == null)
        {
            myRayGunAttack = myPlayer.GetComponent<RayGunAttack>();
            if (myRayGunAttack == null)
            {
                Debug.LogError("RayGunAttack component not found on player!");
            }
        }
        enemySpawners.AddRange(FindObjectsOfType<EnemySpawner>());
        if (enemySpawners.Count == 0)
        {
            Debug.LogError("No EnemySpawner found in the scene!");
        }
        myEnemies.AddRange(GameObject.FindObjectsOfType<Enemy>());
        if (myEnemies.Count == 0)
        {
            Debug.LogError("No enemies found in the scene!");
        }
    }

    public override void updateState()
    {
        base.updateState();
        foreach (var gunController in myGunControllers)
        {
            gunController.UpdateGunController();
        }

        if (myPlayer != null)
        {
            myPlayer.UpdatePlayer(); 
        }
        if (myRayGunAttack != null)
        {
            myRayGunAttack.RayGunAttackUpdate();
        }
        for (int i = myEnemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = myEnemies[i];
            if (enemy != null && enemy.isActiveAndEnabled)
            {
                enemy.UpdateEnemy(); 
            }
            else if (enemy == null)
            {
                myEnemies.RemoveAt(i);
            }
        }
        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.UpdateEnemySpawner();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamesManager.instance.switchState<PauseState>();
        }

    }
    public void AddEnemy(Enemy newEnemy)
    {
        if (newEnemy != null && !myEnemies.Contains(newEnemy))
        {
            myEnemies.Add(newEnemy);
        }
    }
    public override void exitState()
    {
        base.exitState();
        myPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop movement when exiting

        Animator animator = myPlayer.GetComponent<Animator>();
        if(animator != null)
        {
            animator.SetBool("Move", false);
        }
    }
}

