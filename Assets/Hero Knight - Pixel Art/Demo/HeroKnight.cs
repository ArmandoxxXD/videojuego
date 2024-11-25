using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private int dañoGolpe;
    [SerializeField] private float fuerzaEmpuje = 10.0f;
    [SerializeField] private float tiempoParpadeo = 1.5f;

    private Vector2 colliderOriginalSize;
    private Vector2 colliderOriginalOffset; 

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private SpriteRenderer m_spriteRenderer;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1, m_wallSensorR2, m_wallSensorL1, m_wallSensorL2;
    private VidaJugador vidaJugador;
    private BoxCollider2D playerCollider;
    private bool m_isWallSliding = false, m_grounded = false, m_rolling = false, isBlocking = false, isDead = false, facingRight = true, invulnerable = false, canWallJump = true;
    public bool IsWallSliding => m_isWallSliding;
    private float m_timeSinceAttack = 0.0f, m_delayToIdle = 0.0f, m_rollCurrentTime = 0.0f;
    private int m_currentAttack = 0;
    private readonly float m_rollDuration = 8.0f / 14.0f;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        vidaJugador = GetComponent<VidaJugador>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        m_body2d.interpolation = RigidbodyInterpolation2D.Interpolate;

        colliderOriginalSize = playerCollider.size;
        colliderOriginalOffset = playerCollider.offset;
    }

    void Update()
    {
        if (!isDead)
        {
            m_timeSinceAttack += Time.deltaTime;
            if (m_rolling) m_rollCurrentTime += Time.deltaTime;
            if (m_rollCurrentTime > m_rollDuration) m_rolling = false;
           
            HandleGroundAndWallSensors();
            HandleInput();
         }
    }

    private void HandleGroundAndWallSensors()
    {
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            canWallJump = true;
        }
        else if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        bool touchingWallRight = m_wallSensorR1.State() && m_wallSensorR2.State();
        bool touchingWallLeft = m_wallSensorL1.State() && m_wallSensorL2.State();
        m_isWallSliding = !m_grounded && (touchingWallRight || touchingWallLeft);

        if (m_isWallSliding)
        {
            m_animator.SetBool("WallSlide", true);
            if ((facingRight && touchingWallRight) || (!facingRight && touchingWallLeft))
                m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
        }
        else
            m_animator.SetBool("WallSlide", false);
    }

    private void HandleInput()
    {
        float inputX = Input.GetAxis("Horizontal");

        if ((inputX > 0 && !facingRight) || (inputX < 0 && facingRight))
            Flip();

        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        if (m_isWallSliding && Input.GetKeyDown("space") && canWallJump)
        {
            m_body2d.velocity = new Vector2(-m_facingDirection() * m_jumpForce, m_jumpForce);
            m_isWallSliding = false;
            m_animator.SetTrigger("Jump");
            canWallJump = false;
            StartCoroutine(JumpCooldown());
        }
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        HandleActions(inputX);
    }

    private void HandleActions(float inputX)
    {
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
            Attack();
        else if (Input.GetMouseButtonDown(1) && !isBlocking)
            Block();
        else if (Input.GetMouseButtonUp(1) && isBlocking)
            isBlocking = false;
        else if (Input.GetKeyDown("left shift") && !m_isWallSliding)
            Roll();
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 1); // Animación de caminar
        else
            m_animator.SetInteger("AnimState", 0); // Forzar a Idle cuando está quieto
    }

    private void Attack()
    {
        m_currentAttack++;
        if (m_currentAttack > 3) m_currentAttack = 1;
        if (m_timeSinceAttack > 1.0f) m_currentAttack = 1;
        m_animator.SetTrigger("Attack" + m_currentAttack);
        m_timeSinceAttack = 0.0f;
        Golpe();
    }

    private void Block()
    {
        m_animator.SetTrigger("Block");
        m_animator.SetBool("IdleBlock", true);
        isBlocking = true;
    }

    private void Roll()
    {
        if (!m_rolling) // Solo realizar el dash si no está en curso
        {
            m_rolling = true;

            // Reducir el tamaño del collider dinámicamente (dividir por 2)
            playerCollider.size = new Vector2(colliderOriginalSize.x, colliderOriginalSize.y / 2.0f);
            playerCollider.offset = new Vector2(colliderOriginalOffset.x, colliderOriginalOffset.y / 2.0f);

            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection() * m_rollForce, m_body2d.velocity.y);

            // Restaurar el tamaño después de un tiempo
            StartCoroutine(RestoreColliderAfterDash());
        }
    }

    private IEnumerator RestoreColliderAfterDash()
    {
        yield return new WaitForSeconds(m_rollDuration); // Esperar la duración del dash

        // Restaurar el tamaño original del collider
        playerCollider.size = colliderOriginalSize;
        playerCollider.offset = colliderOriginalOffset;

        m_rolling = false; // Finalizar el dash
    }

    private void TriggerAnimation(string trigger, string boolName = null, bool boolValue = false)
    {
        m_animator.SetTrigger(trigger);
        if (boolName != null) m_animator.SetBool(boolName, boolValue);
    }

    private void Golpe()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colisionador in objetos)
            if (colisionador.CompareTag("Enemigo"))
            {
                Enemigo enemigo = colisionador.GetComponent<Enemigo>();
                EnemigoCainos enemigoCainos = colisionador.GetComponent<EnemigoCainos>();

                if (enemigo != null)
                {
                    enemigo.TomarDaño(dañoGolpe);
                }
                else if (enemigoCainos != null)
                {
                    enemigoCainos.TomarDaño(dañoGolpe);
                }
            }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        float scaleX = facingRight ? 1 : -1;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
    }

    private int m_facingDirection() => facingRight ? 1 : -1;

    public void TomarGolpe (int daño, Vector2 direccion)
    {
        if (isBlocking)
        {
            Vector2 direccionEmpuje = new Vector2(transform.position.x - direccion.x , 0.2f).normalized;
            m_body2d.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);
            m_animator.SetTrigger("Block");
            StartCoroutine(DesactivarBloqueo());
        }
        else if (!invulnerable)
        {
            invulnerable = true;
            GetComponent<VidaJugador>().TomarDaño(daño);
            if (vidaJugador.vidaActual <= 0)
            {
                isDead = true; 
            }
            m_animator.SetTrigger("Hurt");
            StartCoroutine(ParpadearYHacerInvulnerable());
        }
    }

    private IEnumerator JumpCooldown()
    {
        canWallJump = false;
        yield return new WaitForSeconds(0.75f);
        canWallJump = true;
    }

    private IEnumerator ParpadearYHacerInvulnerable()
    {

        for (float i = 0; i < tiempoParpadeo && vidaJugador.vidaActual > 0; i += 0.2f)
        {
            m_spriteRenderer.enabled = !m_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        m_spriteRenderer.enabled = true;
        invulnerable = false;
    }

    private IEnumerator DesactivarBloqueo()
    {
        yield return new WaitForSeconds(0.5f); // Esperar antes de volver a bloquear
        isBlocking = false;
        m_animator.SetBool("IdleBlock", false);
        m_animator.SetInteger("AnimState", 0); // Forzar a Idle
    }

    void AE_SlideDust()
    {
        Vector3 spawnPosition = facingRight ? m_wallSensorR2.transform.position : m_wallSensorL2.transform.position;
        if (m_slideDust != null)
        {
            GameObject dust = Instantiate(m_slideDust, spawnPosition, Quaternion.identity);
            dust.transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
