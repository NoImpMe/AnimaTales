using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    CinemachineCamera allyAttackCam;
    [SerializeField]
    CinemachineCamera enemyAttackCam;
    public Camera mainCamera;
    float cameraposz = -10f;
    public float speed;
    public AnimatorController animatorController;
    GameObject instance;
    List<GameObject> instanceList;
    ParticleSystem ps;
    public IEnumerator ZoomSingleOpp(Transform hitter,Transform hitted,  bool isAlly, string skill)
    {
        
        if (isAlly)
        {
            allyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), allyAttackCam.transform.rotation);
            animator.SetTrigger("AllyAttack");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyAttackZoom"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();
            
             yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyZoomed"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted.transform.position.x, hitted.transform.position.y, hitted.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
     
            yield return animatorController.WaitForAnimationEnd(animator, "EnemyZoomed", instance);

        }
        else
        {
            enemyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), enemyAttackCam.transform.rotation);
            animator.SetTrigger("EnemyAttack");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttackZoom"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyZoomed"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted.transform.position.x, hitted.transform.position.y, hitted.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            yield return animatorController.WaitForAnimationEnd(animator, "AllyZoomed", instance);
        }

    }

    public IEnumerator ZoomMultiOpp(Transform hitter, List<Transform> hitted, bool isAlly, string skill)
    {
        if (isAlly)
        {
            allyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), allyAttackCam.transform.rotation);
            animator.SetTrigger("AllyMultiAttack");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyMultiEnemy"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyZoomed2"))
            {
                yield return null;
            }
            instanceList = new List<GameObject>();
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < hitted.Count; i++)
            {
                instanceList.Add(GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted[i].transform.position.x, hitted[i].transform.position.y, hitted[i].transform.position.z), Quaternion.identity));
            }
            ps = instanceList[0].GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            for(int i = 0; i < instanceList.Count; i++)
            {
                GameObject.Destroy(instanceList[i]);
            }
            

            yield return animatorController.WaitForAnimationEnd(animator, "EnemyZoomed2", instanceList[0]);
            yield return new WaitForSeconds(0.5f);

        }
        else
        {
            enemyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), enemyAttackCam.transform.rotation);
            animator.SetTrigger("EnemyMultiAttack");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyMultiAlly"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyZoomed3"))
            {
                yield return null;
            }
            instanceList = new List<GameObject>();
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < hitted.Count; i++)
            {
                instanceList.Add(GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted[i].transform.position.x, hitted[i].transform.position.y, hitted[i].transform.position.z), Quaternion.identity));
            }

            ps = instanceList[0].GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            for (int i = 0; i < instanceList.Count; i++)
            {
                GameObject.Destroy(instanceList[i]);
            }
            yield return animatorController.WaitForAnimationEnd(animator, "AllyZoomed3", instanceList[0]);
            yield return new WaitForSeconds(0.5f);

        }
    }
    public IEnumerator ZoomSingleIde(Transform hitter, Transform hitted, bool isAlly, string skill)
    {
        if (isAlly)
        {
            allyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), allyAttackCam.transform.rotation);
            animator.SetTrigger("AllyHeal");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyHealZoom"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyZoomed1"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted.transform.position.x, hitted.transform.position.y, hitted.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);



            yield return animatorController.WaitForAnimationEnd(animator, "AllyZoomed1", instance);
        }
        else
        {
            enemyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), enemyAttackCam.transform.rotation);
            animator.SetTrigger("EnemyHeal");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyHealZoom"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyZoomed1"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted.transform.position.x, hitted.transform.position.y, hitted.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            yield return animatorController.WaitForAnimationEnd(animator, "EnemyZoomed1", instance);
        }
    }
    public IEnumerator ZoomMultiIde(Transform hitter, List<Transform> hitted, bool isAlly, string skill)
    {
        if (isAlly)
        {
            allyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), allyAttackCam.transform.rotation);
            animator.SetTrigger("AllyMultiHeal");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyMultiAlly"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyZoomed2"))
            {
                yield return null;
            }
            instanceList = new List<GameObject>();
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < hitted.Count; i++)
            {
                instanceList.Add(GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted[i].transform.position.x, hitted[i].transform.position.y, hitted[i].transform.position.z), Quaternion.identity));
            }

            ps = instanceList[0].GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            for (int i = 0; i < instanceList.Count; i++)
            {
                GameObject.Destroy(instanceList[i]);
            }

            yield return animatorController.WaitForAnimationEnd(animator, "AllyZoomed2", instanceList[0]);
        }
        else
        {
            enemyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), enemyAttackCam.transform.rotation);
            animator.SetTrigger("EnemyMultiHeal");
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyMultiEnemy"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);

            ps = instance.GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            GameObject.Destroy(instance);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyZoomed3"))
            {
                yield return null;
            }
            instanceList = new List<GameObject>();
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < hitted.Count; i++)
            {
                instanceList.Add(GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted[i].transform.position.x, hitted[i].transform.position.y, hitted[i].transform.position.z), Quaternion.identity));
            }

            ps = instanceList[0].GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(0.5f);

            yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
            for (int i = 0; i < instanceList.Count; i++)
            {
                GameObject.Destroy(instanceList[i]);
            }
            yield return animatorController.WaitForAnimationEnd(animator, "EnemyZoomed3", instanceList[0]);
        }
    }
    public IEnumerator ZoomRoundOpp(Transform hitter, Transform hitted, string skill)
    {
        enemyAttackCam.ForceCameraPosition(new Vector3(hitter.position.x, hitter.position.y, cameraposz), enemyAttackCam.transform.rotation);
        animator.SetTrigger("RoundAttack");
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("BossZoom"))
        {
            yield return null;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BossZoom"))
        {
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill), new Vector3(hitter.transform.position.x, hitter.transform.position.y, hitter.transform.position.z), Quaternion.identity);
        }        
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("BossIdle"))
        {
            yield return null;
        }
        

        ps = instance.GetComponent<ParticleSystem>();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        GameObject.Destroy(instance);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("AllyZoomed4"))
        {
            yield return null;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AllyZoomed4"))
        {
            instance = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + skill + "ed"), new Vector3(hitted.transform.position.x, hitted.transform.position.y, hitted.transform.position.z), Quaternion.identity);
        }

        ps = instance.GetComponent<ParticleSystem>();
        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        GameObject.Destroy(instance);
        yield return animatorController.WaitForAnimationEnd(animator, "AllyZoomed4", instance);
    }


}
