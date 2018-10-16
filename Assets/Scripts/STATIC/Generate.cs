using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generate {

    static GameObject damageValuePrefab = (GameObject)Resources.Load("UI/DamageValue/DamageValueCanvas",typeof(GameObject));

    public static void DamageValue(Vector2 position, int value, bool isCrit = false)
    {
        GameObject dmgVal = Object.Instantiate(damageValuePrefab, position, Quaternion.identity);

        dmgVal.transform.Find("DamageValue").GetComponent<DamageValue>().Init(value, isCrit);
    }

}
