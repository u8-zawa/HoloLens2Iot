using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authenticity : MonoBehaviour
{
    public GameObject angelStatue;

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

    public void OnTouchButton()
    {
        gsrBeforValue = GetGsrData();

        audioSource.PlayOneShot(sound1);

        Invoke("Judge", 10);
    }

    public void Judge()
    {
        ParticleSystem light = Instantiate(lightParticle);
        light.transform.position = angelStatue.transform.position;
        ParticleSystem bomb = Instantiate(bombParticle);
        bomb.transform.position = angelStatue.transform.position;

        audioSource.Stop();
        gsrAfterValue = GetGsrData();

        gsrValue = gsrBeforValue - gsrAfterValue;
        Debug.Log(gsrBeforValue);
        Debug.Log(gsrAfterValue);
        //gsrValue > 0.04で成功

        //if分岐は仮
        int i = 1;
        //成功パターン
        if (i == 0)
        {
            audioSource.PlayOneShot(sound2);
            text.gameObject.GetComponent<TextMesh>().text = "HONEST";
            light.Play();
        }

        //失敗パターン
        else if (i == 1)
        {
            audioSource.PlayOneShot(sound3);
            Destroy(angelStatue);
            Instantiate(demonStatue, new Vector3(angelStatue.transform.position.x, 
                angelStatue.transform.position.y + 0.30F, angelStatue.transform.position.z + 0.1F), 
                Quaternion.Euler(angelStatue.transform.rotation.x, angelStatue.transform.rotation.y - 180,
                angelStatue.transform.rotation.z));
            text.gameObject.GetComponent<TextMesh>().text = "LIAR";
            bomb.Play();
        }
    }

    //データ仮
    public float GetGsrData()
    {
        //SensorData sensorData = SensorDataManager.Instance.GetSensorData("");
        //if(sensorData != null)
        //{
        //    float gsrValue = sensorData.stat.latest;
        //    return gsrValue;
        //}
        //else
        //{
        //   return 0.0F;
        //}

        //仮
        return 0.0F;
    }
}
