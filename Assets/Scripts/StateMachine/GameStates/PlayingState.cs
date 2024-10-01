using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : State
{
    public PlayerMovement myPlayer;
    public List<Enemy> myEnemies = new List<Enemy>();
    private GunController myGunController;

    public override void enterState()
    {
        base.enterState();
        Time.timeScale = 1f; // Will be removed after refactor
        if (myPlayer == null)
        {
            Debug.LogError("PlayerMovement reference (myPlayer) is not assigned!");
            return;
        }
        if (myGunController == null)
        {
            myGunController = myPlayer.GetComponentInChildren<GunController>();
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
            myGunController.UpdateGunController();
        if (myPlayer != null)
        {
            myPlayer.UpdatePlayer(); 
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

