using System.Diagnostics;
using ToDoMauiClient.DataServices;

namespace ToDoMauiClient;

public partial class MainPage : ContentPage
{
    private readonly IRestDataService _dataservice;

    public MainPage(IRestDataService dataservice)
	{
		InitializeComponent();
		_dataservice = dataservice;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        collectionView.ItemsSource = await _dataservice.GetAllToDosAsync();  
    }

    async void OnAddToDoClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("-----> Add button clicked");
    }
    async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Debug.WriteLine("-----> Item changed clicked");
    }

}

