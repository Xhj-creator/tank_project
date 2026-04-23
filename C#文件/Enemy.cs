using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour,IDamageble
{
    public LayerMask obstacleMask;
    public Transform player;
    public GameObject wheel;
    public GameObject bullet;
    public GameObject shooter;
    public Slider HPbar;
    public GameObject pos0;
    int maxHealth = 100;
    int currentHealth;
    float timecount = 0;
    float timecnt = 0;
    Vector3 dir;
    Vector3 shoot_dir;
    private Rigidbody rigid;
    bool isChasing = false;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        obstacleMask = LayerMask.GetMask("wall_lay", "redwall");
        currentHealth = maxHealth;
        HPbar.maxValue = maxHealth;
        HPbar.value = currentHealth;
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");
        player = playerobj.transform;
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        rigid.velocity = dir.normalized* 2;
        wheel.transform.rotation = Quaternion.LookRotation(Vector3.up, dir.normalized) * Quaternion.Euler(0, 0, 180);
    }

    // Update is called once per frame
    void Update()
    {
        timecount += Time.deltaTime;
        if (CanSeePlayer())
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            if (timecount >= 1)
            {
                dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                shoot_dir = dir;
                rigid.velocity = dir.normalized * 2;
                Quaternion rotation = Quaternion.LookRotation(dir.normalized);
                wheel.transform.rotation = Quaternion.LookRotation(Vector3.up, dir.normalized) * Quaternion.Euler(0, 0, 180);
                GameObject obj1 = Instantiate(bullet, pos0.transform.position, rotation);
                shooter.transform.rotation = Quaternion.LookRotation(shoot_dir);
                timecount = 0;
            }
        }
        HPbar.transform.position = Camera.main.WorldToScreenPoint(pos0.transform.position+Vector3.up);
        timecnt += Time.deltaTime;
        if (timecnt >= 3)
        {
            HPbar.gameObject.SetActive(false);
        }
        else
        {
            HPbar.gameObject.SetActive(true);
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (Physics.Raycast(transform.position, direction.normalized, distance, obstacleMask))
        {
            return false; 
        }

        return true; 
    }

    void ChasePlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - shooter.transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero) return;

        rigid.velocity = direction;
        Quaternion rotation = Quaternion.LookRotation(direction.normalized);
        wheel.transform.rotation = Quaternion.LookRotation(Vector3.up, direction.normalized) * Quaternion.Euler(0, 0, 180);
        if (timecount >= 1)
        {
            shoot_dir = direction;
            GameObject obj1 = Instantiate(bullet, pos0.transform.position, rotation);
            shooter.transform.rotation = Quaternion.LookRotation(shoot_dir);
            timecount = 0;
        }
    }
    public void damage(int damageamount)
    {
        currentHealth -= damageamount;
        timecnt = 0;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Destroy(HPbar.gameObject);
        }
        else
        {
            HPbar.value = currentHealth;
        }
    }
}
