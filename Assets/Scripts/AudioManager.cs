using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume;
    [Range(0.5f, 1.5f)]
    public float Pitch;
    [Range(0f, 0.5f)]
    public float RandomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float RandomPitch = 0.1f;
    public bool Loop = false;
    AudioSource _source;

    public void SetSource(AudioSource _source)
    {
        this._source = _source;
        this._source.clip = Clip;
        this._source.loop = Loop;
    }

    public void Play ()
    {
        _source.volume = Volume * (1 + Random.Range(-RandomVolume / 2f, RandomVolume / 2f));
        _source.pitch = Pitch * (1 + Random.Range(-RandomPitch / 2f, RandomPitch / 2f));
        _source.Play();
    }

    public void Stop()
    {
        _source.Stop();
    }
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    Sound[] sounds;

    private void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
            {
                 Destroy(this.gameObject);
            }
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("_sound" + i + "_" + sounds[i].Name);
            soundObject.transform.SetParent(this.transform);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
        }
    }

    public void PlaySound (string _name)
    {
        foreach (var t in sounds)
        {
            if (t.Name == _name)
            {
                t.Play();
                return;
            }
        }

        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

    public void StopSound(string _name)
    {
        foreach (var t in sounds)
        {
            if (t.Name == _name)
            {
                t.Stop();
                return;
            }
        }

        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }
}
