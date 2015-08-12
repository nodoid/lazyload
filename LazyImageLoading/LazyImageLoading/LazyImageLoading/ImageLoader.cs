// Image loading system adapted from the example by Josh Quintus

using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace LazyImageLoading
{
    public class ImageList
    {
        public Label UserLabel { get; set; }

        public Image UserImage { get; set; }

        public ImageSource IconSource { get; set; }
    }

    public class Images
    {
        public string name { get; set; }

        public string url { get; set; }

        public string id { get; set; }
    }

    public class ImageLoader : ContentPage
    {
        List<Images> imageList;

        public NotifyTaskCompletion<ImageSource> IconSource { get; private set; }

        readonly string heads = Device.OS == TargetPlatform.WinPhone ? "Images/heads.png" : "heads.png";



        public ImageLoader()
        {
            if (Device.OS == TargetPlatform.iOS)
                Padding = new Thickness(0, 20, 0, 0);

            imageList = new List<Images>
            {
                new Images { name = "Percy", url = "http://www.all-the-johnsons.co.uk/images/Pooh-small.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "Lou", url = "http://www.all-the-johnsons.co.uk/images/Lou.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "Angel", url = "http://www.all-the-johnsons.co.uk/images/Angel.jpg", id = Guid.NewGuid().ToString() },
                new Images { name = "Monty", url = "http://www.all-the-johnsons.co.uk/images/Monty.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "Netherley", url = "http://www.all-the-johnsons.co.uk/images/Netherley.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "New Heys", url = "http://www.all-the-johnsons.co.uk/images/NewHeys.jpg", id = Guid.NewGuid().ToString() },
                new Images { name = "RiscStation", url = "http://www.all-the-johnsons.co.uk/images/RSlogo.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "Sally", url = "http://www.all-the-johnsons.co.uk/images/Sally-small.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "Amadeus", url = "http://www.all-the-johnsons.co.uk/images/amadeus.jpg", id = Guid.NewGuid().ToString() },
                new Images { name = "Dalek", url = "http://www.all-the-johnsons.co.uk/images/dalek.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "Dragon", url = "http://www.all-the-johnsons.co.uk/images/dragon.png", id = Guid.NewGuid().ToString() },
                new Images{ name = "Dr Who", url = "http://www.all-the-johnsons.co.uk/images/drwhologo.jpg", id = Guid.NewGuid().ToString() },
                new Images { name = "Falco", url = "http://www.all-the-johnsons.co.uk/images/falco.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "Mole", url = "http://www.all-the-johnsons.co.uk/images/mole.png", id = Guid.NewGuid().ToString() },
                new Images{ name = "Steve Irwin", url = "http://www.all-the-johnsons.co.uk/images/steve_irwin150.jpg", id = Guid.NewGuid().ToString() },
                new Images { name = "Suits You!", url = "http://www.all-the-johnsons.co.uk/images/suitsyou.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "Tux", url = "http://www.all-the-johnsons.co.uk/images/tux.png", id = Guid.NewGuid().ToString() },
                new Images{ name = "BCS logo", url = "http://www.all-the-johnsons.co.uk/images/bcs.jpg", id = Guid.NewGuid().ToString() },
                new Images{ name = "FarmApps", url = "http://www.farmapps.com.au/images/app-icon-152.png", id = Guid.NewGuid().ToString() },
                new Images{ name = "uSee", url = "https://www.usee.com/img/usee-logo-transparent.png", id = Guid.NewGuid().ToString() }
            };


            var imagesList = new List<ImageList>();   

            for (var i = 0; i < imageList.Count; ++i)
            {
                imagesList.Add(new ImageList{ UserLabel = new Label{ Text = imageList[i].name, StyleId = imageList[i].id } });
                imagesList[i].UserImage = new Image{ WidthRequest = 50, HeightRequest = 50, Aspect = Aspect.AspectFill, StyleId = imageList[i].id };
                imagesList[i].IconSource = AsyncImageSource.FromUriAndResource(imageList[i].url, heads).Result;
            }

            var listView = new ListView
            {
                ItemsSource = imagesList,
                ItemTemplate = new DataTemplate(() =>
                    {
                        var nameLabel = new Label();
                        nameLabel.SetBinding(Label.TextProperty, "UserLabel.Text");
                        var image = new Image();
                        image.SetBinding(Image.SourceProperty, "IconSource");
                        return new ViewCell
                        {
                            View = new StackLayout
                            {
                                Padding = new Thickness(0, 5),
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    image,
                                    new StackLayout
                                    {
                                        VerticalOptions = LayoutOptions.Center,
                                        Spacing = 0,
                                        Children =
                                        {
                                            nameLabel,
                                        }
                                    }
                                }
                            }
                        };
                    }),
            };

            Content = new StackLayout
            {
                Children =
                {
                    listView
                }
            };
        }
    }
}


