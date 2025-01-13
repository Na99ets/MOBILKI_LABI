using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;

namespace MauiApp2;

public class TodoListViewModel : INotifyPropertyChanged 
    {
        public ObservableCollection<TaskViewModel> Todolist { 
            get => _todoList;
        }

        public string NewTask
        {
            get => _newTask;
            set
            {
                if (_newTask != value)
                {
                    _newTask = value;
                    OnPropertyChanged(nameof(NewTask));
                }
            }
        }

        public ObservableCollection<TaskViewModel> _todoList;
        public string _newTask = "";

        public ICommand AddTodoCommand { get; }
        public ICommand DeleteTodoCommand { get; }

        public TodoListViewModel()
        {
            _todoList = LoadTasks();

            AddTodoCommand = new Command(
                () => 
                {
                    _todoList.Add(new TaskViewModel(NewTask, false));
                    SaveTasks();
                }
            );

            DeleteTodoCommand = new Command<TaskViewModel>(
                x =>
                {
                    _todoList.Remove(x);
                    SaveTasks();
                }
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<TaskViewModel> LoadTasks()
        {
            var tasksJson = Preferences.Get("Tasks", "");

            if (tasksJson == "")
                return new ObservableCollection<TaskViewModel>();

            return JsonSerializer.Deserialize<ObservableCollection<TaskViewModel>>(tasksJson) ?? new ObservableCollection<TaskViewModel>();
        }

        public void SaveTasks()
        {
            var tasksJson = JsonSerializer.Serialize(Todolist);
            Preferences.Set("Tasks", tasksJson);
        }
    }
        