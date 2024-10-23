using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    protected Rigidbody2D mRigidbody;

    protected Transform target;

    public GameObject deathVfx;
    public enum STATE
    {
        WAIT, MOVE, ATTACK
    };

    public STATE currentState;

    public float charingTime;

    private float timer;

    public AudioSource beeSound;

    private void Awake()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        currentState = STATE.WAIT;
    }
    void Start()
    {
        int dogIndexRandom = Random.Range(0, GameController.instance.currentLevel.dogList.Count);
        target = GameController.instance.currentLevel.dogList[dogIndexRandom];
        timer = 0.0f;
        if (AudioManager.instance.soundState == 0)
            beeSound.volume = 0.0f;
        else
            beeSound.volume = 1.0f;
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        switch (currentState)
        {
            case STATE.WAIT:
                break;

            case STATE.MOVE:
                mRigidbody.AddForce(Vector3.Normalize(target.position - (transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)))) * 5);
                break;

            case STATE.ATTACK:

                timer += Time.deltaTime;

                if (timer >= charingTime)
                {
                    mRigidbody.AddForce(Vector3.Normalize(target.position - (transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)))) * 10);
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Line")
        {
            StartToAttack();
        }
        if (collision.gameObject.tag == "Bee")
        {
            StartToAttack();
        }
        if (collision.gameObject.tag == "Dog")
        {
            StartToAttack();
            collision.gameObject.GetComponent<DogController>().Hurt();
        }
        if (collision.gameObject.tag == "Lava" || collision.gameObject.tag == "Water")
        {
            Instantiate(deathVfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void StartToAttack()
    {
        currentState = STATE.ATTACK;
        timer = 0.0f;
    }
}
