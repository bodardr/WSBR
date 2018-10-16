using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageValue : MonoBehaviour {

    /// <summary>
    /// Initialises the Damage Value text to the value.
    /// </summary>
    /// <param name="value">the Damage Value.</param>
    /// <param name="isCrit">indicates if the Damage was crit or not (false by default).</param>
    public void Init(int value, bool isCrit = false)
    {
        GetComponent<TextMeshProUGUI>().text = value.ToString();

        if (isCrit)
            GetComponent<TextMeshProUGUI>().color = Color.yellow;
    }

    /// <summary>
    /// Destroys the Damage Value. (Animation Callback)
    /// </summary>
    public void Destroy()
    {
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }
}
