namespace MauiApp2;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnDoneChanged(object sender, CheckedChangedEventArgs e)
        {
            if (BindingContext is TodoListViewModel todolist)
                {
                    todolist.SaveTasks();
                }
        }

}

