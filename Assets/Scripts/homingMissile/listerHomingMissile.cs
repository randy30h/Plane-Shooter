using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class listerHomingMissile : MonoBehaviour
{
    public static listerHomingMissile current;

    public List<GameObject> enemiesListed = new List<GameObject>(2);

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        for(int i = 0; i < enemiesListed.Count; i++)
        {
            if (!enemiesListed[i].activeSelf)
            {
                enemiesListed.Remove(enemiesListed[i]);
            }
        }
    }
}
