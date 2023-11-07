using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public class PathsLabelsController {
    [SerializeField] public GameObject pathsLabels;

    public void RevealCost (int city_a, int city_b, int cost) {
        int minor = Mathf.Min(city_a, city_b);
        int mayor = Mathf.Max(city_a, city_b);
        GameObject pathLabel = pathsLabels.transform.Find($"{minor}{mayor}").gameObject;
        TextMeshPro label = pathLabel.GetComponent<TextMeshPro>();
        Animator animator = pathLabel.GetComponent<Animator>();
        GameController.instance.StartCoroutine(ShowText(label, cost.ToString(), animator));       
        
    }

    private IEnumerator ShowText(TextMeshPro label, string text, Animator animator)
    {
        yield return new WaitForSeconds(.5f);
        animator.SetTrigger("reveal");
        yield return new WaitForSeconds(1f);
        label.text = text;

    }


}