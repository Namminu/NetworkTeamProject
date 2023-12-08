using UnityEngine;
using UnityEngine.UIElements;

public class BombController : MonoBehaviour
{
    public float bombTime = 2.5f;
    public int streamLength;

    public GameObject BombStream;
    public GameObject Explosion;

    private Animator animator;


    private bool isBomb = false;
    private bool isburstsfast = false;


    private string bombName = "";
    public string overlappingPlayer = "";               // Player�� ��ź ��ġ�� �������� �÷��̾ �Ǻ��ϱ� ���� string

    public string getBombName() { return bombName; }
    public void setBombName(string name) { bombName = name; }

    private int raycastDistance;

    private float distanceUp;
    private float distanceDown;
    private float distanceLeft;
    private float distanceRight;
    enum Way
    {
        up = 0,
        down,
        left,
        right,
        none
    }
    //private Way way;

    // Start is called before the first frame upd ate
    void Start()
    {

        animator = GetComponent<Animator>();
        Invoke("BombAction", bombTime);
        raycastDistance = streamLength;

        distanceUp = streamLength;
        distanceDown = streamLength;
        distanceLeft = streamLength;
        distanceRight = streamLength;

        DistanceCheck(1);

    }

    // Update is called once per frame
    void Update()
    {



    }


    void BombAction()
    {
        if (isBomb)
            return;

        //if(!isburstsfast)
        {
            // �����۹ڽ����� �Ÿ�
           {
                RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, LayerMask.GetMask("ItemBox"));
                if (hitUp.collider != null)
                {
                    if (distanceUp == hitUp.distance)
                    {
                        hitUp.collider.GetComponent<CreateRandomItem>().SpawnRandomObject();
                    }
                }

                // ����� Ray �׸���
                Debug.DrawRay(transform.position, Vector2.up * raycastDistance, Color.red);

                // ��
                RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, LayerMask.GetMask("ItemBox"));
                if (hitDown.collider != null)
                {
                    if (distanceDown == hitDown.distance)
                    {
                        hitDown.collider.GetComponent<CreateRandomItem>().SpawnRandomObject();
                    }

                }

                // ����� Ray �׸���
                Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.green);

                // ��
                RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, LayerMask.GetMask("ItemBox"));
                if (hitLeft.collider != null)
                {
                    if (distanceLeft == hitLeft.distance)
                    {
                        hitLeft.collider.GetComponent<CreateRandomItem>().SpawnRandomObject();
                    }
                }

                // ����� Ray �׸���
                Debug.DrawRay(transform.position, Vector2.left * raycastDistance, Color.blue);

                // ��
                RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, LayerMask.GetMask("ItemBox"));
                if (hitRight.collider != null)
                {
                    if (distanceRight == hitRight.distance)
                    {
                        hitRight.collider.GetComponent<CreateRandomItem>().SpawnRandomObject();
                    }
                }

                // ����� Ray �׸���
                Debug.DrawRay(transform.position, Vector2.right * raycastDistance, Color.yellow);
            }
        }
        //else
        {
            //DistanceCheck(2);
        }

        




        Instantiate(BombStream, transform.position, Quaternion.identity);
        for (int i = 1; i <= streamLength; i++)
        {

            //��
            if (distanceUp >= i)
            {


                Instantiate(BombStream, transform.position + new Vector3(0, i, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
            }

            // �Ʒ�
            if (distanceDown >= i)
            {


                Instantiate(BombStream, transform.position + new Vector3(0, -i, 0), Quaternion.Euler(new Vector3(0, 0, -90)));
            }

            // ����
            if (distanceLeft >= i)
            {


                Instantiate(BombStream, transform.position + new Vector3(-i, 0, 0), Quaternion.Euler(new Vector3(0, 0, 180)));
            }

            // ������
            if (distanceRight >= i)
            {


                Instantiate(BombStream, transform.position + new Vector3(i, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            }

        }
        isBomb = true;

        GetComponent<BoxCollider2D>().enabled = false;

        GameObject bombExplosion = Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(bombExplosion, 0.8f);
        Destroy(gameObject);

    }

    // ���ٱ⿡ ��ź�� ������� ������
    public void BombBombBomb()
    {
        animator.SetTrigger("isBomb");
        isburstsfast = true;
        BombAction();
    }

    public void BombstreamLength(int length)
    {
        Debug.Log(length);
        streamLength = length;
    }

    // �Ÿ� üũ    n = 1 : �����۹ڽ� üũ��
    //             n = 2 : ������ �ڽ� ��������   
    void DistanceCheck(int n)
    {
        if(n == 1)
        // �� �Ÿ�
        {
             // ��
             RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, LayerMask.GetMask("Wall"));
             if (hitUp.collider != null)
             {
                 distanceUp = hitUp.distance;

                 Debug.Log("�� ������� �Ÿ�: " + distanceUp);
             }

             // ����� Ray �׸���
             Debug.DrawRay(transform.position, Vector2.up * raycastDistance, Color.red);

             // ��
             RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, LayerMask.GetMask("Wall"));
             if (hitDown.collider != null)
             {
                 distanceDown = hitDown.distance;
                 Debug.Log("�� ������� �Ÿ�: " + distanceDown);
             }

             // ����� Ray �׸���
             Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.green);

             // ��
             RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, LayerMask.GetMask("Wall"));
             if (hitLeft.collider != null)
             {
                 distanceLeft = hitLeft.distance;
                 Debug.Log("�� ������� �Ÿ�: " + distanceLeft);
             }

             // ����� Ray �׸���
             Debug.DrawRay(transform.position, Vector2.left * raycastDistance, Color.blue);

             // ��
             RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, LayerMask.GetMask("Wall"));
             if (hitRight.collider != null)
             {
                 distanceRight = hitRight.distance;
                 Debug.Log("�� ������� �Ÿ�: " + distanceRight);
             }

             // ����� Ray �׸���
             Debug.DrawRay(transform.position, Vector2.right * raycastDistance, Color.yellow);
         }


         //������ �ڽ� �Ÿ�
         {
             RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, LayerMask.GetMask("ItemBox"));
             if (hitUp.collider != null)
             {
                 if (distanceUp > hitUp.distance)
                 {
                     distanceUp = hitUp.distance;
                    
                    if (n == 2)
                        hitUp.collider.GetComponent<CreateRandomItem>().SpawnRandomObject();
                }



                 Debug.Log("�� ������� �Ÿ�: " + distanceUp);
             }

             // ����� Ray �׸���
             Debug.DrawRay(transform.position, Vector2.up * raycastDistance, Color.red);

             // ��
             RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, LayerMask.GetMask("ItemBox"));
             if (hitDown.collider != null)
             {
                 if (distanceDown > hitDown.distance)
                 {
                     distanceDown = hitDown.distance;

                    if (n == 2)
                        hitDown.collider.GetComponent<CreateRandomItem>().SpawnRandomObject();
                    
                }



                 Debug.Log("�� ������� �Ÿ�: " + distanceDown);
             }

             // ����� Ray �׸���
             Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.green);

             // ��
             RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, LayerMask.GetMask("ItemBox"));
             if (hitLeft.collider != null)
             {
                 if (distanceLeft > hitLeft.distance)
                 {
                     distanceLeft = hitLeft.distance;

                    if (n == 2)
                        hitLeft.collider.GetComponent<CreateRandomItem>().SpawnRandomObject();
                }



                 Debug.Log("�� ������� �Ÿ�: " + distanceLeft);
             }

             // ����� Ray �׸���
             Debug.DrawRay(transform.position, Vector2.left * raycastDistance, Color.blue);

             // ��
             RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, LayerMask.GetMask("ItemBox"));
             if (hitRight.collider != null)
             {
                 if (distanceRight > hitRight.distance)
                 {
                     distanceRight = hitRight.distance;

                    if (n == 2)
                        hitRight.collider.GetComponent<CreateRandomItem>().SpawnRandomObject();
                }


                 Debug.Log("�� ������� �Ÿ�: " + distanceRight);
             }

             // ����� Ray �׸���
             Debug.DrawRay(transform.position, Vector2.right * raycastDistance, Color.yellow);
            
        }
        if (n == 2)
            Debug.Log("2222222222");
    }
}

