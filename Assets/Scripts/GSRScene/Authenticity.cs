using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authenticity : MonoBehaviour
{
    public GameObject angelStatue;
    private GameObject _demonStatueObject;
    private ParticleSystem _lightEffectObject;
    private ParticleSystem _bombEffectObject;

    public ParticleSystem lightParticle;
    public ParticleSystem bombParticle;

    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    public AudioSource audioSource;

    [SerializeField]
    [Tooltip("createGameObject")]
    public GameObject demonStatue;

    public GameObject text;

    private float gsrBeforValue = 0.0F;
    private float gsrAfterValue = 0.0F;
    private float gsrValue = 0.0F;

    [SerializeField] private float voltLimit = 0.01f;

    public void OnTouchButton()
    {
        angelStatue.SetActive(true);
        if (_demonStatueObject != null) Destroy(_demonStatueObject);
        if (_lightEffectObject != null) Destroy(_lightEffectObject);
        if (_bombEffectObject != null) Destroy(_bombEffectObject);

        gsrBeforValue = GetGsrLatest();

        audioSource.PlayOneShot(sound1);

        Invoke("Judge", 10);
    }

    public void Judge()
    {
        _lightEffectObject = Instantiate(lightParticle);
        _lightEffectObject.transform.position = angelStatue.transform.position;
        _bombEffectObject = Instantiate(bombParticle);
        _bombEffectObject.transform.position = angelStatue.transform.position;

        audioSource.Stop();
        gsrAfterValue = GetGsrLatest();

        gsrValue = gsrBeforValue - gsrAfterValue;
        Debug.Log(gsrBeforValue);
        Debug.Log(gsrAfterValue);
        //gsrValue > 0.04�Ő���

        //if����͉�
        bool i = CompareMaxAndLatest();
        //�����p�^�[��
        if (!i)
        {
            audioSource.PlayOneShot(sound2);
            text.gameObject.GetComponent<TextMesh>().text = "HONEST";
            _lightEffectObject.Play();
        }

        //���s�p�^�[��
        else if (i)
        {
            audioSource.PlayOneShot(sound3);
            //Destroy(angelStatue);
            angelStatue.SetActive(false);
            _demonStatueObject = Instantiate(demonStatue, new Vector3(angelStatue.transform.position.x, 
                angelStatue.transform.position.y + 0.30F, angelStatue.transform.position.z + 0.1F), 
                Quaternion.Euler(angelStatue.transform.rotation.x, angelStatue.transform.rotation.y - 180,
                angelStatue.transform.rotation.z));
            text.gameObject.GetComponent<TextMesh>().text = "LIAR";
            _bombEffectObject.Play();
        }
    }

    //�f�[�^��
    public SensorData GetGsrData()
    {
        SensorData sensorData = SensorDataManager.Instance.GetSensorData("gsr");
        if(sensorData != null)
        {
            return sensorData;
        }
        else
        {
           return null;
        }

        //��
        //return 0.0F;

    }

    public float GetGsrLatest()
    {
        var data = GetGsrData();
        return data != null ? data.Stat.Latest : 0f;
    }

    public bool CompareMaxAndLatest()
    {
        SensorData gsrData = GetGsrData();
        if (gsrData == null) return false;
        return gsrData.Stat.Max - gsrData.Stat.Min > voltLimit;
    }
}
