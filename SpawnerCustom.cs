using UnityEngine;
using System.Collections;

public class SpawnerCustom : MonoBehaviour
{
    public GameObject prefabA;
    public Transform target;
    int count = 0;

    public int showCount = 0;
    public int max = 10;

    void Start()
    {

    }

    void Update()
    {

    }

    public GameObject spawnObject()
    {
        GameObject spawned = null;
        if (count < max)
        {
            if (prefabA != null)
            {
                spawned = (GameObject)Instantiate(prefabA, transform.position, transform.rotation);
                ThiefBehavior thief = spawned.GetComponent<ThiefBehavior>();
				if (thief)
					thief.TargetTransform = target;
				else {
					CiclopBehavior cic = spawned.GetComponent<CiclopBehavior>();
					if (cic)
						cic.TargetTransform = target;
				}
            }
            count++;
            showCount = count;            
        }
        return spawned;
    }

    public void reset()
    {
        showCount = count = 0;
    }
}
