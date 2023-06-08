using Plugin.Messaging;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using WorkApp.Models;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WorkApp.Views
{
    public partial class GetAllWorkersPage : ContentPage
    {
        private Button _button;
        private ListView _listView;
        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");
        public GetAllWorkersPage()
        {
            this.Title = "Workers";

            var db = new SQLiteConnection(_dbPath);

            StackLayout stackLayout = new StackLayout();

            _listView = new ListView();
            _listView.ItemsSource = db.Table<Worker>().OrderBy(x => x.Name).ToList();
            _listView.ItemSelected += OnItemSelected;
            stackLayout.Children.Add(_listView);

            Content = stackLayout;

            _button = new Button();
            _button.Text = "Make a call";
            _button.Clicked += call_btn_Clicked;
            stackLayout.Children.Add(_button);
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedWorker = e.SelectedItem as Worker;

            var phoneNumber = GetPhoneNumberFromDatabase(selectedWorker.Id, _dbPath);

            if (string.IsNullOrEmpty(phoneNumber))
            {
                DisplayAlert("Error", "Phone number not found.", "OK");
                return;
            }

            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
            {
                phoneDialer.MakePhoneCall(phoneNumber);
            }
            else
            {
                DisplayAlert("Error", "Phone call not supported.", "OK");
            }

            ((ListView)sender).SelectedItem = null;
        }

        private string GetPhoneNumberFromDatabase(int workerId, string _dbPath)
        {
            using (var connection = new SQLiteConnection(_dbPath))
            {
                var worker = connection.Table<Worker>().FirstOrDefault(w => w.Id == workerId);
                if (worker != null)
                {
                    return worker.Phone;
                }
            }

            return null;
        }

        private void call_btn_Clicked(object sender, EventArgs e)
        {
            var call = CrossMessaging.Current.PhoneDialer;
            if (call.CanMakePhoneCall)
            {
                call.MakePhoneCall("+37253006922");
            }
        }
    }
}