namespace Bookie.UserControls
{
    using Bookie.Common.Model;
    using System.Collections.Generic;

    public partial class BookTiles
    {
        public static List<Book> b = new List<Book>();

        public BookTiles()
        {
            InitializeComponent();
        }

        private void Path_65_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (Book item in Lb.SelectedItems)
            {
                b.Add(item);
            }
        }
    }
}