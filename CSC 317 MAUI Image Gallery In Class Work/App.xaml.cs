using System.Collections.ObjectModel;

namespace CSC_317_MAUI_Image_Gallery_In_Class_Work;

public partial class App : Application
{

	public static ObservableCollection<Thumbnail> galleryImages = new ObservableCollection<Thumbnail>();
	public static int index_selected;

	public static string GalleryPath = ""; 
	public static bool GalleryChanged = false;

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}


}
