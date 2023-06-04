using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scent : MonoBehaviour
{
    #region Variables
    [field: SerializeField]
    public SphereCollider Collider { get; set; }

    [SerializeField]
    private GameObject scent;
    private float wait = 0f;
    private bool herdLeft = false;

    public Herd Herd { get; set; }

    public Vector3 ScentOrigin { get; set; }

    public static Scent NextScent { private get; set; } = null;
    public Transform NextScentPos
    {
        get
        {
            if (NextScent == null) return Herd.transform;
            else return NextScent.transform;
        }
    }
    #endregion

    //Assigment of the Scent to the herd.
    //Subscription to IsStationaryChanged to handle the shrinking of the scent
    public void Initialize(Herd herd)
    {
        this.Herd = herd;
        herd.IsStationaryChanged += IsStationaryChanged;
    }

    /// <summary>
    /// Method handling the start of the shrink coroutine after a period of time.
    /// </summary>
    /// <param name="val">Parameter acquired via subscription</param>
    private void IsStationaryChanged(bool val)
    {
        if (val == false)
        {
            StartCoroutine(ScentShrink());
            Debug.Log("SHRINKING");
        }
    }

    void Start()
    {
        wait = 0f;
        ScentOrigin = transform.position;
        StartCoroutine(ScentGrow());
    }

    /// <summary>
    /// Handling of the growth of the scent collider over a time period, as long as the herd is stationary
    /// </summary>
    /// <returns></returns>
    IEnumerator ScentGrow()
    {
        while (Herd.IsStationary)
        {
            Collider.radius += 2f;
            yield return new WaitForSeconds(1f);
        }
    }
    
    IEnumerator ScentShrink()
    {
        yield return new WaitForSeconds(20f);

        while (Collider.radius >= 0.5f)
        {
            Collider.radius -= 2f;
            if (Collider.radius <= 0.5f)
            {
                Destroy(scent);
            }
            yield return new WaitForSeconds(3f);
        }
    }

    private void OnDisable()
    {
        Herd.IsStationaryChanged -= IsStationaryChanged;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Herd")) 
        {
            herdLeft = true;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(scent.transform.position, Collider.radius);
    }
#endif
}
