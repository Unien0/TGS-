using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    public GMData_SO GameManagerData;
    private float BgmVolume
    {
        //BGM‚Ì‰¹—Ê
        get { if (GameManagerData != null) return GameManagerData.BgmVolume; else return 0; }
        set { GameManagerData.BgmVolume = value; }
    }
    private float SeVolume
    {
        //SE‚Ì‰¹—Ê
        get { if (GameManagerData != null) return GameManagerData.SeVolume; else return 0; }
        set { GameManagerData.SeVolume = value; }
    }


    public Slider BGMSlider;
    public Slider SESlider;
    public AudioSource BGMSource;
    public AudioSource SESource;

    private void Start()
    {
        //BGMSlider.value = BgmVolume;
        //SESlider.value = SeVolume;
    }

    public void BGMVolume()
    {
        BGMSource.volume = BGMSlider.value;
        BgmVolume = BGMSource.volume;
    }
    public void SEVolume()
    {
        SESource.volume = SESlider.value;
        SeVolume = SESource.volume;
    }
}
