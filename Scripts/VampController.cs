using UnityEngine;
using System.Collections;

public class VampController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    //private int health;
    private bool _grounded;
    private bool _dead;
    private GameController _gameController;

    [SerializeField]
    private GameObject _batBurst = null;

    //this for initialization
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _gameController = GameController.GetGameControllerInScene();

    }

    void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jumping..? Grounded:" + _grounded);
            if (_grounded)
            {
                _rigidBody.AddForce(new Vector2(0, 1200));
                _animator.SetTrigger("Jump");
                _grounded = false;
                _animator.SetBool("Grounded", false);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger("Attack");

          
        }
    }


    void FixedUpdate()
    {
        if (_dead)
        {
            return;
        }

        //Read move left/right input
        var horizontal = Input.GetAxis("Horizontal");

        //If we're moving to the right, we've flipped this character by setting localScale=-1
        var localScale = transform.localScale;

        //Flips the character left if the input is < 0 and, right if >0 
        if (horizontal < 0)
        {
            // localScale is a Vector 3, which means it contains x,y,z
            localScale.x = 1;
        }
        else if (horizontal > 0f)
        {
            localScale.x = -1;
        }


        transform.localScale = localScale;

        //If we're moving left or right, play the run animation
        if (horizontal != 0)
        {
            _animator.SetBool("Run", true); 
        }
        else
        {
            _animator.SetBool("Run", false);
        }
        //var bip = Application.platform;
        //Move the actual object by setting its velocity
        _rigidBody.velocity = new Vector2(horizontal * 20, _rigidBody.velocity.y);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //all of platforms have a tag of "platform"
        if (collision.gameObject.tag == "platform")
        {
            _grounded = true;
            _animator.SetBool("Grounded", true);
        }
    }

    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "zombie")
        {
            //when we hit the zombie we die

            //Disable our physics
            _rigidBody.isKinematic = true;

            //Disable the vamp image
            GetComponent<SpriteRenderer>().enabled = false;

            //Enable bat particles here
            _batBurst.SetActive(true);

            //He's dead Jim.
            _dead = true;

           
            collision.gameObject.GetComponent<ZombieController>().KillZombie();


            _gameController.PlayerDied();



        }
    }
}
