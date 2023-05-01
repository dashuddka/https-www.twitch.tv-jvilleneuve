using Avalonia.Controls.Platform;
using Microsoft.CodeAnalysis;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using System.IO;
using Avalonia.Media.Imaging;
using System.Data;
using System.Reactive;
using System.Collections.ObjectModel;
using FlightScoreboard.Models;
using DynamicData;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using SkiaSharp;

namespace FlightScoreboard.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool isArrival = false;
        private bool isDeparture = false;
        private bool isYesterday = false;
        private bool isToday = false;
        private bool isTomorrow = false;
        private string date = "";
        private ObservableCollection<Flight> yesterday_arrival;
        private ObservableCollection<Flight> yesterday_departure;
        private ObservableCollection<Flight> today_arrival;
        private ObservableCollection<Flight> today_departure;
        private ObservableCollection<Flight> tomorrow_arrival;
        private ObservableCollection<Flight> tomorrow_departure;
        private List<Flight> list;
        private ObservableCollection<Flight> current;


        public MainWindowViewModel()
        {
            yesterday_arrival = new ObservableCollection<Flight>();
            yesterday_departure = new ObservableCollection<Flight>();
            today_arrival = new ObservableCollection<Flight>();
            today_departure = new ObservableCollection<Flight>();
            tomorrow_arrival = new ObservableCollection<Flight>();
            tomorrow_departure = new ObservableCollection<Flight>();
            IsDeparture = true;
            IsToday = true;
            Update();
            Refresh = ReactiveCommand.Create(() =>
            {
                yesterday_arrival = new ObservableCollection<Flight>();
                yesterday_departure = new ObservableCollection<Flight>();
                today_arrival = new ObservableCollection<Flight>();
                today_departure = new ObservableCollection<Flight>();
                tomorrow_arrival = new ObservableCollection<Flight>();
                tomorrow_departure = new ObservableCollection<Flight>();
                Update();
            });
            Current = today_departure;

        }

        private void Update()
        {
            date = DateTime.Today.ToString("dd/MM/yyyy");
            string yesterday = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
            string tomorrow = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
            using (var connection = new SqliteConnection("Data Source=../../../Assets/database.db;Mode=ReadOnly"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand($"SELECT * FROM Flights WHERE Data in ('{date}', '{yesterday}', '{tomorrow}')", connection);
                list = new List<Flight>();
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Flight fl = new Flight();
                            var image = reader[0] as byte[];
                            MemoryStream imageStream = new MemoryStream(image);
                            fl.MiniImage = new Bitmap(imageStream);
                            fl.Company = reader[1] as string;
                            fl.Number = (Int64)reader[2];
                            fl.Source = reader[3] as string;
                            fl.Destination = reader[4] as string;
                            if (fl.Source != "Новосибирск") fl.City = fl.Source;
                            else fl.City = fl.Destination;
                            fl.Schedule = reader[5] as string;
                            fl.Settlement = reader[6] as string;
                            fl.Sector = reader[7] as string;
                            fl.Status = reader[8] as string;
                            image = reader[9] as byte[];
                            imageStream = new MemoryStream(image);
                            fl.Image = new Bitmap(imageStream);
                            fl.Description1 = reader[10] as string;
                            fl.Description2 = reader[11] as string;
                            fl.Data = reader[12] as string;
                            fl.Way = $"{fl.Source} -> {fl.Destination}";
                            list.Add(fl);
                        }
                    }
                }
                foreach (Flight fly in list)
                {
                    if (fly.Destination == "Новосибирск")
                    {
                        if (fly.Data == yesterday) yesterday_arrival.Add(fly);
                        else if (fly.Data == date) today_arrival.Add(fly);
                        else tomorrow_arrival.Add(fly);
                    }
                    else
                    {
                        if (fly.Data == yesterday) yesterday_departure.Add(fly);
                        else if (fly.Data == date) today_departure.Add(fly);
                        else tomorrow_departure.Add(fly);
                    }
                }
            }
        }

        public bool IsArrival
        {
            get => isArrival;
            set
            {
                this.RaiseAndSetIfChanged(ref isArrival, value);
                if (isYesterday) Current = yesterday_arrival;
                else if (isToday) Current = today_arrival;
                else Current = tomorrow_arrival;
            }
        }
        public bool IsDeparture
        {
            get => isDeparture;
            set
            {
                this.RaiseAndSetIfChanged(ref isDeparture, value);
                if (isYesterday) Current = yesterday_departure;
                else if (isToday) Current = today_departure;
                else Current = tomorrow_departure;
            }
        }
        public bool IsYesterday
        {
            get => isYesterday;
            set
            {
                this.RaiseAndSetIfChanged(ref isYesterday, value);
                if (isArrival) Current = yesterday_arrival;
                else Current = yesterday_departure;
            }
        }
        public bool IsToday
        {
            get => isToday;
            set
            {
                this.RaiseAndSetIfChanged(ref isToday, value);
                if (isArrival) Current = today_arrival;
                else Current = today_departure;
            }
        }
        public bool IsTomorrow
        {
            get => isTomorrow;
            set
            {
                this.RaiseAndSetIfChanged(ref isTomorrow, value);
                if (isArrival) Current = tomorrow_arrival;
                else Current = tomorrow_departure;
            }
        }

        public ObservableCollection<Flight> Current
        {
            get => current;
            set => this.RaiseAndSetIfChanged(ref current, value);
        }

        public ReactiveCommand<Unit, Unit> Refresh { get; }
    }
}
