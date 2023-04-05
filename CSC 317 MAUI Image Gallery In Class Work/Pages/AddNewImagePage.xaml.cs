using System.Collections.ObjectModel;

namespace CSC_317_MAUI_Image_Gallery_In_Class_Work.Pages;

public partial class AddNewImagePage : ContentPage
{
	PickOptions options;
	MainPage page;

	ObservableCollection<Thumbnail> thumbnails = new ObservableCollection<Thumbnail>();

	public AddNewImagePage(MainPage page)
	{
		InitializeComponent();
		var customFileType = new FilePickerFileType(
				new Dictionary<DevicePlatform, IEnumerable<string>>
				{
					{ DevicePlatform.iOS, new[] { "public.my.image.extension" } }, // UTType values
					{ DevicePlatform.Android, new[] { "application/image" } }, // MIME type
					{ DevicePlatform.WinUI, new[] { ".png", ".bmp", ".jpeg", ".jpg", ".gif" } }, // file extension
					{ DevicePlatform.Tizen, new[] { "*/*" } },
					{ DevicePlatform.macOS, new[] { "png", "bmp", "jpeg", "jpg", "gif" } }, // UTType values
				});
		//This sets up the options for the FilePicker used by this app.
		options = new PickOptions();
		options.PickerTitle = "Select your Images";
		options.FileTypes = customFileType;
		this.page = page;

		layoutAdd.ItemsSource = thumbnails;
	}

	private async void FinalizeSource(object sender, EventArgs e)
	{
		if(thumbnails.Count > 0)
		{
			//At least 1 image was added. 
			foreach (var item in thumbnails)
				App.galleryImages.Add(item);

			await DisplayAlert("Success", "Successfully Added the specified images.", "Ok");
			await Navigation.PopAsync();
		}
		else
		{
			await DisplayAlert("No Images Selected", "You did not select any images.  Select at least 1 image or press Cancel.", "Ok");
		}


		//if(nameSpecified && imageSpecified)
		//{
		//    //Save Image to Gallery
		//    Thumbnail image = new Thumbnail();
		//    image.ImageName = txtImageName.Text;
		//    image.ImageURL = txtSource.Text;

		//    App.galleryImages.Add(image);
		//    page.ChangedGallery = true;
		//    Navigation.PopAsync();
		//}
		//else
		//{
		//    //Highlight what isn't filled in.
		//    if (nameSpecified == false)
		//        txtImageName.BackgroundColor = Color.Parse("#442222");
		//    else
		//        txtImageName.BackgroundColor = originalEntryColor;

		//    //Highlight what isn't filled in.
		//    if (imageSpecified == false)
		//        txtSource.BackgroundColor = Color.Parse("#442222");
		//    else
		//        txtSource.BackgroundColor = originalEntryColor;
		//}
	}

	private async void SelectImagesFromFile(object sender, EventArgs e)
	{
		try
		{
			var result = await FilePicker.Default.PickMultipleAsync(options);
			if (result != null)
			{
				IEnumerable<FileResult> files = result.ToList();

				foreach (var file in files)
				{
					Thumbnail image = new Thumbnail();

					image.ImageURL = file.FullPath;
					image.ImageName = file.FileName;
					
					thumbnails.Add(image);
				}

			}

		}
		catch (Exception ex)
		{
			// The user canceled or something went wrong
			await DisplayAlert("Error", ex.Message, "Ok");
		}
	}

	private void RemoveSelected(object sender, EventArgs e)
	{
		List<Thumbnail> itemsToRemove = layoutAdd.SelectedItems.Cast<Thumbnail>().ToList();

		foreach (Thumbnail item in itemsToRemove)
		{
			thumbnails.Remove(item);
		}
	}

	private void Cancel(object sender, EventArgs e)
	{
		Navigation.PopAsync();
	}

	private async void SelectImagesFromURI(object sender, EventArgs e)
	{
		string result = await DisplayPromptAsync("Enter URL", "Enter the URL address of the image you want to add.");

		if (result != null)
		{

			ImageSource source;
			try
			{
				source = ImageSource.FromUri(new Uri(result));
			}
			catch(UriFormatException)
			{
				await DisplayAlert("Invalid URL", "This is not a valid URL.", "Ok");
				return;
			}

			Thumbnail image = new Thumbnail();
			image.ImageName = Path.GetFileName(result);
			image.ImageURL = result;

			thumbnails.Add(image);	
		}


	}
}