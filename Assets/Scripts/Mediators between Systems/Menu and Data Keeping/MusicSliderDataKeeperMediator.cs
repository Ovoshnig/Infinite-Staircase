public class MusicSliderDataKeeperMediator : SliderDataKeeperMediator
{
    private readonly AudioSettings _audioSettings;

    public MusicSliderDataKeeperMediator(MusicSliderView musicSliderView,
        MusicVolumeKeeper musicVolumeKeeper, AudioSettings audioSettings)
        : base(musicSliderView, musicVolumeKeeper) => _audioSettings = audioSettings;

    protected override float MinValue => _audioSettings.MinVolume;
    protected override float MaxValue => _audioSettings.MaxVolume;
}
