using System.ComponentModel;

namespace MauiApp2;
public class TaskViewModel
{
    private string _text;
    private bool _isDone;

    public string Text { 
        get => _text;
        set 
        { 
            if (_text != value)
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        } 
    }

    public bool IsDone
    {
        get => _isDone;
        set
        {
            if (_isDone != value)
            {
                _isDone = value;
                OnPropertyChanged(nameof(IsDone));
            }
        }
    }

    public TaskViewModel(string text, bool isDone) 
    {
        _text = text;
        _isDone = isDone;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }



}