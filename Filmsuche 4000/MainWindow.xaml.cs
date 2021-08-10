using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Filmsuche_4000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Suche { get; set; }
        
        private Models.Search[] data;
        public dynamic Data => data!=null ? new ObservableCollection<dynamic>(data) : null;

        private async void Suche_Changed(object sender, TextChangedEventArgs e)
        {
            data = null;
            if (!string.IsNullOrEmpty(Suche) && Suche.Length>3)
            {
                using (var http=new HttpClient() { BaseAddress=new Uri("https://www.omdbapi.com/") })
                {
                    var response = await http.GetAsync($"?apikey=477bca08&s={Suche}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Models.MovieModel model = System.Text.Json.JsonSerializer.Deserialize<Models.MovieModel>(json);
                        data = model.Search;
                    }
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Data"));
        }
    }
}
