using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();

			this.Loaded += MainPage_Loaded;
		}

		private void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			Storyboard1.Begin();

			Perspective(Persp, 5000);
		}

		private void Perspective(FrameworkElement element, double duration, float rotationDepth = 750f)
		{
			var parent = ElementCompositionPreview.GetElementVisual(element.Parent as FrameworkElement);

			var width = (float)element.ActualWidth;
			var height = (float)element.ActualHeight;
			var halfWidth = (float)(width / 2.0);
			var halfHeight = (float)(height / 2.0);

			// Initialize the Compositor
			var visual = ElementCompositionPreview.GetElementVisual(element);

			// Create Scoped batch for animations
			var batch = visual.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);




			// THIS IS THE OLD CODE THAT WORKS
			// ===============================
			//// Rotation animation
			//var projectionMatrix = new Matrix4x4(1, 0, 0, 0,
			//									 0, 1, 0, 0,
			//									 0, 0, 1, 1 / rotationDepth,
			//									 0, 0, 0, 1);
			//// To ensure that the rotation occurs through the center of the visual rather than the
			//// left edge, pre-multiply the rotation matrix with a translation that logically shifts
			//// the axis to the point of rotation, then restore the original location
			//parent.TransformMatrix = Matrix4x4.CreateTranslation(-halfWidth, -halfHeight, 0) *
			//						projectionMatrix *
			//						Matrix4x4.CreateTranslation(halfWidth, halfHeight, 0);
			// ===============================



			// THIS IS THE CODE I'M TRYING TO GET TO WORK
			// ==========================================
			var z = -(float)Math.Pow(100, 2);

			parent.Offset = new Vector3((float)Window.Current.Bounds.Width / 2, (float)Window.Current.Bounds.Height / 2, 0);

			parent.TransformMatrix = Matrix4x4.CreateLookAt(new Vector3(0, 0, z), new Vector3(0, 0, 0), new Vector3(0, 1, 0))
					* Matrix4x4.CreatePerspectiveFieldOfView(0.001f, 1, 1, 2);
			// ==========================================




			visual.RotationAxis = new Vector3(1, 0, 0);
			visual.CenterPoint = new Vector3(halfWidth, halfHeight, 0f);
			visual.RotationAngleInDegrees = 0.0f;

			var rotateAnimation = visual.Compositor.CreateScalarKeyFrameAnimation();

			rotateAnimation.InsertKeyFrame(0.0f, 90);
			rotateAnimation.InsertKeyFrame(1f, 0);
			rotateAnimation.Duration = TimeSpan.FromMilliseconds(duration);			

			visual.StartAnimation("RotationAngleInDegrees", rotateAnimation);

			// Batch is ended an no objects can be added
			batch.End();
		}
	}
}
