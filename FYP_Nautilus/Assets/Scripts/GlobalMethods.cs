using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public static class GlobalMethods
{
    public static IEnumerator printDialog(GameObject text, string words, float duration)
    {
        text.gameObject.SetActive(true);
        text.GetComponentInChildren<TextMeshProUGUI>().text = "";
        foreach(char ch in words)
        {
            text.GetComponentInChildren<TextMeshProUGUI>().text += ch;
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        text.gameObject.SetActive(false);
    }
}
