using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Bookie.Common.Model;

namespace Bookie.UserControls
{
    public partial class BookTiles
    {
        public static List<Book> b = new List<Book>();

        public BookTiles()
        {
            InitializeComponent();
        }

        private void Path_65_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Book item in Lb.SelectedItems)
            {
                b.Add(item);
            }
        }
    }
}