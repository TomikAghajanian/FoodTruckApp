
using FoodTruckApp.Droid;
using FoodTruckApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace FoodTruckApp.Pages
{
    public partial class MainMenu : MasterDetailPage
    {
        public ObservableCollection<MainMenuItem> MainMenuItems { get; set; }

        public MainMenu()
        {
            // Set the binding context to this code behind.
            BindingContext = this;

            MainMenuItems = new ObservableCollection<MainMenuItem>();

            // Build the Menu
            MessagingCenter.Subscribe<string>(this, "update", (sender) =>
            {
                MainMenuItem mainMenuItem = new MainMenuItem() { Title = sender, Icon = "menu_inbox.png", TargetType = typeof(PageOne) };
                MainMenuItems.Add(mainMenuItem);
            });

            Detail = new NavigationPage(new MapPage());

            InitializeComponent();
        }



        // When a MenuItem is selected.
        public void MainMenuItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;
            if (item != null)
            {
                if (item.Title.Equals("Page One"))
                {
                    Detail = new NavigationPage(new PageOne());
                }
                else if (item.Title.Equals("Page Two"))
                {
                    Detail = new NavigationPage(new PageTwo());
                }

                MenuListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
