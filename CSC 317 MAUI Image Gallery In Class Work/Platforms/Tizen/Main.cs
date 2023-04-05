using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace CSC_317_MAUI_Image_Gallery_In_Class_Work;

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
}
