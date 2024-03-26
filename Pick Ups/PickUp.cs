using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public int amountValue;

    [Header("Random Splash")]
    public Transform objTrans;
    public float delay = 0;
    public float pastTime = 0;
    private float when;
    private Vector3 off;

    [Header("Magnet")]
    public Rigidbody2D rb;
    public GameObject player;
    public bool magnetize = false;
    public float magnetRadius = 2f;
    public bool pickupable = true;

    void Awake()
    {
        off = new Vector3(Random.Range(-2, 2), off.y, off.z);
        off = new Vector3(off.x, Random.Range(-2, 2), off.z);
        when = Random.Range(0, 2);
    }

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        StartCoroutine(MagnetCo());
    }

    // Update is called once per frame
    void Update()
    {

        if(when >= delay)
        {
            pastTime = Time.deltaTime;
            objTrans.position += off * Time.deltaTime;
            delay += pastTime;
        }

        if (magnetize && Vector3.Distance(player.transform.position, transform.position) <= magnetRadius)
        {
            if (this is Item && InventoryItemSlots.MyInstance.InventoryIsFull(this as Item))
            {
                //Debug.Log("Inventory is full!");
                pickupable = false;
            }

            else
            {
                pickupable = true;
                Vector3 playerPoint = Vector3.MoveTowards(transform.position,
                    player.transform.position + new Vector3(0, -0.3f, 0), 20 * Time.deltaTime);
                rb.MovePosition(playerPoint);
            }
        }
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Wall"))
        {
            off = -off;
        }

        if(other.CompareTag("Player") && other.isTrigger && magnetize)
        {
            if(pickupable)
            {
                if (this is Item)
                {
                    // Show ItemPickUpPopUp when an Item is Picked Up
                    ItemPickUpPopUpController.MyInstance.CreateInstance(this as Item);

                    this.gameObject.SetActive(false);
                    return;
                }
                this.gameObject.SetActive(false);
                //Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator MagnetCo()
    {
        yield return new WaitForSeconds(1f);
        magnetize = true;
    }

    private IEnumerator DestroyCo()
    {
        Destroy(this.gameObject);
        yield return new WaitForSeconds(1f);
    }
}
