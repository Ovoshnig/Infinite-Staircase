public class SoundSliderDataKeeperMediator : SliderDataKeeperMediator
{
    private readonly AudioSettings _audioSettings;

    public SoundSliderDataKeeperMediator(SoundSliderView soundSliderView,
        SoundVolumeKeeper soundVolumeKeeper, AudioSettings audioSettings)
        : base(soundSliderView, soundVolumeKeeper) => _audioSettings = audioSettings;

    protected override float MinValue => _audioSettings.MinVolume;
    protected override float MaxValue => _audioSettings.MaxVolume;
}
