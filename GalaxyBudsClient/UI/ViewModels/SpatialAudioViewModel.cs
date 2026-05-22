using System.ComponentModel;
using GalaxyBudsClient.Bluetooth.Devices.Features;
using GalaxyBudsClient.Models;

namespace GalaxyBudsClient.UI.ViewModels;

public class SpatialAudioViewModel : INotifyPropertyChanged
{
    private readonly SpatialAudioFeature _feature;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsSpatialAudioEnabled
    {
        get => _feature.IsEnabled;
        set
        {
            _feature.IsEnabled = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSpatialAudioEnabled)));
        }
    }

    public SpatialQuaternion CurrentOrientation => _feature.Engine.GetCurrentOrientation();

    public SpatialAudioViewModel(SpatialAudioFeature feature)
    {
        _feature = feature;
        _feature.Engine.HeadOrientationChanged += OnHeadOrientationChanged;
    }

    private void OnHeadOrientationChanged(object? sender, SpatialQuaternion e)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentOrientation)));
    }
}