using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeerOrCoffee.Enums;
using BeerOrCoffee.Interfaces;
using BeerOrCoffee.Services;
using BeerOrCoffee.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace BeerOrCoffee
{
    public partial class BeerOrCoffeePage : ContentPage
    {         private readonly IImageCropService _imageCropService;
        private readonly DataTemplate _userResultTemplate;
        private readonly EmotionCognitiveSevice _emotionService;

        private bool _initialCameraCall;

        private byte[] _userPhotoBytes;          public BeerOrCoffeePage()         {             InitializeComponent();

            ViewModel = new BeerOrCoffeeViewModel();

            _userResultTemplate = new DataTemplate(typeof(UserResultCell));
            _imageCropService = DependencyService.Get<IImageCropService>();
            _emotionService = new EmotionCognitiveSevice();         }

        public BeerOrCoffeeViewModel ViewModel
        {
            get;
        }             protected override void OnAppearing()         {             base.OnAppearing();              if (!_initialCameraCall)             {                 _initialCameraCall = true;                 InitialCameraCall().ConfigureAwait(false);             }         }          private async Task InitialCameraCall()         {             await CrossMedia.Current.Initialize();              await TakePhoto();         }
         private async Task TakePhoto()         {             var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() { DefaultCamera = CameraDevice.Front});

            if (photo == null)
                return;

            ClearView();
             var photoStream = photo.GetStream();

            // demo
            var memoryStream = new MemoryStream();
            photoStream.CopyTo(memoryStream);
            _userPhotoBytes = memoryStream.ToArray();

            UserImage.Source = ImageSource.FromStream(() => new MemoryStream(_userPhotoBytes));

            var result = await _emotionService.SendImage(_userPhotoBytes);

            var items = result.Select(face =>
            {
                var cropRect = new Rectangle(face.FaceRectangle.Left, face.FaceRectangle.Top, face.FaceRectangle.Width + face.FaceRectangle.Left, face.FaceRectangle.Height + face.FaceRectangle.Top);
                var scores = face.Scores;

                // TODO: Rherlt 
                var coffeeOrBeerType = BeerOrCoffeeType.Beer;

                var croppedImageBytes = _imageCropService.CropImage(_userPhotoBytes, cropRect);

                return new UserResultItem() { UserImage = croppedImageBytes, Type = coffeeOrBeerType };
            }).ToArray();

            CreateUserImages(items);

            #region Create Demo
            //CreateBeerUser();

            //CreateCoffeeUser();

            #endregion

            // set visibilities
            BeerStack.IsVisible = BeerStack.Children.Any();
            CoffeeStack.IsVisible = CoffeeStack.Children.Any();         }

        private void ClearView()
        {
            // clear image
            UserImage.Source = null;

            // clear beer
            BeerStack.Children.Clear();

            // clear coffee
            CoffeeStack.Children.Clear();
        }


        private void OnStartAgain(object sender, System.EventArgs e)
        {
            TakePhoto().ConfigureAwait(false);
        }

        private void CreateUserImages(IList<UserResultItem> items)
        {
            foreach (var item in items)
            {
                if(item.Type == BeerOrCoffeeType.Beer)
                    BeerStack.Children.Add(CreateBeerOrCoffeeResult(item));
                else
                    CoffeeStack.Children.Add(CreateBeerOrCoffeeResult(item));
            }
        }

        #region Demo
        private void CreateBeerUser()
        {
            // beer demo
            var demoBeerCount = 3;
            for (int i = 0; i < demoBeerCount; i++)
            {
                var cropRect = new Rectangle(0 + (50 * i), 0 + (50 * i), 250 + (50 * i), 250 + (50 * i));
                var croppedImageBytes = _imageCropService.CropImage(_userPhotoBytes, cropRect);
                BeerStack.Children.Add(CreateBeerOrCoffeeResult(new UserResultItem() { UserImage = croppedImageBytes, Type = Enums.BeerOrCoffeeType.Beer }));
            }
        }

        private void CreateCoffeeUser()
        {
            // coffee demo
            var demoCoffeeCount = 2;
            for (int i = 0; i < demoCoffeeCount; i++)
            {
                var cropRect = new Rectangle(0 + (50 * i), 0 + (50 * i), 250 + (50 * i), 250 + (50 * i));
                var croppedImageBytes = _imageCropService.CropImage(_userPhotoBytes, cropRect);
                CoffeeStack.Children.Add(CreateBeerOrCoffeeResult(new UserResultItem() { UserImage = croppedImageBytes, Type = Enums.BeerOrCoffeeType.Coffee }));
            }
        }
        #endregion

        private View CreateBeerOrCoffeeResult(object dataContext)
        {
            var view = _userResultTemplate.CreateContent() as UserResultCell;
            view.BindingContext = dataContext;
            return view;
        }
    }
}
