using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class move : MonoBehaviour
{
    // Start is called before the first frame update
    //laser
    public int maxBounces = 0;
    private LineRenderer lineRenderer;
    float maxRange = 40f;
    public LayerMask enemy_lay;      
    public LayerMask wall_lay;       
    public int laserDamage = 40;
    public float laserDuration = 0.1f;
    //
    int my_speed = 3;
    public bool speedup_mode=false;
    float speed_cnt = 0;
    public bool noharm_mode = false;
    float noharm_cnt = 0f;
    public int mode = 1;
    float timecnt = 0f;
    int currentHealth=100;
    public GameObject bullet;
    public GameObject wave;
    //public GameObject pos0;
    public Slider HPbar;
    public GameObject shooter1;
    public GameObject shooter2;
    public GameObject shooter3;
    private Rigidbody rigid;
    public GameObject wheel;
    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject shield;
    private GameObject Shield;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        enemy_lay = LayerMask.GetMask("enemy_lay");
        wall_lay = LayerMask.GetMask("wall_lay");
    }

    // Update is called once per frame
    void Update()
    {

        //移动
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(-vertical, 0,horizontal);
        Vector3 centerposition = transform.position;
        if (dir != Vector3.zero)
        {
            //transform.rotation = Quaternion.LookRotation(dir);
            rigid.velocity=dir * my_speed;
            wheel.transform.rotation = Quaternion.LookRotation(Vector3.up, dir.normalized)*Quaternion.Euler(0,0,180);
            wheel1.transform.rotation = Quaternion.LookRotation(Vector3.up, dir.normalized) * Quaternion.Euler(0, 0, 180);
            wheel2.transform.rotation = Quaternion.LookRotation(Vector3.up, dir.normalized) * Quaternion.Euler(0, 0, 180);
        }

        //血条隐藏
        timecnt += Time.deltaTime;
        if (timecnt >= 3)
        {
            HPbar.gameObject.SetActive(false);
        }
        else
        {
            HPbar.gameObject.SetActive(true);
        }

        //免疫
        if (noharm_mode == true)
        {
            if ((noharm_cnt==0)&&Shield==null)
            {
                Shield = Instantiate(shield, centerposition,Quaternion.identity);
            }
            noharm_cnt += Time.deltaTime;
            Shield.transform.position = centerposition;
        }
        if (noharm_cnt >= 8)
        {
            noharm_mode = false;
            noharm_cnt = 0;
            my_speed = 3;
            Destroy(Shield);
            Shield = null;
        }

        //加速
        if (speedup_mode == true)
        {
            if (speed_cnt == 0)
            {
                my_speed = 6;
            }
            speed_cnt += Time.deltaTime;
        }
        if (speed_cnt >= 5)
        {
            speedup_mode = false;
            speed_cnt = 0;
        }

        //射击
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 shoot_dir = hit.point - centerposition;
                shoot_dir.y = 0;
                Quaternion rotation = Quaternion.LookRotation(shoot_dir);
                Quaternion shooter_rotation = Quaternion.FromToRotation(Vector3.back, shoot_dir)*Quaternion.Euler(-90f,0, 24.351f);
                shooter1.transform.rotation = shooter_rotation;
                shooter2.transform.rotation = shooter_rotation;
                shooter3.transform.rotation = shooter_rotation;
                switch (mode)
                {
                    case 1:
                        GameObject obj1 = Instantiate(bullet, centerposition, rotation);
                        break;
                    case 2:
                        GameObject obj2 = Instantiate(bullet, centerposition,rotation);
                        obj2.transform.Translate(0.15f, 0, 0);
                        GameObject obj3 = Instantiate(bullet, centerposition, rotation);
                        obj3.transform.Translate(-0.15f, 0, 0);
                        break;
                    case 3:
                        Fire(shoot_dir);
                        break;
                    case 4:
                        GameObject obj4 = Instantiate(wave, centerposition, rotation*Quaternion.Euler(-90f,0,180f));
                        break;
                }
            }

        }
        HPbar.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main,centerposition+Vector3.up*2);
    }
    public void Fire(Vector3 shoot_dir)
    {
        StartCoroutine(FireLaser(shoot_dir));
    }
    
    IEnumerator FireLaser(Vector3 shoot_dir)
    {
        Transform firePoint = wheel.transform;
        Vector3 currentOrigin = firePoint.position;
        Vector3 currentDirection = shoot_dir.normalized;
        float remainingRange = maxRange;
        int bounceCount = 0;

        // 存储路径点
        List<Vector3> pathPoints = new List<Vector3>();
        pathPoints.Add(currentOrigin);

        // 记录已伤害的敌人（防止重复伤害）
        HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();

        while (remainingRange > 0 && bounceCount <= maxBounces)
        {
            Ray ray = new Ray(currentOrigin, currentDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, remainingRange))
            {
                // 先处理这段路径上的敌人
                RaycastHit[] enemyHits = Physics.RaycastAll(currentOrigin, currentDirection, hit.distance, enemy_lay);
                foreach (RaycastHit enemyHit in enemyHits)
                {
                    GameObject enemyObj = enemyHit.collider.gameObject;
                    if (!damagedEnemies.Contains(enemyObj))
                    {
                        IDamageble enemy = enemyObj.GetComponent<IDamageble>();
                        if (enemy != null)
                        {
                            enemy.damage(laserDamage);
                            damagedEnemies.Add(enemyObj);
                        }
                    }
                }

                // 添加击中点
                pathPoints.Add(hit.point);

                // 判断是否击中墙体
                if (((1 << hit.collider.gameObject.layer) & wall_lay) != 0)
                {
                    if (bounceCount < maxBounces)
                    {
                        // 计算反射方向
                        currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                        currentOrigin = hit.point + currentDirection * 0.01f;
                        remainingRange -= hit.distance;
                        bounceCount++;
                    }
                    else
                    {
                        // 达到最大反弹次数，停止
                        break;
                    }
                }
                else
                {
                    // 不是墙体，继续穿透
                    currentOrigin = hit.point + currentDirection * 0.01f;
                    remainingRange -= hit.distance;
                }
            }
            else
            {
                // 没有击中任何东西
                Vector3 endPoint = currentOrigin + currentDirection * remainingRange;

                // 检测这段路径上的敌人
                RaycastHit[] enemyHits = Physics.RaycastAll(currentOrigin, currentDirection, remainingRange, enemy_lay);
                foreach (RaycastHit enemyHit in enemyHits)
                {
                    GameObject enemyObj = enemyHit.collider.gameObject;
                    if (!damagedEnemies.Contains(enemyObj))
                    {
                        Enemy enemy = enemyObj.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            enemy.damage(laserDamage);
                            damagedEnemies.Add(enemyObj);
                        }
                    }
                }

                pathPoints.Add(endPoint);
                break;
            }
        }

        // 绘制激光
        lineRenderer.positionCount = pathPoints.Count;
        for (int i = 0; i < pathPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, pathPoints[i]);
        }

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        lineRenderer.enabled = false;
    }
    public void damage(int damageamount)
    {
        if (noharm_mode == true)
        {
            return;
        }
        else
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
}
