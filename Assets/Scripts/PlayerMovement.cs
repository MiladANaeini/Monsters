using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : Health
{
    public float moveSpeed = 5f;
    private float originalMoveSpeed;
    Animator am;
    SpriteRenderer sr;
    Rigidbody2D rb;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 moveDir;
    private UpdateSpriteOnHit updateSpriteOnHit;
    public GameObject playerBlood;
    public Slider healthSlider;
    public Slider amberXPSlider;// Speed Boost
    public Slider blueXPSlider;// Shovel
    public Menu menu;

    public GameObject gameOverMenu;

    public GameObject speedBoostUI; // parent GameObject of speedtext and speedtimer
    private TMP_Text speedText;
    private TMP_Text speedTimer;

    [HideInInspector]
    public int blueXP = 0;
    [HideInInspector]
    public int amberXP = 0;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        am = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        healthSlider.maxValue = maxHealth; 
        healthSlider.value = health;

        amberXPSlider.maxValue = 10;
        amberXPSlider.value = amberXP;
        blueXPSlider.maxValue = 1;
        blueXPSlider.value = blueXP;

        updateSpriteOnHit = GetComponent<UpdateSpriteOnHit>();
        onHealthChanged += UpdateSprite;
  

    }
    public void UpdateSprite()
    {
        healthSlider.value = health;
        updateSpriteOnHit.ChangeSprite();
        GameObject blood = Instantiate(playerBlood, transform.position, Quaternion.identity);
        Destroy(blood, 2f);

        if (health <= 0)
        {
            //gameOverMenu.SetActive(true);
            menu.switchState(Menu.MenuState.gameOverMenu);
            menu.PauseGame();
            Destroy(gameObject); // destroy the player character
        }
    }
    public void UpdateAnimation()
    {
        if (moveDir.x != 0 || moveDir.y != 0)
        {
            am.SetBool("Move", true);
            SpriteDirectionChecker();
        }
        else
        {
            am.SetBool("Move", false);
        }
    }
    void SpriteDirectionChecker()
    {
        sr.flipX = lastHorizontalVector < 0;
    }
    public void UpdatePlayer()
    {
        InputManagement();
        Move();
        UpdateAnimation();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY);
        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
        }
        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
        }

    }

    void Move()
    {
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("XP"))
        {
       if (other.gameObject.CompareTag("BlueXP"))
        {

            StartCoroutine(MoveTowardsPlayer(other.gameObject, "BlueXP"));
        }
        else if (other.gameObject.CompareTag("AmberXP"))
        {
            StartCoroutine(MoveTowardsPlayer(other.gameObject, "AmberXP"));
        }
        }
    }
    IEnumerator MoveTowardsPlayer(GameObject xpObject, string xpType)
    {
        var collider = xpObject.GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
        while (xpObject != null)
        {
            xpObject.transform.position = Vector2.MoveTowards(xpObject.transform.position, transform.position, 5f * Time.deltaTime);

            if (Vector2.Distance(xpObject.transform.position, transform.position) < 0.1f)
            {
                if (xpType == "BlueXP")
                {
                    UpdateXP(blueXPSlider, ref blueXP);

                }
                else if (xpType == "AmberXP")
                {
                    UpdateXP(amberXPSlider, ref amberXP);
                    if (amberXP >= 10)
                    {
                        speedBoostUI.SetActive(true);
                        speedText = speedBoostUI.transform.Find("SpeedText").GetComponent<TMP_Text>();
                        speedTimer = speedBoostUI.transform.Find("SpeedTimer").GetComponent<TMP_Text>();
                        originalMoveSpeed = moveSpeed;
                        moveSpeed *= 2;
                        amberXP = 0;
                        speedText.text = "Speed 2X!";
                        Debug.Log(speedText.text);
                        StartCoroutine(ResetMoveSpeedAfterDelay(5f));
                    }
                }
                Destroy(xpObject); 
                yield break; // Stop the coroutine
            }
            yield return null;
        }
    }
    IEnumerator ResetMoveSpeedAfterDelay(float delay)
    {
        float timeRemaining = delay;

        while (timeRemaining > 0)
        {
            speedTimer.text = $"{timeRemaining:F1}s";

            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        speedBoostUI.SetActive(false);
        moveSpeed = originalMoveSpeed;
        speedText.text = "";
        speedTimer.text = "";  
    }
    private void UpdateXP(Slider XPSlider, ref int XPAmount)
    {
        XPAmount++;
        XPSlider.value = XPAmount; 
    }


}
