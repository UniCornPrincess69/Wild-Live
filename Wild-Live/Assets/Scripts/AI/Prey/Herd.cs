using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Herd : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject child = null;

    [SerializeField]
    private GameObject scentPrefab = null;

    [SerializeField]
    private GameObject preyPrefab = null;

    [SerializeField]
    private int preyCount = 0;

    [SerializeField]
    private bool isStationary = false;

    [field: SerializeField]
    private Transform MovingCenter { get; set; } = null;

    public bool IsStationary
    {
        get { return isStationary; }
        set
        {
            isStationary = value;
            IsStationaryChanged.Invoke(isStationary);
        }
    }

    private Scent lastScent = null;
    private Scent currentScent = null;

    [field: SerializeField]
    public List<Prey> Preys { get; set; } = new List<Prey>();

    private bool isInstantiated = false;
    #endregion

    public System.Action<bool> IsStationaryChanged = (isStationary) => {  };

    private void Awake()
    {
        for (int i = 0; i < preyCount; i++)
        {
            var rngX = Random.Range(-5, 5);
            var rngY = Random.Range(-5, 5);
            var pos = new Vector3(rngX, transform.position.y, rngY);

            var temp = Instantiate(preyPrefab, MovingCenter);
            temp.transform.localPosition = pos;
            Preys.Add(temp.GetComponent<Prey>());
        }
    }

    private void Start()
    {
        
        for (int i = 0; i < Preys.Count; i++)
        {
            Preys[i].Herd = this;
        }

    }

    //Instantiation of the Scent at the position of the herd, when it is stationary
    private void Update()
    {
        
        if (!IsStationary) isInstantiated = false;
        else
        {
            if (!isInstantiated)
            {
                var temp = Instantiate(scentPrefab, MovingCenter.position, Quaternion.identity);
                var scent = temp.GetComponent<Scent>();
                scent.Initialize(this);
                isInstantiated = true;
                if (lastScent == null) lastScent = scent;
                else
                {
                    if (currentScent == null) currentScent = scent;
                    else
                    {
                        lastScent = currentScent;
                        currentScent = scent;
                        Scent.NextScent = currentScent;
                    }
                }
            }
        }
    }
}
