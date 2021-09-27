using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private CapsuleCollider col;
    private Animator anim;
    private Score score;
    private Vector3 dir;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForse;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private int coins;
    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private GameObject scoreText;
    [SerializeField] 
    private Text coinsText;
    [SerializeField]
    private Score scoreScript;

    private bool isSliding;
    private bool isImmortal;


    private int lineToMove = 1;
    public float lineDistance = 4;
    private float maxSpeed = 110;

    public GameObject bonusEffect;

    [SerializeField] private AudioSource somersaultSound;
    [SerializeField] private AudioSource sighSound;
    [SerializeField] private AudioSource JumpSound;
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private AudioSource starSound;
    [SerializeField] private AudioSource shieldSound;
    [SerializeField] private AudioSource hitSound;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        col = GetComponent<CapsuleCollider>();
        score = scoreText.GetComponent<Score>();
        score.scoreMultilplier = 1;
        Time.timeScale = 1;
        coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();
        StartCoroutine(SpeedIncrease());
        isImmortal = false;
    }

    private void Update()
    {
        if (SwipeController.swipeRight)
        {
            if (lineToMove < 2)
                lineToMove++;
        }
        if (SwipeController.swipeLeft)
        {
            if (lineToMove > 0)
                lineToMove--;
        }
        if (SwipeController.swipeUp)
        {
            if (controller.isGrounded)
            Jump();
        }
        if (SwipeController.swipeDown)
        {
            StartCoroutine(Slide());
        }

        if (controller.isGrounded && !isSliding)
            anim.SetBool("isRunning", true);         
        else
            anim.SetBool("isRunning", false);
            

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

        
    }

    private void Jump()
    {
        dir.y = jumpForse;
        anim.SetTrigger("isJumping");
        sighSound.Play();
        JumpSound.Play();
    }
    
    void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle") 
        {
            hitSound.Play();
            if (isImmortal)
                Destroy(hit.gameObject);
            else
            {
                losePanel.SetActive(true);
                int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
                PlayerPrefs.SetInt("lastRunScore", lastRunScore);
                Time.timeScale = 0;

            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            coins++;
            PlayerPrefs.SetInt("coins", coins);
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);
            Instantiate(bonusEffect, transform.position, Quaternion.identity);
            coinSound.Play();
        }

        if (other.gameObject.tag == "BonusMultipliers")
        {
            StartCoroutine(BonusMultipliers());
            Destroy(other.gameObject);
            Instantiate(bonusEffect, transform.position, Quaternion.identity);
            starSound.Play();
        }

        if (other.gameObject.tag == "BonusShield")
        {
            StartCoroutine(BonusShield());
            Destroy(other.gameObject);
            Instantiate(bonusEffect, transform.position, Quaternion.identity);
            shieldSound.Play();
        }
    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSecondsRealtime(1);
        if (speed < maxSpeed)
        {
            speed += 1;
            StartCoroutine(SpeedIncrease());
        }
        
    }

    private IEnumerator Slide()
    {
        col.center = new Vector3(0, 1.5f, 0);
        col.height = 5;
        isSliding = true;
        anim.SetBool("isRunning", false);
        anim.SetTrigger("isSliding");

        yield return new WaitForSeconds(1);     

        col.center = new Vector3(0, 2.853746f, 0);
        col.height = 2.853746f;
        isSliding = false;

        somersaultSound.Play();

    }
    private IEnumerator BonusMultipliers()
    {
        score.scoreMultilplier = 2;

        yield return new WaitForSeconds(5);

        score.scoreMultilplier = 1;

        

    }

    private IEnumerator BonusShield()
    {
        isImmortal = true;

        yield return new WaitForSeconds(5);

        isImmortal = false;
    }
}
