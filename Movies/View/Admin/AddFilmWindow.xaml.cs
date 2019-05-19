﻿using Microsoft.Win32;
using Movies.DataModel;
using Movies.LogicApp;
using Movies.LogicApp.Helps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Movies.View.Admin
{
    /// <summary>
    /// Логика взаимодействия для AddFilmWindow.xaml
    /// </summary>
    public partial class AddFilmWindow : Window
    {

        #region Секция свойств

        Films film; // Фильм

        AdminLogic logic; // Логика работы администратора с сервером
        List<Actors> ListActors; // Общий список актеров
        List<Actors> FilmActors; // Список актеров фильма

        // Путь к файлу
        public string FilePath { get; set; }


        List<ActorsFilm> ThisFilmActors; // Список актеров фильма

        
        int? idProducer; // Айди продюсера фильма
        DateTime? dateFilm; // Дата выхода фильма
        int? idGenre; // Айди жанра
        string Country; // Страна
        #endregion

        

        public AddFilmWindow()
        {
            InitializeComponent();

            logic = new AdminLogic(); // Логика работы администратора
            load(); // Загружаем первоначальные данные
        }


        #region Секция методов

        // Метод, который загружает первоначальные данные в асинхронном режиме в компоненты
        private async void load()
        {
            ProducersCB.ItemsSource = await logic.GetProducersAsync(); // Получаем список продюссеров
            GenresCB.ItemsSource = await logic.GetGenresAsync(); // Получаем список жанров
            CountriesCB.ItemsSource = new List<string>() // Задаем страны
                {
                    "Россия",
                    "Украина",
                    "США",
                    "Канада",
                    "Англия",
                    "Германия"
                };

            ListActors = await logic.GetActorsAsync(); // Получаем список актеров
            ActorsCB.ItemsSource = ListActors; // Задаем список актеров в ComboBox

        }

        #endregion

        #region Секция событий


        #region Секция событий, которые привязывают измененные значения

        // Событие на изменение в Странах
        private void CountriesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Country = (string)CountriesCB.SelectedItem;
        }

        // Событие на изменение в жанрах
        private void GenresCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (Genres)GenresCB.SelectedItem;

            idGenre = item.IdGenre;
        }

        // Событие на изменение выбранной даты
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateFilm = DateFilm.SelectedDate;
        }

        // Событие на изменение выбранного значения в ProducersCB
        private void ProducersCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (Producers)ProducersCB.SelectedItem; // Получаем выбранный итем

            idProducer = item.idProducer; // Присваиваем в память значение айди продюсера
        }

        #endregion


        #region Секция событий кликов на кнопки

        // Событие на клик кнопки добавить актера
        private void AddActors_Click(object sender, RoutedEventArgs e)
        {
            // Если входные данные введены, то добавь в список
            if (role.Text != string.Empty && ActorsCB.SelectedItem != null)
            {
                // Если список актеров фильма пустой, то проинициализируй его
                if (FilmActors == null)
                    FilmActors = new List<Actors>();


                // Получаем актера 
                var actor = (Actors)ActorsCB.SelectedItem;
                actor.Role = role.Text;




                FilmActors.Add(actor);

                lv.ItemsSource = FilmActors;
                lv.Items.Refresh();


                // Удаляем из общего пула актера
                ListActors.Remove(actor);



                ActorsCB.Items.Refresh();

                role.Text = string.Empty; // Обнуляем текст в роли героя
            }
        }


        // Событие на клик добавить постер
        private void AddPoster_Click(object sender, RoutedEventArgs e)
        {


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы изображений (*.jpg, *.png)|*.jpg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                //MessageBox.Show(FilePath);

                //img.Source = new BitmapImage(new Uri(FilePath));


                var imageData = ImageLogic.GetImageBinary(FilePath);

                img.Source = ImageLogic.ByteToImage(imageData);
            }
        }


        // Событие на клик удалить актера
        private void DeleteActors_Click(object sender, RoutedEventArgs e)
        {
            if (lv.SelectedItem != null)
            {
                var item = (Actors)lv.SelectedItem;

                if (item != null)
                {
                    FilmActors.Remove(item);
                    ListActors.Add(item);

                    ActorsCB.Items.Refresh();
                    lv.Items.Refresh();
                }
            }
        }


        // Событие на клик добавить фильм
        private void AddFilm_Click(object sender, RoutedEventArgs e)
        {
            // Если все данные ввели, то добавь фильм в БД
            if (
                FilmName.Text != string.Empty &&
                idProducer != null &&
                dateFilm != null &&
                idGenre != null &&
                Country != string.Empty &&
                FilmActors != null
                )
            {
                MessageBox.Show($"Добавляем филм! {dateFilm}");
            }


            //film = new Films()
            //{

            //};

            //// Список актеров фильма в коллекции
            //List<ActorsFilm> actorsFilm = new List<ActorsFilm>()
            //{

            //};

            //ThisFilmActors.Add(new ActorsFilm()
            //{
            //    IdActor = actor.IdActor,
            //    Role = actor.Role,
            //    IdFilm = film.IdFilm
            //});
        }


        #endregion

        #endregion


    }
}
