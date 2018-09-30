using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour
{
    public CharacterValues cv;
    public string dir;
    public Transform arms;

    public Animation anim;
    public AnimationClip fire;
    public AnimationClip aim;
    public AnimationClip noAim;
    public AnimationClip reload;

    public AnimationClip jump;
    public AnimationClip idleStand;
    public AnimationClip run;
    public AnimationClip walkFw;
    public AnimationClip walkBw;
    public AnimationClip walkLeft;
    public AnimationClip walkRight;

    public AnimationClip idleCrouch;
    public AnimationClip crouchFw;
    public AnimationClip crouchBw;
    public AnimationClip crouchLeft;
    public AnimationClip crouchRight;

    public float delta = 0;

    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip drawSound;
    public ReloadSound[] reloadSounds;

    #region muzzle
    public GameObject muzzle;
    #endregion

    public float range = 2000;
    public Transform bulletGo;
    public LayerMask hitLayers;
    public GameObject blood;
    public GameObject concrete;
    public GameObject wood;
    public GameObject metal;
    public GameObject dirt;

    void Start()
    {
        anim.wrapMode = WrapMode.Loop;
        anim[fire.name].layer = 2;
        anim[fire.name].AddMixingTransform(arms);
        anim[fire.name].speed = 0.5f;
        anim[aim.name].layer = 1;
        anim[aim.name].AddMixingTransform(arms);
        anim[noAim.name].layer = 1;
        anim[noAim.name].AddMixingTransform(arms);
        anim[reload.name].layer = 2;
        anim[reload.name].AddMixingTransform(arms);
        anim[reload.name].wrapMode = WrapMode.Once;
        anim[jump.name].wrapMode = WrapMode.ClampForever;
        anim[fire.name].wrapMode = WrapMode.Once;
        muzzle.SetActive(false);
    }

    void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, delta, 0), Time.deltaTime * 5);
        SetAnimFromDir(cv.ver, cv.hor);
        if (cv.aiming)
        {
            anim.CrossFade(aim.name);
        }
        else
        {
            anim.CrossFade(noAim.name);
        }
        if (!cv.grounded)
        {
            anim.CrossFade(jump.name);
            delta = 0;
        }
        else
        {
            if (cv.running)
            {
                if (dir == "Forward ")
                {
                    anim.CrossFade(run.name);
                    anim[run.name].speed = cv.velPercent;
                    delta = 0;
                }
                else if (dir == "Forward Right ")
                {
                    anim.CrossFade(run.name);
                    anim[run.name].speed = cv.velPercent;
                    delta = 45;
                }
                else if (dir == "Forward Left ")
                {
                    anim.CrossFade(run.name);
                    anim[run.name].speed = cv.velPercent;
                    delta = -45;
                }
            }
            else
            {
                if (cv.state == 0)
                {
                    if (dir == "")
                    {
                        anim.CrossFade(idleStand.name);
                        delta = 0;
                    }
                    else if (dir == "Forward ")
                    {
                        anim.CrossFade(walkFw.name);
                        anim[walkFw.name].speed = cv.velPercent;
                        delta = 0;
                    }
                    else if (dir == "Backward ")
                    {
                        anim.CrossFade(walkBw.name);
                        anim[walkBw.name].speed = cv.velPercent;
                        delta = 0;
                    }
                    else if (dir == "Left ")
                    {
                        anim.CrossFade(walkLeft.name);
                        anim[walkLeft.name].speed = cv.velPercent;
                        delta = 0;
                    }
                    else if (dir == "Right ")
                    {
                        anim.CrossFade(walkRight.name);
                        anim[walkRight.name].speed = cv.velPercent;
                        delta = 0;
                    }
                    else if (dir == "Forward Right ")
                    {
                        anim.CrossFade(walkFw.name);
                        anim[walkFw.name].speed = cv.velPercent;
                        delta = 45;
                    }
                    else if (dir == "Forward Left ")
                    {
                        anim.CrossFade(walkFw.name);
                        anim[walkFw.name].speed = cv.velPercent;
                        delta = -45;
                    }
                    else if (dir == "Backward Right ")
                    {
                        anim.CrossFade(walkBw.name);
                        anim[walkBw.name].speed = cv.velPercent;
                        delta = -45;
                    }
                    else if (dir == "Backward Left ")
                    {
                        anim.CrossFade(walkBw.name);
                        anim[walkBw.name].speed = cv.velPercent;
                        delta = 45;
                    }
                }
                else if (cv.state == 1)
                {
                    if (dir == "")
                    {
                        anim.CrossFade(idleCrouch.name);
                        delta = 0;
                    }
                    else if (dir == "Forward ")
                    {
                        anim.CrossFade(crouchFw.name);
                        anim[crouchFw.name].speed = cv.velPercent;
                        delta = 0;
                    }
                    else if (dir == "Backward ")
                    {
                        anim.CrossFade(crouchBw.name);
                        anim[crouchBw.name].speed = cv.velPercent;
                        delta = 0;
                    }
                    else if (dir == "Left ")
                    {
                        anim.CrossFade(crouchLeft.name);
                        anim[crouchLeft.name].speed = cv.velPercent;
                        delta = 0;
                    }
                    else if (dir == "Right ")
                    {
                        anim.CrossFade(crouchRight.name);
                        anim[crouchRight.name].speed = cv.velPercent;
                        delta = 0;
                    }
                    else if (dir == "Forward Right ")
                    {
                        anim.CrossFade(crouchFw.name);
                        anim[crouchFw.name].speed = cv.velPercent;
                        delta = 45;
                    }
                    else if (dir == "Forward Left ")
                    {
                        anim.CrossFade(crouchFw.name);
                        anim[crouchFw.name].speed = cv.velPercent;
                        delta = -45;
                    }
                    else if (dir == "Backward Right ")
                    {
                        anim.CrossFade(crouchBw.name);
                        anim[crouchBw.name].speed = cv.velPercent;
                        delta = -45;
                    }
                    else if (dir == "Backward Left ")
                    {
                        anim.CrossFade(crouchBw.name);
                        anim[crouchBw.name].speed = cv.velPercent;
                        delta = 45;
                    }
                }
            }
        }
    }

    void SetAnimFromDir(float v, float h)
    {
        const float ensureOffset = 0.1f;
        string msg = "";
        if (cv.velMag > 0.1f)
        {
            if (v > ensureOffset)
            {
                msg += "Forward ";
            }
            else if (v < -ensureOffset)
            {
                msg += "Backward ";
            }

            if (h > ensureOffset)
            {
                msg += "Right ";
            }
            else if (h < -ensureOffset)
            {
                msg += "Left ";
            }
        }

        Debug.Log(msg);
        dir = msg;
    }

    [RPC]
    public void FireAnim()
    {
        anim.Rewind(fire.name);
        anim.CrossFade(fire.name, 0.05f);
        source.minDistance = 100;
        source.clip = fireSound;
        source.PlayOneShot(fireSound);
        StartCoroutine(MuzzleFlash());
        RaycastHit hit2;
        if (Physics.Raycast(bulletGo.position, bulletGo.forward, out hit2, range, hitLayers))
        {
            OnHit(hit2);
        }
    }

    IEnumerator MuzzleFlash()
    {
        muzzle.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzle.SetActive(false);
    }

    void OnHit(RaycastHit hit)
    {
        if (hit.rigidbody)
        {
            hit.rigidbody.AddForceAtPosition(2000 * bulletGo.forward, hit.point);
        }
        if (hit.transform.tag == "Player")
        {
            Instantiate(blood, hit.point, Quaternion.identity);
        }
        else
        {
            if (hit.transform.tag == "Wood")
            {
                GameObject theObj = Instantiate(wood, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                theObj.transform.parent = hit.transform;
            }
            else if (hit.transform.tag == "Metal")
            {
                GameObject theObj = Instantiate(metal, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                theObj.transform.parent = hit.transform;
            }
            else if (hit.transform.tag == "Dirt")
            {
                GameObject theObj = Instantiate(dirt, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                theObj.transform.parent = hit.transform;
            }
            else
            {
                GameObject theObj = Instantiate(concrete, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                theObj.transform.parent = hit.transform;
            }
        }
    }

    [RPC]
    public void ReloadAnim(float length)
    {
        StartCoroutine(ReloadingSound());
        anim[reload.name].speed = reload.length / length;
        anim.Rewind(reload.name);
        anim.Play(reload.name);
    }

    [RPC]
    public void DrawAnim()
    {
        //anim.Play(draw.name);
        source.minDistance = 6;
        source.clip = drawSound;
        source.Play();
    }



    IEnumerator ReloadingSound()
    {
        foreach (ReloadSound lol in reloadSounds)
        {
            yield return new WaitForSeconds(lol.length);
            source.minDistance = 6;
            source.clip = lol.clip;
            source.Play();
        }
    }
}
