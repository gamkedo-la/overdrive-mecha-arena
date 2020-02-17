using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioOcclusion : MonoBehaviour
{
    [Header("FMOD Event")]
    [EventRef]
    [SerializeField] private string SelectAudio;
    FMOD.Studio.EventInstance Audio;
    FMOD.Studio.PARAMETER_ID VolumeParameterID;
    FMOD.Studio.PARAMETER_ID LPFParameterID;

    Transform SlLocation;
    Transform emitterPosition;
    [Header("Occlusion Options")]
    [Range(0f, 1f)]
    [SerializeField] private float VolumeValue = 0.5f;
    [Range(10f, 22000f)]
    [SerializeField] private float LPFCutoff = 10000f;
    [SerializeField] private LayerMask OcclusionLayer;

    Vector3 elevateYMecha;
    Vector3 elevateYTesla;
    //GameObject Player;
    // Start is called before the first frame update
    void Awake()
    {
        //Find StudioListener.
        //Instantiate Event
        Audio = RuntimeManager.CreateInstance(SelectAudio);
        FMOD.Studio.EventDescription AudioEventDescription;
        Audio.getDescription(out AudioEventDescription);
        // Get parameter Volume
        FMOD.Studio.PARAMETER_DESCRIPTION VolumeParameterDescription;
        AudioEventDescription.getParameterDescriptionByName("Volume", out VolumeParameterDescription);
        VolumeParameterID = VolumeParameterDescription.id;
        //Get parameter LPF
        FMOD.Studio.PARAMETER_DESCRIPTION LPFParameterDescription;
        AudioEventDescription.getParameterDescriptionByName("LPF", out LPFParameterDescription);
        LPFParameterID = LPFParameterDescription.id;

        //Player = GameObject.Find("Player");
    }

    private void Start()
    {
        SlLocation = FindObjectOfType<StudioListener>().transform;
  

        elevateYMecha = new Vector3(0, 20, 0);
        elevateYTesla = new Vector3(0, 15, 0);
        FMOD.Studio.PLAYBACK_STATE pbState;
        Audio.getPlaybackState(out pbState);
        if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            Audio.start();
        }

        //points
        
    }

    // Update is called once per frame
    void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(Audio, gameObject.transform, GetComponent<Rigidbody>());

        RaycastHit hit;

        Physics.Linecast(gameObject.transform.position + elevateYTesla, SlLocation.position + elevateYMecha, out hit, OcclusionLayer);

        if (hit.collider != null && hit.collider.tag == "Player")
        {
            NotOccluded();
            Debug.DrawLine(gameObject.transform.position + elevateYTesla, SlLocation.position + elevateYMecha, Color.magenta);
        }
        else
        {
            Occluded();
            Debug.DrawLine(gameObject.transform.position + elevateYTesla, hit.point, Color.white);
        }
    }

    void Occluded()
    {
        Audio.setParameterByID(VolumeParameterID, VolumeValue);
        Audio.setParameterByID(LPFParameterID, LPFCutoff);
    }

    void NotOccluded()
    {
        Audio.setParameterByID(VolumeParameterID, .75f);
        Audio.setParameterByID(LPFParameterID, 22000f);
    }

}
