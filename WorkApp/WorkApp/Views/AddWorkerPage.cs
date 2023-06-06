using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WorkApp.Models;
using Xamarin.Forms;
using SQLite;

namespace WorkApp.Views
{
    public class AddWorkerPage : ContentPage
    {
        private Entry _nameEntry;
        private Entry _phoneEntry;
        private Button _saveButton;

        string _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDB.db3");

        public AddWorkerPage()
        {
            this.Title = "Add Worker";

            StackLayout stackLayout = new StackLayout();

            _nameEntry = new Entry();
            _nameEntry.Keyboard = Keyboard.Text;
            _nameEntry.Placeholder = "Worker Name";
            stackLayout.Children.Add( _nameEntry );

            _phoneEntry = new Entry();
            _phoneEntry.Keyboard = Keyboard.Text;
            _phoneEntry.Placeholder = "Worker Phone Number";
            stackLayout.Children.Add(_phoneEntry);

            _saveButton = new Button();
            _saveButton.Text = "Add";
            _saveButton.Clicked += _saveButton_Clicked;
            stackLayout.Children.Add(_saveButton);

            Content = stackLayout;
        }

        private async void _saveButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(_dbPath);
            db.CreateTable<Worker>();

            var maxPk = db.Table<Worker>().OrderByDescending(c => c.Id).FirstOrDefault();

            Worker worker = new Worker()
            {
                Id = (maxPk == null ? 1 : maxPk.Id + 1),
                Name = _nameEntry.Text,
                Phone = _phoneEntry.Text,
            };
            db.Insert(worker);
            await DisplayAlert(null, worker.Name + " Added", "Ok");
            await Navigation.PopAsync();
        }
    }
}