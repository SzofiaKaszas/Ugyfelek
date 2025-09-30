using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ugyfelek
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string kapcsolatString = "server=localhost;" +
								 "user=root;" +
								 "database=shop;" +
								 "password=;" +
								 "SslMode=none;";
		public MainWindow()
		{
			InitializeComponent();
			Load();
		}

		private void Load()
		{
			try
			{
				using var kapcsolat = new MySqlConnection(kapcsolatString);
				kapcsolat.Open();

				string lekerdezes = "SELECT customers.Name, orders.Product, orders.Quantity, orders.OrderDate, orders.Delivered FROM `orders` INNER JOIN customers ON orders.CustomerId = customers.Id;";

				using var adapter = new MySqlDataAdapter(lekerdezes, kapcsolat);
				var tabla = new DataTable();
				adapter.Fill(tabla);
				ugyfelek.ItemsSource = tabla.DefaultView;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Hiba: " + ex.Message);
			}
		}

		private void UjRendeles_Click(object sender, RoutedEventArgs e)
		{
			var windows = new UjRendelesAblak(kapcsolatString);
			windows.ShowDialog();
			Load();
		}
	}
}