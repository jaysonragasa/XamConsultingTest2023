using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

using MPCMobile.Client.Interfaces;
using MPCMobile.Client.Models;
using MPCMobile.Helpers;
using MPCMobile.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;

namespace MPCMobile.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		#region vars
		#endregion

		#region services
		private IAreaService<AreaModel> _areaSvc;
		private ICategorySerice<CategoryModel> _catSvc;
		private IEventService<EventModel> _evtSvc;
		private IReqResInService _reqresinSvc;
		private IDisplayAlert _alert;
		#endregion

		#region properties
		private bool _isbusy = false;
		public bool IsBusy
		{
			get => _isbusy;
			set => SetProperty(ref _isbusy, value);
		}

		private string _location = string.Empty;
		public string Location
		{
			get => _location;
			set => SetProperty(ref _location, value);
		}

		private string _comments = null;
		public string Comments
		{
			get => _comments;
			set => SetProperty(ref _comments, value);
		}

		private DateTime _diaryDate = DateTime.Now;
		public DateTime DiaryDate
		{
			get => _diaryDate;
			set => SetProperty(ref _diaryDate, value);
		}

		public ObservableCollection<ImageUIModel> Images { get; set; } = new ObservableCollection<ImageUIModel>();
		public ObservableCollection<AreaModel> Areas { get; set; } = new ObservableCollection<AreaModel>();
		public ObservableCollection<CategoryModel> Categories { get; set; } = new ObservableCollection<CategoryModel>();

		private string _tags = null;
		public string Tags
		{
			get => _tags;
			set => SetProperty(ref _tags, value);
		}

		public ObservableCollection<EventModel> Events { get; set; } = new ObservableCollection<EventModel>();

		private AreaModel _selArea = null;
		public AreaModel SelectedArea
		{
			get => _selArea;
			set => SetProperty(ref _selArea, value);
		}

		private CategoryModel _selCategory = null;
		public CategoryModel SelectedCategory
		{
			get => _selCategory;
			set => SetProperty(ref _selCategory, value);
		}

		private EventModel _selEvent = null;
		public EventModel SelectedEvent
		{
			get => _selEvent;
			set => SetProperty(ref _selEvent, value);
		}

		private bool _linkExistingEvent = false;
		public bool LinkExistingEvent
		{
			get => _linkExistingEvent;
			set => SetProperty(ref _linkExistingEvent, value);
		}
		#endregion

		#region commands
		public IAsyncRelayCommand AddPhotoCommend { get; set; }
		public IRelayCommand RemovePhotoCommand { get; set; }
		public IAsyncRelayCommand NextCommand { get; set; }
		#endregion

		#region ctors
		public MainViewModel()
		{
			// resolve services
			this._areaSvc = Ioc.Default.GetService<IAreaService<AreaModel>>();
			this._catSvc = Ioc.Default.GetService<ICategorySerice<CategoryModel>>();
			this._evtSvc = Ioc.Default.GetService<IEventService<EventModel>>();
			this._reqresinSvc = Ioc.Default.GetService<IReqResInService>();
			this._alert = Ioc.Default.GetService<IDisplayAlert>();

			InitCommands();
			Populate();
			GetGeoLocation();
		}
		#endregion

		#region command methods
		private async Task AddPhoto()
		{
			try
			{
				var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
				{
					Title = "Take a photo"
				});

				if (photo != null)
				{
					string base64Img = string.Empty;
					string imagePath = string.Empty;

					// convert image to base64 string
					base64Img = StreamHelpers.ConvertStreamToBase64(photo.OpenReadAsync().Result);

					{ // save the image locally
						var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
						using (var stream = await photo.OpenReadAsync())
						using (var newStream = File.OpenWrite(newFile))
							await stream.CopyToAsync(newStream);
						imagePath = newFile;
					}

					this.Images.Add(new ImageUIModel()
					{
						Id = Guid.NewGuid(),

						// getting a "Payload too large" 
						// from ReqRes.in server
						// so reduce the string being submitted by truncating it
						// since this is for "test project" only.
						ImageBase64 = base64Img.Substring(0, 50),

						ImagePath = imagePath
					});
				}
			}
			catch (Exception ex)
			{
				this._alert.ShowAlert("Error", "Something went wrong while accessing your camera. Please try again");
			}
		}

		private void RemovePhoto(ImageUIModel model)
		{
			Images.Remove(model);
		}

		private async Task Next()
		{
			var ok = true;

			IsBusy = true;

			{ // validate entries
				if (this.SelectedArea == null && this.SelectedCategory == null)
					ok = false;

                if (LinkExistingEvent && this.SelectedEvent == null) 
					ok = false;
            }

			if (ok)
			{
				var model = new DiaryModel()
				{
					DiaryId = Guid.NewGuid(),
					Comments = this.Comments,
					Date = this.DiaryDate,
					SelectedArea = this.SelectedArea,
					SelectedCategory = this.SelectedCategory,
					SelectedEvent = this.SelectedEvent,
					Tags = this.Tags?.Split(new string[] { "'" }, StringSplitOptions.RemoveEmptyEntries)?.ToList()
				};

				model.Images = new List<ImageModel>();
				for (int i = 0; i < this.Images.Count; i++)
					model.Images.Add(new ImageModel()
					{
						Id = this.Images[i].Id,
						ImageBase64 = this.Images[i].ImageBase64
					});

				var result = await this._reqresinSvc.Submit(model);

				if (result)
				{
					_alert.ShowAlert("", "Your dialy diary is submitted");
					ClearFields();
				}
				else
					_alert.ShowAlert("Error", "Unable to save your daily diary");
			}
			else
			{
                _alert.ShowAlert("Error", "Some of the necessary fields are left incomplete.");
            }

            IsBusy = false;

            await Task.CompletedTask;
		}
		#endregion

		#region methods
		public void InitCommands()
		{
			AddPhotoCommend = new AsyncRelayCommand(AddPhoto);
			RemovePhotoCommand = new RelayCommand<ImageUIModel>(RemovePhoto);
			NextCommand = new AsyncRelayCommand(Next);
		}

		public async Task Populate()
		{
			this.Areas.Clear();
			this.Categories.Clear();
			this.Events.Clear();

			var areas = await this._areaSvc?.GetAllAsync();
			if (areas.Any() && areas != null)
				for (int i = 0; i < areas.Count; i++)
					this.Areas.Add(areas[i]);

			var cats = await this._catSvc?.GetAllAsync();
			if (cats.Any() && cats != null)
				for (int i = 0; i < cats.Count; i++)
					this.Categories.Add(cats[i]);

			var events = await this._evtSvc?.GetAllAsync();
			if (events.Any() && events != null)
				for (int i = 0; i < events.Count; i++)
					this.Events.Add(events[i]);
		}

		public async Task GetGeoLocation()
		{
			try
			{
				var loc = await Geolocation.GetLastKnownLocationAsync();

				if (loc != null)
				{
					var pm = await Geocoding.GetPlacemarksAsync(loc.Latitude, loc.Longitude);

					if (pm != null && pm.Any())
					{
						var placemark = pm.FirstOrDefault();

						if (placemark != null)
							this.Location = $"{placemark.Thoroughfare}, {placemark.Locality}, {placemark.AdminArea}, {placemark.CountryName}";
					}
				}
			}
			catch (Exception ex)
			{
				this.Location = "Location not found";
			}
		}

		private void ClearFields()
		{
			this.Images.Clear();
			this.Comments = string.Empty;
			this.Tags = string.Empty;
			this.LinkExistingEvent = false;
			this.SelectedArea = null;
			this.SelectedCategory = null;
			this.SelectedEvent = null;
		}
		#endregion
	}
}