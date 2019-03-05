using System.Collections.Generic;
using System.Windows.Controls;

namespace SlimUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UpdaterPage : Page
    {
        public UpdaterPage()
        {
            InitializeComponent();
            List<User> items = new List<User>();
            items.Add(new User() { Name = "John Doe", Age = 42 });
            items.Add(new User() { Name = "Jane Doe", Age = 39 });
            items.Add(new User() { Name = "Sammy Doe", Age = 13 });
            items.Add(new User() { Name = "this is a veeeeeerrrrrrryyyyy loooooooooooooonnnngggggg nameeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee", Age = 18 });
            updateListView.ItemsSource = items;
        }

        public class User
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }
    }
}
