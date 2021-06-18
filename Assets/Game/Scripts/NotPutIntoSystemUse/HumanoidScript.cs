using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
namespace RPG.Character
{
    public class HumanoidScript : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] clips;
        private AudioSource audioSource;
        public Transform refMaulGrip;
        public Transform weaponParent;
        public Maul weapon;
        public Maul weaponFab;
        public KeyCode ShieldWeaponUnsheath;

        AnimController animScript;
        //animAnimator animAnimator;
        public bool isalive;

        public GameObject sparks;
        public GameObject wind;
        public GameObject glow;

        public bool shieldStatus;
        // public UnityEngine.Animations.Rigging.Rig handIK;
        public Animator animControl;
        public Animator rigController;
        Shader isAliveShader;
        Shader dyingShader;
        Shader swordParticles;

        public GameObject dissolveModel;
        public List<Material> dissolveMaterials;
        public float duration = 2f;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void step()
        {
            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);
        }

        private void Jump()
        {
            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);
        }
        private AudioClip GetRandomClip()
        {
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }

        void Start()
        {
            Renderer[] rends = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                dissolveMaterials.Add(rend.material);
                Debug.LogWarning("found renderers");

            }
            isAliveShader = Shader.Find("Universal Render Pipeline/Lit");
            dyingShader = Shader.Find("Shader Graphs/Dissolve");
            animScript = GetComponentInParent<AnimController>();
            animControl = GetComponentInParent<Animator>();
            if (animControl != null)
                Debug.LogWarning("animControl not null");
            isalive = true;
            shieldStatus = true;
        }


        // Update is called once per frame
        void Update()
        {
            if (weapon)
            {

            }
            else
            {
                //ShieldBockWalk.weight = 0;
                //anim.SetLayerWeight(1, 0.0f);
            }
            if (Input.GetKeyDown(ShieldWeaponUnsheath))
            {
                rigController.SetBool("WeaponUnsheath", true);
                Debug.LogWarning("<color = yellow> WeaponUnsheath is Successful !! </color>");
            }
        }
        public void StartDissolve()
        {
            StartCoroutine(Dissolve());
        }
        public void Undissolve()
        {
            StartCoroutine(UnDissolve());

        }

        IEnumerator Dissolve()
        {
            float dissolveValue = 0f;
            Debug.Log("Dissolve coroutine started");
            while (dissolveValue < 1f)
            {
                dissolveValue += Time.deltaTime / duration;
                dissolveValue = Mathf.Clamp01(dissolveValue);
                foreach (Material mat in dissolveMaterials)
                {
                    Debug.LogWarning("Material name: " + mat.name);
                    //yield return new WaitForSeconds(0.4f);
                    Destroy(wind);
                    Destroy(sparks);
                    Destroy(glow);
                    mat.shader = dyingShader;
                    animScript.dissolveEffectCompleted = true;
                    // mat.SetFloat("Vector1_66CEB364", dissolveValue);
                }

                yield return null;
            }
            yield return new WaitForSeconds(0.2f);
            if (animScript.switchingCameras == true)
            {
                animScript.MyHumanoid.SetActive(false);
            }

        }

        IEnumerator UnDissolve()
        {
            yield return new WaitForSeconds(0.75f);
            /* if (animScript.switchingCameras == false)
             {
                 animScript.MyHumanoid.SetActive(true);
             }*/
            Debug.LogWarning("undissolve reached");
            float dissolveValue = 0f;
            //  Debug.Log("Dissolve coroutine started");
            while (dissolveValue < 1f)
            {
                dissolveValue += Time.deltaTime / duration;
                dissolveValue = Mathf.Clamp01(dissolveValue);
                foreach (Material mat in dissolveMaterials)
                {
                    // Debug.LogWarning("Material name: " + mat.name);
                    yield return new WaitForSeconds(0.4f);

                    mat.shader = isAliveShader;

                    animScript.dissolveEffectCompleted = true;
                    // mat.SetFloat("Vector1_66CEB364", dissolveValue);
                }
                yield return null;
            }

        }

        public void OnTriggerEnter(Collider other)
        {
            /* if (other.gameObject.tag == "cannonball")
             {      
                 knightHealth--;

                 if (knightHealth>=1)
                 {    
                     animControl.SetBool("Damage", true);
                     Debug.LogWarning("Knight Damaged");
                 }

                 Debug.Log("knight Health :" + knightHealth);
                 Debug.LogWarning ("cannon ball connected to knight");

                 if (knightHealth<= 0)
                 {
                     animControl.SetBool("Death", true);
                     Debug.LogWarning ("shield destroy success");
                 }

                 Destroy (other.gameObject);
             }*/
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "CheckPoint")
            {
                animScript.respawnPoint = other.gameObject.transform.position;
                Debug.LogWarning(animScript.respawnPoint + "checkpoint reached ");
            }

        }

    }

}