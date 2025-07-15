using R3;

public class GamePauserAudioMixerTunerMediator : Mediator
{
    private readonly GamePauser _gamePauser;
    private readonly AudioMixerTuner _audioMixerTuner;

    public GamePauserAudioMixerTunerMediator(GamePauser gamePauser, AudioMixerTuner audioMixerTuner)
    {
        _gamePauser = gamePauser;
        _audioMixerTuner = audioMixerTuner;
    }

    public override void Initialize()
    {
        _gamePauser.IsPause
            .Subscribe(_audioMixerTuner.SetPause)
            .AddTo(CompositeDisposable);
    }
}
