using UnityEngine;
using System.Collections;

[System.Serializable]
public class ReloadSound
{
    public string name = "Mag out";
    public AudioClip clip;
    public float length;
}

public class Weapon : MonoBehaviour
{
    public Animation anim;
    public AnimationClip fireAnim;
    public AnimationClip reloadAnim;
    public AnimationClip drawAnim;

    #region bools
    public bool reloading;
    public bool[] canAims;
    private bool canAim;
    public bool[] canReloads;
    private bool canReload;
    public bool[] canFires;
    private bool canFire;
    #endregion

    #region stats
    public float fireRate = 0.1f;
    public float timer = 0;
    [SerializeField]
    protected int bulletsLeft = 30;
    [SerializeField]
    protected int bulletsPerMag = 30;
    [SerializeField]
    protected int magsLeft = 10;
    public float range = 2000;
    public float damageMin = 10;
    public float damageMax = 20;
    public Transform bulletGo;
    public LayerMask hitLayers;
    public GameObject blood;
    public GameObject concrete;
    public GameObject wood;
    public GameObject metal;
    public GameObject dirt;
    #endregion

    #region readOnly
    public int bulletsLeftRead = 30;
    public int bulletsPerMagRead = 30;
    public int magsLeftRead = 10;
    #endregion


    #region components
    public CharacterValues cv;
    public PlayerAnimations pa;
    #endregion

    #region sound
    public AudioSource localSource;
    public AudioClip fireSound;
    public AudioClip drawSound;
    public ReloadSound[] reloadSounds;
    #endregion

    #region ads
    public Camera cam;
    public bool aiming;
    public float hipFov = 75;
    public float aimFov = 55;
    private float curFov = 75;
    public Vector3 hipPos;
    public Vector3 crouchPos;
    public Vector3 aimPos;
    private Vector3 curPos;
    #endregion

    #region recoil
    public Transform camKB;
    public Transform wepKB;
    public float minKB;
    public float maxKB;
    public float minKBSide;
    public float maxKBSide;
    public float returnSpeed = 5f;
    #endregion

    #region muzzle
    public GameObject muzzle;
    #endregion


    void Start()
    {
        muzzle.SetActive(false);
        if (GetComponent<NetworkView>().isMine)
        {
            StartCoroutine(CheckBools());
            StartCoroutine(Draw());
        }
        else
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        if (GetComponent<NetworkView>().isMine)
        {
            bulletsLeftRead = bulletsLeft;
            bulletsPerMagRead = bulletsPerMag;
            magsLeftRead = magsLeft;
            camKB.localRotation = Quaternion.Lerp(camKB.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
            wepKB.localRotation = Quaternion.Lerp(wepKB.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, curFov, Time.deltaTime * 10);
            transform.localPosition = Vector3.Lerp(transform.localPosition, curPos, Time.deltaTime * 10);
            if (Screen.lockCursor)
                CheckInput();
            timer = Mathf.Clamp(timer, 0, 10);
            if (timer > 0) timer -= Time.deltaTime;
            canReloads[1] = true;
            canAims[1] = !cv.running;
            canFires[1] = !cv.running;
            if (aiming)
            {
                curFov = aimFov;
                curPos = aimPos;
            }
            else
            {
                curFov = hipFov;
                if (cv.state == 0)
                {
                    curPos = hipPos;
                }
                else if (cv.state == 1)
                {
                    curPos = crouchPos;
                }
            }

            if (!canAim) aiming = false;
        }
        else
        {
            this.enabled = false;
        }
    }

    void CheckInput()
    {
        if (canAim && Input.GetKeyDown(KeyCode.Mouse1))
        {
            aiming = !aiming;
        }
        if (!reloading && timer == 0 && canFire && Input.GetKey(KeyCode.Mouse0) && bulletsLeft > 0 && Screen.lockCursor)
        {
            FireOneShot();
        }
        if (!reloading && canReload && magsLeft > 0 && Input.GetKeyDown(KeyCode.R) && Screen.lockCursor)
        {
            reloading = true;
            StartCoroutine(Reload());
        }
    }

    void FireOneShot()
    {
        timer = fireRate;
        anim.Rewind(fireAnim.name);
        anim.Play(fireAnim.name);
        localSource.clip = fireSound;
        localSource.PlayOneShot(fireSound);
        StartCoroutine(MuzzleFlash());
        StartCoroutine(Kick3(camKB, new Vector3(-Random.Range(minKB, maxKB), Random.Range(minKBSide, maxKBSide), 0), 0.1f));
        StartCoroutine(Kick3(wepKB, new Vector3(-Random.Range(minKB, maxKB), Random.Range(minKBSide, maxKBSide), 0), 0.1f));
        pa.GetComponent<NetworkView>().RPC("FireAnim", RPCMode.Others);
        RaycastHit hit2;
        if (Physics.Raycast(bulletGo.position, bulletGo.forward, out hit2, range, hitLayers))
        {
            OnHit(hit2);
        }
        bulletsLeft--;
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
            if (hit.transform.root.GetComponent<NetworkView>())
                hit.transform.root.GetComponent<NetworkView>().RPC("ApplyDamage", RPCMode.AllBuffered, Random.Range(damageMin, damageMax), 1);
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

    IEnumerator Kick3(Transform goTransform, Vector3 kbDirection, float time)
    {
        Quaternion startRotation = goTransform.localRotation;
        Quaternion endRotation = goTransform.localRotation * Quaternion.Euler(kbDirection);
        float rate = 1.0f / time;
        var t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            goTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }
    }

    IEnumerator Reload()
    {
        reloading = true;
        canAims[0] = false;
        canFires[0] = false;
        canReloads[0] = false;
        StartCoroutine(ReloadingSound());
        pa.GetComponent<NetworkView>().RPC("ReloadAnim", RPCMode.Others, reloadAnim.length);
        anim.Play(reloadAnim.name);
        yield return new WaitForSeconds(reloadAnim.length);
        canAims[0] = true;
        canFires[0] = true;
        canReloads[0] = true;
        bulletsLeft = bulletsPerMag;
        magsLeft--;
        reloading = false;
    }

    IEnumerator ReloadingSound()
    {
        foreach (ReloadSound lol in reloadSounds)
        {
            yield return new WaitForSeconds(lol.length);
            localSource.clip = lol.clip;
            localSource.Play();
        }
    }

    IEnumerator CheckBools()
    {
        CheckAim();
        CheckReload();
        CheckFire();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CheckBools());
    }

    IEnumerator MuzzleFlash()
    {
        muzzle.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzle.SetActive(false);
    }

    void CheckAim()
    {
        canAim = false;
        foreach (bool lol in canAims)
        {
            if (!lol) return;
        }
        canAim = true;
    }

    void CheckReload()
    {
        canReload = false;
        foreach (bool lol in canReloads)
        {
            if (!lol) return;
        }
        canReload = true;
    }

    void CheckFire()
    {
        canFire = false;
        foreach (bool lol in canFires)
        {
            if (!lol) return;
        }
        canFire = true;
    }

    IEnumerator Draw()
    {
        canAims[0] = false;
        canFires[0] = false;
        canReloads[0] = false;
        localSource.clip = drawSound;
        localSource.Play();
        anim.Play(drawAnim.name);
        pa.GetComponent<NetworkView>().RPC("DrawAnim", RPCMode.Others);
        yield return new WaitForSeconds(drawAnim.length);
        canAims[0] = true;
        canFires[0] = true;
        canReloads[0] = true;
    }
}
