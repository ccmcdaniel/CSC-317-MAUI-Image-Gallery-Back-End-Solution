using System.Collections.ObjectModel;

namespace CSC_317_MAUI_Image_Gallery_In_Class_Work;

public partial class MainPage : ContentPage
{
	
	public string GalleryPath
	{
		get
		{
			return lblGalleyPath.Text;
		}
		set
		{
			lblGalleyPath.Text = value;
		}
	}
	public bool ChangedGallery { get; set; }

	public MainPage()
	{
		InitializeComponent();

		App.index_selected = 0;

		imgDisplay.Source = "empty_image.png";
		lblDisplay.Text = "No Image Selected";

		thumbnailHolder.ItemsSource = App.galleryImages;

	}

	private void PreviousImage(object sender, EventArgs e)
	{
		if (App.galleryImages.Count == 0)
			return;

		App.index_selected--;

		if(App.index_selected == -1)
			App.index_selected = App.galleryImages.Count - 1;

		thumbnailHolder.SelectedItem = App.galleryImages[App.index_selected];
		
	}

	private void NextImage(object sender, EventArgs e)
	{
		if (App.galleryImages.Count == 0)
			return;

		App.index_selected = (App.index_selected + 1) % App.galleryImages.Count;
		thumbnailHolder.SelectedItem = App.galleryImages[App.index_selected];

	}

	private void LoadNewImage(object sender, SelectionChangedEventArgs e)
	{
	
		if(e.CurrentSelection.Count == 0)
		{
			imgDisplay.Source = "empty_image.png";
			lblDisplay.Text = "No Image Selected";
			return;
		}	

		Thumbnail selected = (e.CurrentSelection[0] as Thumbnail);

		if (App.galleryImages.Contains(selected))
		{
			imgDisplay.Source = selected.ImageURL;
			lblDisplay.Text = selected.ImageName;
		}
		else
		{
			imgDisplay.Source = "empty_image.png";
			lblDisplay.Text = "No Image Selected";
		}
	}

	private void ResetImage(object sender, ElementEventArgs e)
	{
		if(App.galleryImages.Count == 0)
			imgDisplay.Source = "empty_image.png";
	}
}

