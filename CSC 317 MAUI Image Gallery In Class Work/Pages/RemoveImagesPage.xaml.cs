namespace CSC_317_MAUI_Image_Gallery_In_Class_Work.Pages;

public partial class RemoveImagesPage : ContentPage
{
	MainPage page;

	public RemoveImagesPage(MainPage page)
	{
		InitializeComponent();
		layoutRemove.ItemsSource = App.galleryImages;

		this.page = page;
	}

	private void Finalize(object sender, EventArgs e)
	{
		
		//foreach(Thumbnail image in layoutRemove.SelectedItems)
		//{
		//	App.galleryImages.Remove(image);
		//}

		List<Thumbnail> itemsToRemove = layoutRemove.SelectedItems.Cast<Thumbnail>().ToList();

		foreach (Thumbnail item in itemsToRemove)
		{
			App.galleryImages.Remove(item);
		}

		if(itemsToRemove.Count > 0)
		{
			page.ChangedGallery = true;
		}

		Navigation.PopAsync();
	}
}