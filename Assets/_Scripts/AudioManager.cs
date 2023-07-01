using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

/// <summary>
/// Used to give a name to indices within the array of sound effect audio clips
/// </summary>
public enum Clip { Punch }

/// <summary>
/// Used to play audio for the game
/// </summary>
public class AudioManager: Singleton<AudioManager>
{
    #region Variables

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;

    [Tooltip("Holds the music audio clips that can be played. Should be indexed to match the scene index minus 1")]
    [SerializeField] private AudioClip[] musicClips;
    [Tooltip("Holds the audio clips that can be played. Should be indexed to match the Clip enum")]
    [SerializeField] private AudioClip[] effectClips;

    private float musicFadeTime = 0.5f;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    #endregion //end Unity Control Methods

    #region Static Driver Methods

    /// <summary>
    /// If the audio manager instance has been created, this will play the audio clip corresponding to the passed Clip
    /// </summary>
    /// <param name="_clipToPlay"></param>
    public static void TryPlayClip(Clip _clipToPlay)
    {
        // If the audio manager has been created...
        if(Instance != null)
        {
            // Call the instance method that will play the audio clip
            Instance.PlayClip(_clipToPlay);
        }
    }//end TryPlayClip

    /// <summary>
    /// If the audio manager instance has been created, this will play a music clip that matches the passed scene.
    /// It will fade the previous music clip out and fade the new one in.
    /// </summary>
    /// <param name="_activeScene"></param>
    public static void TryPlayMusic(Scenes _activeScene)
    {
        // If the audio manager has been created...
        if (Instance != null || _activeScene == Scenes.Persistent)
        {
            // Call the instance method that will play the music clip
            // The scenes enum includes the persistent scene at index 0, but that scene doesn't have an associated
            // music clip, so offset by 1
#pragma warning disable CS4014
            Instance.PlayMusic((int)_activeScene - 1);
        }
    }//end TryPlayMusic

    #endregion

    #region Instance Methods

    /// <summary>
    /// This will play the audio clip corresponding to the passed Clip
    /// </summary>
    /// <param name="_clipToPlay"></param>
    private void PlayClip(Clip _clipToPlay)
    {
        // Stop the audio source to cancel any previous audio clip
        effectSource.Stop();

        // Switch to the audio clip that matches the passed Clip
        effectSource.clip = effectClips[(int)_clipToPlay];

        // Play the new audio clip
        effectSource.Play();
    }//end PlayClip

    /// <summary>
    /// Used to play a music clip that matches the passed index. It will fade out any previous music and then fade in this music clip
    /// </summary>
    /// <param name="_musicIndex"></param>
    /// <returns></returns>
    private async Task PlayMusic(int _musicIndex)
    {
        // Store the current volume of the music
        float _startVolume = musicSource.volume;

        // Fade out the music
        while (musicSource.volume > 0)
        {
            musicSource.volume -= _startVolume * Time.deltaTime / musicFadeTime;

            await Task.Yield();
        }

        // Stop the music source
        musicSource.Stop();

        // Switch the music source's audio clip
        musicSource.clip = musicClips[_musicIndex];

        // Play the new music clip
        musicSource.Play();

        // Fade the music back to the starting volume
        while (musicSource.volume < _startVolume)
        {
            musicSource.volume += _startVolume * Time.deltaTime / musicFadeTime;

            await Task.Yield();
        }
    }//end PlayMusic

    #endregion
}