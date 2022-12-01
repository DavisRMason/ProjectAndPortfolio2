using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyLonginousSpear : EmptyHand
{
    #region UnityVariables

    [Header("----- Audio -----")]
    [SerializeField] List<AudioClip> rButtonClips = new List<AudioClip>();
    [Range(0, 1)][SerializeField] float rButtonClipsVolume;
    [SerializeField] List<AudioClip> rightClickClips = new List<AudioClip>();
    [Range(0, 1)][SerializeField] float rightClickClipsVolume;

    #endregion

    public override void rButtonFunction()
    {
        gameManager.instance.playerScript.aud.PlayOneShot(rButtonClips[Random.Range(0, rButtonClips.Count)], rButtonClipsVolume);
    }

    public override void RightClick()
    {
        gameManager.instance.playerScript.aud.PlayOneShot(rightClickClips[Random.Range(0, rButtonClips.Count)], rightClickClipsVolume);
    }
}
