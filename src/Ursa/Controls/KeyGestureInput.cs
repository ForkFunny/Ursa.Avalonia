using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;

namespace Ursa.Controls;

public class KeyGestureInput: TemplatedControl
{
    static KeyGestureInput()
    {
        InputElement.FocusableProperty.OverrideDefaultValue<KeyGestureInput>(true);
    }
    
    public static readonly StyledProperty<KeyGesture> GestureProperty = AvaloniaProperty.Register<KeyGestureInput, KeyGesture>(
        nameof(Gesture));

    public KeyGesture Gesture
    {
        get => GetValue(GestureProperty);
        set => SetValue(GestureProperty, value);
    }

    public static readonly StyledProperty<IList<Key>?> AcceptableKeysProperty = AvaloniaProperty.Register<KeyGestureInput, IList<Key>?>(
        nameof(AcceptableKeys));

    public IList<Key>? AcceptableKeys
    {
        get => GetValue(AcceptableKeysProperty);
        set => SetValue(AcceptableKeysProperty, value);
    }

    public static readonly StyledProperty<bool> ConsiderKeyModifiersProperty = AvaloniaProperty.Register<KeyGestureInput, bool>(
        nameof(ConsiderKeyModifiers), true);

    public bool ConsiderKeyModifiers
    {
        get => GetValue(ConsiderKeyModifiersProperty);
        set => SetValue(ConsiderKeyModifiersProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        ContentControl.HorizontalContentAlignmentProperty.AddOwner<KeyGestureInput>(
            new StyledPropertyMetadata<HorizontalAlignment>(HorizontalAlignment.Center));

    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
        ContentControl.VerticalContentAlignmentProperty.AddOwner<KeyGestureInput>(
            new StyledPropertyMetadata<VerticalAlignment>(VerticalAlignment.Center));

    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    

    protected override void OnKeyDown(KeyEventArgs e)
    {
        // base.OnKeyDown(e);
        if (AcceptableKeys is not null && !AcceptableKeys.Contains(e.Key))
        {
            return;
        }

        if (!ConsiderKeyModifiers)
        {
            if(e.Key is Key.LeftCtrl or Key.RightCtrl or Key.LeftAlt or Key.RightAlt or Key.LeftShift or Key.RightShift or Key.LWin or Key.RWin)
            {
                return;
            }
            Gesture = new KeyGesture(e.Key);
        }
        KeyGesture gesture;
        switch (e.KeyModifiers)
        {
            case KeyModifiers.Control when e.Key is Key.LeftCtrl or Key.RightCtrl:
            case KeyModifiers.Alt when e.Key is Key.LeftAlt or Key.RightAlt:
            case KeyModifiers.Shift when e.Key is Key.LeftShift or Key.RightShift:
            case KeyModifiers.Meta when e.Key is Key.LWin or Key.RWin:
                gesture = new KeyGesture(e.Key);
                break;
            default:
                gesture = new KeyGesture(e.Key, e.KeyModifiers);
                break;
        }
        Gesture = gesture;
        e.Handled = true;
    }
}