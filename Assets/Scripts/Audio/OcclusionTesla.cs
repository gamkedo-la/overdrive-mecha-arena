using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class OcclusionTesla : MonoBehaviour
{
    [Header("FMOD Event")]
    [EventRef]
    public string SelectAudio;
    FMOD.Studio.EventInstance Audio;
    FMOD.Studio.PARAMETER_ID VolumeParameterID;
    FMOD.Studio.PARAMETER_ID LPFParameterID;

    Transform SlLocation;

    [Header("Occlusion Options")]
    [Range(0f, 1f)]
    public float VolumeValue = 0.5f;
    [Range(10f, 22000f)]
    public float LPFCutoff = 10000f;
    public LayerMask OcclusionLayer = 1;

    //GameObject Player;
    // Start is called before the first frame update
    void Awake()
    {
        //Find StudioListener.
        SlLocation = FindObjectOfType<StudioListener>().transform;
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
        FMOD.Studio.PLAYBACK_STATE pbState;
        Audio.getPlaybackState(out pbState);
        if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            Audio.start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(Audio, GetComponent<Transform>(), GetComponent<Rigidbody>());

        RaycastHit hit;

        Physics.Linecast(gameObject.transform.position, SlLocation.position, out hit, OcclusionLayer);

        if (hit.collider != null  && hit.collider.tag == "Player")
        {
            NotOccluded();
            Debug.DrawLine(transform.position, SlLocation.position, Color.magenta);
        }
        else
        {
            Occluded();
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
    }

    void Occluded()
    {
        Audio.setParameterByID(VolumeParameterID, VolumeValue);
        Audio.setParameterByID(LPFParameterID, LPFCutoff);
    }

    void NotOccluded()
    {
        Audio.setParameterByID(VolumeParameterID, 1f);
        Audio.setParameterByID(LPFParameterID, 22000f);
    }

}
 