
namespace PrayerPal.View;

public partial class QiblahPage : ContentPage
{
	QiblahViewModel viewModel;
	public QiblahPage(QiblahViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel = viewModel;
		BindingContext = viewModel;


    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.TurnOnCompass();
		await viewModel.CalculateQiblaDirection();
    }

	protected override void OnDisappearing()
	{
		viewModel.TurnOffCompass();
	}

  //  private void Switch_Toggled(object sender, ToggledEventArgs e)
  //  {
		//sender.SwitchToggled();
  //  }
}