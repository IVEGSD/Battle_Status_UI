using UnityEngine;

public class AnimationControllerSupport : MonoBehaviour
{
    private bool isPlayingAnimation;

    private void Start()
    {
        isPlayingAnimation = false;
    }

    //動畫播放完畢後call
    public void Event_Finished()
    {
        isPlayingAnimation = false;
    }

    //動畫播放前call
    public void Event_Playing()
    {
        isPlayingAnimation = true;
    }

    //用來判定宜家是否有動畫正在播放
    public bool GetCurrentStatus()
    {
        return isPlayingAnimation;
    }
}