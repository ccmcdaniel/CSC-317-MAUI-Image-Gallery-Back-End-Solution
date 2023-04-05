using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace CSC_317_MAUI_Image_Gallery_In_Class_Work;

public partial class AppShell : Shell
{

	public AppShell()
	{
		InitializeComponent();
	}

	private void AddNewImage(object sender, EventArgs e)
	{
		MainPage page = this.CurrentPage as MainPage;

		Navigation.PushAsync(new Pages.AddNewImagePage(page));
	}

	private async void RemoveImages(object sender, EventArgs e)
	{
		MainPage page = this.CurrentPage as MainPage;

		if (App.galleryImages.Count > 0) 
			await Navigation.PushAsync(new Pages.RemoveImagesPage(page));
		else
			await DisplayAlert("No Images", "There are no images to remove.", "Ok");
	}

	private async void ClearGallery(object sender, EventArgs e)
	{
		bool answer = await DisplayAlert("Are you sure", "This operation will remove ALL pictures from the gallery.", "Yes", "No");
		
		if(answer == true)
		{
			App.galleryImages.Clear();
		}
	}

	private void SaveGallery(object sender, EventArgs e)
	{
		SaveGallery();
	}

	private void SaveGallery()	
	{
		MainPage page = this.CurrentPage as MainPage;

		if (App.GalleryPath == "")
		{
			Navigation.PushAsync(new Pages.SaveGallery(page));
		}
		else
		{
			StreamWriter file = new StreamWriter(App.GalleryPath);
			string json = JsonSerializer.Serialize(App.galleryImages);
			
			App.GalleryChanged = false;
			file.Write(json);
			file.Close();
		}
	}

	private void SaveGalleryAs(object sender, EventArgs e)
	{
		MainPage page = this.CurrentPage as MainPage;

		Navigation.PushAsync(new Pages.SaveGallery(page));	
	}

	private async void CreateNewGallery(object sender, EventArgs e)
	{
		MainPage page = this.CurrentPage as MainPage;

		if (page.ChangedGallery)
		{
			bool result = await DisplayAlert("Unsaved Changes", "Warning: Changes have not been saved.  Would you like to save?", "Yes", "No");

			if(result == true)
			{
				SaveGallery();
				return;
			}
		}
		page.GalleryPath = "";
		page.ChangedGallery = false;
		App.galleryImages.Clear();

	}

	private async void ExitProgram(object sender, EventArgs e)
	{
		MainPage page = this.CurrentPage as MainPage;
		if (page.ChangedGallery == true)
		{
			bool result = await DisplayAlert("Unsaved Changes", "Warning: Changes have not been saved.  Would you like to save?", "Yes", "No");

			if (result == true)
			{
				SaveGallery();
				return;
			}
		}
		App.Current.Quit();
	}

	private async void OpenGallery(object sender, EventArgs e)
	{
		MainPage page = this.CurrentPage as MainPage;
		if (page.ChangedGallery == true)
		{
			bool save_result = await DisplayAlert("Unsaved Changes", "Warning: Changes have not been saved.  Would you like to save?", "Yes", "No");

			if (save_result == true)
			{
				SaveGallery();
				return;
			}
		}

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
		PickOptions options = new PickOptions();
		options.PickerTitle = "Select your Images";
		options.FileTypes = customFileType;

		var result = await FilePicker.PickAsync(options);

		if(result != null)
		{
			StreamReader file = new StreamReader(result.FullPath);
			string json = file.ReadToEnd();

			ObservableCollection<Thumbnail> loaded_gallery = JsonSerializer.Deserialize<ObservableCollection<Thumbnail>>(json);

			App.galleryImages.Clear();

			foreach (var image in loaded_gallery)
				App.galleryImages.Add(image);

			page.ChangedGallery = false;
			page.GalleryPath = result.FullPath;
			file.Close();
		}
	}
}
