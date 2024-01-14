using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_RigidBody;
    public float m_MovementSpeed;

    public Animator m_Animator;
    public ArenaBuilder m_ArenaBuilderScript;
    public float playerType;
    public AudioSource m_deathSound;
    public AudioSource m_PowerUpSound;
    public TextMeshProUGUI m_HUD;

    private GameObject m_ForceField;
    private GameObject m_LastBombPlaced;
    private float rotationSpeed;
    private PlayerInventory inventory;
    private int lives;
    private bool dead;
    private float deadTime;
    private float invincibleTime;
    private Vector3 startPosition;
    private Quaternion lastRotation;


    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_ForceField = gameObject.transform.GetChild(0).gameObject;
        rotationSpeed = 10f;
        dead = false;
        deadTime = 0f;
        invincibleTime = 0f;
        inventory = new PlayerInventory();

        lastRotation = Quaternion.identity;

        lives = 3;

        if (playerType < 1)
        {
            switch (GlobalVariables.ArenaMapIndex)
            {
                case 4:
                    gameObject.transform.position = new Vector3(GlobalVariables.CustomArenaRespawnPositions[0].x, 1, GlobalVariables.CustomArenaRespawnPositions[0].y);
                    break;
                case 0:
                    gameObject.transform.position = new Vector3(1, 1, 1);
                    break;
                case 1:
                    gameObject.transform.position = new Vector3(4, 1, 1);
                    break;
                case 2:
                default:
                    gameObject.transform.position = new Vector3(2, 1, 2);
                    break;
            }
        }
        else
        {
            inventory.BombColor = Color.blue;

            switch (GlobalVariables.ArenaMapIndex)
            {
                case 4:
                    int last = GlobalVariables.CustomArenaRespawnPositions.Count - 1;
                    gameObject.transform.position = new Vector3(GlobalVariables.CustomArenaRespawnPositions[last].x, 1, GlobalVariables.CustomArenaRespawnPositions[last].y);
                    break;
                case 0:
                    gameObject.transform.position = new Vector3(15, 1, 11);
                    break;
                case 1:
                    gameObject.transform.position = new Vector3(12, 1, 15);
                    break;
                case 2:
                default:
                    gameObject.transform.position = new Vector3(14, 1, 14);
                    break;
            }
        }

        startPosition = gameObject.transform.position;

    }

    void Update()
    {

        m_ForceField.SetActive(Time.time <= invincibleTime);

        m_HUD.text = "Lives: " + lives + ", Bombs: " + inventory.Bombs;
        if(dead && Time.time < deadTime)
        {
            gameObject.transform.position = startPosition - new Vector3(0, 2, 0);
            return;
        } else if (dead)
        {
            dead = false;
            gameObject.transform.position = startPosition;
        }

        Vector3 forward = new Vector3(0, 0, 1);
        Vector3 right = new Vector3(-1.0f, 0.0f, 0.0f);

        Vector3 velocity = new Vector3();
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (playerType < 1)
        {
            if (Input.GetKey(KeyCode.W))
            {
                velocity += forward;
                moveDirection += -forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                velocity += -forward;
                moveDirection += forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                velocity += right;
                moveDirection += -right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                velocity += -right;
                moveDirection += right;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                velocity += forward;
                moveDirection += -forward;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                velocity += -forward;
                moveDirection += forward;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                velocity += right;
                moveDirection += -right;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                velocity += -right;
                moveDirection += right;
            }
        }

        if (moveDirection != Vector3.zero)
        {
            moveDirection.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            lastRotation = Quaternion.Slerp(lastRotation, targetRotation, Time.deltaTime * rotationSpeed);
            m_RigidBody.rotation = lastRotation;
            m_Animator.SetBool("IsMoving", true);

        } else
        {
            m_RigidBody.rotation = lastRotation;
            m_Animator.SetBool("IsMoving", false);
        }


        velocity = velocity.normalized * Mathf.Min(5.5f, m_MovementSpeed + inventory.SpeedPowerup * 0.15f);

        m_RigidBody.velocity = velocity;

        Vector3 pos = gameObject.transform.position;
        if (m_LastBombPlaced)
        {
            Vector3 bombPos = m_LastBombPlaced.transform.position;
            if (Mathf.Pow(pos.x - bombPos.x, 2) + Mathf.Pow(pos.z - bombPos.z, 2) > 0.6)
            {
                Physics.IgnoreCollision(m_LastBombPlaced.GetComponent<Collider>(), GetComponent<Collider>(), false);
                m_LastBombPlaced = null;
            }
        }

        if ((playerType < 1 && Input.GetKey(KeyCode.E)) || (playerType >= 1 && Input.GetKeyDown(KeyCode.Return)))
        {
            int x = (int)(gameObject.transform.position.x + 0.5f);
            int z = (int)(gameObject.transform.position.z + 0.5f);

            GameObject b = m_ArenaBuilderScript.TryPlaceBomb(x, z, inventory.Bombs, inventory.BombColor, inventory.DetonationTime, inventory.RadiusPowerup, inventory);
            Debug.Log(inventory.Bombs);

            if (b != null)
            {
                Debug.Log("Bomb dropped " + x + " " + z);
                inventory.Bombs--;
                // Enable collsion for last bomb
                if (m_LastBombPlaced)
                    Physics.IgnoreCollision(m_LastBombPlaced.GetComponent<Collider>(), GetComponent<Collider>(), false);

                m_LastBombPlaced = b;
                Physics.IgnoreCollision(m_LastBombPlaced.GetComponent<Collider>(), GetComponent<Collider>(), true);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        addPowerup(other.tag);

        if (other.tag == "Explosion")
        {
            if(Time.time > invincibleTime)
            {
                kill();
            }
        }
    }


    int getBombRadius()
    {
        return 1 + (this.inventory.RadiusPowerup);
    }
    int getBombs()
    {
        return this.inventory.Bombs;
    }
    void addSpeedPowerup()
    {
        this.inventory.SpeedPowerup += 1;
    }
    void addRadiusPowerup()
    {
        this.inventory.RadiusPowerup += 1;
    }
    void addBombPowerup()
    {
        this.inventory.Bombs += 1;
    }
    void resetInventory()
    {
        // Lose half of powerups on death
        this.inventory.Bombs = 1;
        this.inventory.SpeedPowerup = this.inventory.SpeedPowerup / 2;
        this.inventory.RadiusPowerup = this.inventory.RadiusPowerup / 2;
    }
    float getDetonationTime()
    {
        return this.inventory.DetonationTime;
    }
    void kill()
    {

        this.lives--;
        if (this.lives < 1)
        {
            int playerIndexWin = (playerType < 1) ? 2 : 1;
            m_ArenaBuilderScript.gameOver("GAME OVER PLAYER" + playerIndexWin + " WINS");
        }
        this.dead = true;
        this.deadTime = Time.time + 3f;
        this.invincibleTime = Time.time + 6f;
        m_deathSound.Play();
        this.resetInventory();

        if (GlobalVariables.ArenaMapIndex == 4)
        {
            int randomPos = Random.Range(0, GlobalVariables.CustomArenaRespawnPositions.Count);
            startPosition = new Vector3(GlobalVariables.CustomArenaRespawnPositions[randomPos].x, 1, GlobalVariables.CustomArenaRespawnPositions[randomPos].y);
        }
    }

    void resetPosition()
    {
        gameObject.transform.position = startPosition;
        gameObject.transform.rotation = Quaternion.identity;

        int randomPos = Random.Range(0, GlobalVariables.CustomArenaRespawnPositions.Count);
        Debug.Log(randomPos);
        if (playerType < 1)
        {
            switch (GlobalVariables.ArenaMapIndex)
            {
                case 4:
                    gameObject.transform.position = new Vector3(GlobalVariables.CustomArenaRespawnPositions[randomPos].x, 1, GlobalVariables.CustomArenaRespawnPositions[randomPos].y);
                    break;
                case 0:
                    gameObject.transform.position = new Vector3(1, 1, 1);
                    break;
                case 1:
                    gameObject.transform.position = new Vector3(4, 1, 1);
                    break;
                case 2:
                default:
                    gameObject.transform.position = new Vector3(2, 1, 2);
                    break;
            }
        }
        else
        {
            inventory.BombColor = Color.blue;

            switch (GlobalVariables.ArenaMapIndex)
            {
                case 4:
                    gameObject.transform.position = new Vector3(GlobalVariables.CustomArenaRespawnPositions[randomPos].x, 1, GlobalVariables.CustomArenaRespawnPositions[randomPos].y);
                    break;
                case 0:
                    gameObject.transform.position = new Vector3(15, 1, 11);
                    break;
                case 1:
                    gameObject.transform.position = new Vector3(12, 1, 15);
                    break;
                case 2:
                default:
                    gameObject.transform.position = new Vector3(14, 1, 14);
                    break;
            }
        }


    }
    void reset()
    {
        this.inventory = new PlayerInventory();
        if(playerType >= 1.0f)
        {
            this.inventory.BombColor = Color.blue;
        }
        this.resetPosition();
        this.lives = 3;
    }
    void addPowerup(string type)
    {
        if (type == "SpeedPowerUp")
        {
            this.addSpeedPowerup();
        }
        else if (type == "RadiusPowerUp")
        {
            this.addRadiusPowerup();
        }
        else if (type == "BombPowerUp")
        {
            this.addBombPowerup();
        } else
        {
            return;
        }
        m_PowerUpSound.Play();

        Debug.Log("PowerUp Hit");

    }
}

public class PlayerInventory
{
    public int Bombs { get; set; }
    public int SpeedPowerup { get; set; }
    public int RadiusPowerup { get; set; }
    public float DetonationTime { get; set; }

    public Vector2Int LastPlaced { get; set; }
    public Color BombColor { get; set; }

    public PlayerInventory(int bombs = 1, int speedPowerup = 0, int radiusPowerup = 0, float detonationTime = 2)
    {
        Bombs = bombs;
        SpeedPowerup = speedPowerup;
        RadiusPowerup = radiusPowerup;
        DetonationTime = detonationTime;

        LastPlaced = new Vector2Int(-1, -1);
        BombColor = Color.red;
    }
}
