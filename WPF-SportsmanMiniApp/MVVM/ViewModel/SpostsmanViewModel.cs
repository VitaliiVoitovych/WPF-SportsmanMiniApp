﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_SportsmanMiniApp.Core;
using WPF_SportsmanMiniApp.MVVM.Model;

namespace WPF_SportsmanMiniApp.MVVM.ViewModel
{
    public class SpostsmanViewModel : ObservableObject
    {
        private int index = 0;
        private readonly AppDbContext db;
        public ObservableCollection<Sportsman> Sportsmen { get; set; }
        private Sportsman currentSportsman;
        private bool isReadOnly;
        public bool IsReadOnlyProp
        {
            get => isReadOnly;
            set
            {
                if(isReadOnly == value) return;
                isReadOnly = value;
                OnPropertyChanged();
            }
        }
        private string mode;
        public string Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                OnPropertyChanged();
            }
        }
        public Sportsman CurrentSportsman
        {
            get => currentSportsman;
            set
            {
                if (currentSportsman == value) return;
                currentSportsman = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand PrevBtnCommand { get; set; }
        public RelayCommand NextBtnCommand { get; set; }
        public RelayCommand OkBtnCommand { get; set; }
        public RelayCommand BrowseCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public SpostsmanViewModel()
        {
            db = new AppDbContext();
            db.Sportsmen.Load();
            Sportsmen = db.Sportsmen.Local;
            PrevBtnCommand = new RelayCommand((o) =>
            {
                if (index > 0)
                {
                    CurrentSportsman = Sportsmen[--index];
                }
            });
            NextBtnCommand = new RelayCommand((o) =>
            {
                if (index < Sportsmen.Count - 1)
                {
                    CurrentSportsman = Sportsmen[++index];
                }
            });
            BrowseCommand = new RelayCommand((o) =>
            {
                Mode = "Browse";
                CurrentSportsman = Sportsmen.FirstOrDefault();
                IsReadOnlyProp = true;
            });
            AddCommand = new RelayCommand((o) =>
            {
                Mode = "Add";
                IsReadOnlyProp = false;
                CurrentSportsman = new Sportsman();
            });
            EditCommand = new RelayCommand((o) =>
            {
                Mode = "Edit";
                CurrentSportsman = Sportsmen.FirstOrDefault();
                IsReadOnlyProp = false;
            });
            RemoveCommand = new RelayCommand((o) =>
            {
                Mode = "Remove";
                IsReadOnlyProp = true;
            });
            OkBtnCommand = new RelayCommand((o) =>
            {
                if (Mode == "Add")
                {
                    Sportsmen.Add(CurrentSportsman);
                }
                if (Mode == "Remove")
                {
                    Sportsmen.Remove(CurrentSportsman);
                    CurrentSportsman = Sportsmen.FirstOrDefault();
                }
                db.SaveChanges();
            });
        }
    }
}
