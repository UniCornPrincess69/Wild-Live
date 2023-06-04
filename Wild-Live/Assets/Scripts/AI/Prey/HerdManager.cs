using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//If there are ever more then one herd. This will be used
public class HerdManager : MonoBehaviour
{
    [field: SerializeField]
    public List<Herd> Herds { get; set; }

}
