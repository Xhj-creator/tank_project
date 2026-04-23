using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autowallcontrol : MonoBehaviour
{
    public float lowerHeight = 3f;
    public float speed = 2f;
    public LayerMask playerlayer;
    public LayerMask enemylayer;
    public Vector3 checkBoxSize = new Vector3(2f, 10f, 2f);

    private Vector3 originalPosition;
    private Vector3 loweredPosition;
    private bool isLowered = false;
    private float tankClearTimer = 0f;      // 上方无坦克的计时
    private Coroutine moveCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        enemylayer= LayerMask.GetMask("enemy_lay");
        playerlayer = LayerMask.GetMask("ayer_lay");
        originalPosition = transform.position;
        loweredPosition = originalPosition - Vector3.up * lowerHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLowered)
        {
            tankClearTimer += Time.deltaTime;
            if (!IsTankAbove()&&(tankClearTimer>=5))
            {
                RaiseWall();
                tankClearTimer = 0f;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            LowerWall();
        }
    }
    void LowerWall()
    {
        if (isLowered)
        {
            return;
        }
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToPosition(loweredPosition));
        isLowered = true;
        tankClearTimer = 0f;
    }
    void RaiseWall()
    {
        // 再次确认上方没有坦克（防止协程延迟期间坦克进入）
        if (IsTankAbove()) return;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveToPosition(originalPosition));
        isLowered = false;
        tankClearTimer = 0f;
    }
    IEnumerator MoveToPosition(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        moveCoroutine = null;
    }
    bool IsTankAbove()
    {
        Vector3 checkCenter = transform.position + Vector3.up * (lowerHeight + 0.5f);

        Collider[] tanks1 = Physics.OverlapBox(checkCenter, checkBoxSize / 2f, Quaternion.identity, playerlayer);
        Collider[] tanks2 = Physics.OverlapBox(checkCenter, checkBoxSize / 2f, Quaternion.identity, enemylayer);
        return (tanks1.Length+ tanks1.Length) > 0;
    }
}
