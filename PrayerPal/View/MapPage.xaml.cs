
namespace PrayerPal.View;

public partial class MapPage : ContentPage
{
    MapViewModel viewModel;

    public MapPage(MapViewModel viewModel)
	{
        InitializeComponent();
        this.viewModel = viewModel;
		BindingContext = viewModel;
        //Task.Run(async () =>{
        //    await viewModel.GetNearestMosquesOnStart();
        //}).Wait();
        Appearing += OnShellAppearing;
    }

    private async void OnShellAppearing(object sender, EventArgs e)
    {
        //Dispatcher.GetForCurrentThread().Dispatch(new Action(async () => { await viewModel.GetNearestMosquesOnStart(); }));

        await Dispatcher.DispatchAsync(new Action(async () => { await viewModel.GetNearestMosquesOnStart(); }));
        //await viewModel.GetNearestMosquesOnStart();
        Appearing -= OnShellAppearing;

    }

    protected override async void OnAppearing()
    {
        //try
        //{
        //    await viewModel.GetNearestMosques();

        //}
        //catch (Exception ex)
        //{

        //    Debug.WriteLine($"Error on appearing: {ex}");
        //}
    }

  

    
}