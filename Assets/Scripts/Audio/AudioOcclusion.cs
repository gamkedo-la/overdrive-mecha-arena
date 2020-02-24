using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioOcclusion : MonoBehaviour
{
    [Header("FMOD Event")]
    [EventRef]
    [SerializeField]
    private string SelectAudio;
    public EventInstance audio;
    private EventDescription audioDes;
    private StudioListener listener;
    private PLAYBACK_STATE pbState;

    [Header("Occlusion Options")]

    [SerializeField] private LayerMask OcclusionLayer;

    Vector3 elevateYMecha;
    Vector3 elevateYTesla;

    private bool audioIsVirtual;
    private float maxDistance;
    private float listenerDistance;

    //GameObject Player;
    // Start is called before the first frame update


    private void Start()
    {
        audio = RuntimeManager.CreateInstance(SelectAudio);
        audio.start();
        audio.release();

        elevateYMecha = new Vector3(0, 20, 0);

        audioDes = RuntimeManager.GetEventDescription(SelectAudio);
        audioDes.getMaximumDistance(out maxDistance);

        listener = FindObjectOfType<StudioListener>();
           
        audio.getPlaybackState(out pbState);
        if (pbState != PLAYBACK_STATE.PLAYING)
        {
            audio.start();
        }

        //points
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RuntimeManager.AttachInstanceToGameObject(audio, gameObject.transform, GetComponent<Rigidbody>());

        audio.isVirtual(out audioIsVirtual);
        audio.getPlaybackState(out pbState);

        listenerDistance = Vector3.Distance(transform.position, listener.transform.position);

        RaycastHit hit;


        if (!audioIsVirtual && pbState == PLAYBACK_STATE.PLAYING && listenerDistance <= maxDistance)
        {
            Physics.Linecast(gameObject.transform.position, listener.transform.position + elevateYMecha, out hit, OcclusionLayer);

            Debug.Log("Check occlusion");
            if (hit.collider != null && hit.collider.tag == "Player")
            {
                NotOccluded();
                Debug.Log("not occluded");
                Debug.DrawLine(gameObject.transform.position, listener.transform.position + elevateYMecha, Color.green);
            }
            else
            {
                Occluded();
                Debug.Log("occluded");
                Debug.DrawLine(gameObject.transform.position, hit.point, Color.magenta);
            }
        }
    }

    void Occluded()
    {
        audio.setParameterByName("Occlusion", 1f);
    }

    void NotOccluded()
    {
        audio.setParameterByName("Occlusion", 0f);
    }

}
