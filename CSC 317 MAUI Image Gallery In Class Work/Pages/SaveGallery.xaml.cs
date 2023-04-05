using Microsoft.Maui.Storage;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using System.Text.Json;

namespace CSC_317_MAUI_Image_Gallery_In_Class_Work.Pages;

public partial class SaveGallery : ContentPage
{
	PickOptions options;
	string path = "";
	MainPage page;

	public SaveGallery(MainPage page)
	{
		InitializeComponent();
		this.page = page;

		var customFileType = new FilePickerFileType(
				new Dictionary<DevicePlatform, IEnumerable<string>>
				{
					{ DevicePlatform.iOS, new[] { "public.my.gallery.extension" } }, // UTType values
					{ DevicePlatform.Android, new[] { "application/gallery" } }, // MIME type
					{ DevicePlatform.WinUI, new[] { ".gly" } }, // file extension
					{ DevicePlatform.Tizen, new[] { "*/*" } },
					{ DevicePlatform.macOS, new[] { "gly" } }, // UTType values
				});
		//This sets up the options for the FilePicker used by this app.
		options = new PickOptions();
		options.PickerTitle = "Select your File";
		options.FileTypes = customFileType;
		this.page = page;
	}

	private async void OpenFile(object sender, EventArgs e)
	{
		try
		{
			CancellationToken t = new CancellationToken();

			var result = await FolderPicker.Default.PickAsync(t);

			if(result.IsSuccessful)
			{
				path = result.Folder.Path;
				lblPath.Text = path;
			}
		}
		catch (Exception ex)
		{
			string errormessage = "An Error Occurred while attempting to open the file.";
			errormessage += "\n\nDetails: ";
			errormessage += $"\n{ex.Message}";
			await DisplayAlert("Error", errormessage, "OK");
		}
	}

	private async void CompleteSave(object sender, EventArgs e)
	{
		if(txtFileName.Text == null || txtFileName.Text.Length == 0)
		{
			txtFileName.BackgroundColor = Color.Parse("#331111");
			await DisplayAlert("Error", "Please specify a file name.", "Ok");
			return;
		}

		string fullPath;


		if (lblPath.Text == null || lblPath.Text.Length == 0)
		{
			lblPath.BackgroundColor = Color.Parse("#331111");
			await DisplayAlert("Error", "Please choose a location to save to.", "Ok");
			return;
		}

		try
		{
			string gallery = JsonSerializer.Serialize(App.galleryImages);


			fullPath = lblPath.Text + "\\" + txtFileName.Text + ".gly";
			StreamWriter file = new StreamWriter(fullPath);
			file.Write(gallery);
			file.Close();
		}
		catch(Exception)
		{

			await DisplayAlert("Save Failed", "Check the folder path or file name", "Ok");
			return;
		}

		await DisplayAlert("Succesful", "Your gallery was successfully saved!", "Ok");
		page.GalleryPath = fullPath;
		page.ChangedGallery = false;
		await Navigation.PopAsync();
	}
}