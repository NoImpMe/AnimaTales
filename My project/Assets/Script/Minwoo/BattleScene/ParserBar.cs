using System.Collections;
using UnityEngine;

public class ParserBar : MonoBehaviour
{
    [SerializeField]
    private ParserBarController parserBarController;
    public float maxPoint = 1;
    public float thisPoint = 0;
    public void Initialize()
    {
        parserBarController.UpdatePoint(thisPoint / maxPoint);
    }

    public IEnumerator PutDamage(float damage)
    {
        
        thisPoint += damage;
        if (thisPoint > maxPoint) 
        {
            maxPoint = thisPoint;
        }
        if (thisPoint < 0)
        {
            thisPoint = 0;
        }
        StartCoroutine(parserBarController.SmoothPointChange(parserBarController.parserBarFill.fillAmount, thisPoint / maxPoint, 1.3f));

        yield return null;
    }
    public float SetMaxPoint(float maxPoint)
    {
        if (this.maxPoint < maxPoint)
        {
            this.maxPoint = maxPoint;
            return -1;
        }
        else
        {
            return this.maxPoint;
        }
    }
    
}
