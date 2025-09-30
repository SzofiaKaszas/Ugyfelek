using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Ugyfelek
{
	/// <summary>
	/// Interaction logic for UjRendelesAblak.xaml
	/// </summary>
	public partial class UjRendelesAblak : Window
	{
		private string kapcsolatString;
		public UjRendelesAblak(string kapString)
		{
			InitializeComponent();
			kapcsolatString = kapString;
			LoadCombo();
		}

		private void LoadCombo()
		{
			try
			{
				using var kapcsolat = new MySqlConnection(kapcsolatString);
				kapcsolat.Open();

				string lekerdezes = "SELECT `Id`,`Name` FROM `customers`";

				using var useradapter = new MySqlDataAdapter(lekerdezes, kapcsolat);
				var userTabla = new DataTable();
				useradapter.Fill(userTabla);

				ugyfelek.ItemsSource = userTabla.DefaultView;
				ugyfelek.DisplayMemberPath = "Name";
				ugyfelek.SelectedValuePath = "Id";
			}
			catch (Exception ex)
			{
				MessageBox.Show("Hiba " + ex.Message);
			}
		}

		private void Hozzaad_Click(object sender, RoutedEventArgs e)
		{
			if (ugyfelek.SelectedValue == null || string.IsNullOrWhiteSpace(termek.Text) || string.IsNullOrWhiteSpace(mennyiseg.Text))
			{
				MessageBox.Show("Minden mezőbe adj meg valamit");
				return;
			}

			int ugyfelId = (int)ugyfelek.SelectedValue;
			MessageBox.Show(ugyfelId + "");
			string termekNeve = termek.Text;

			int mennyisegInt;

			if (int.TryParse(mennyiseg.Text, out int result))
			{
				mennyisegInt = result;
			}
			else
			{
				MessageBox.Show("A mennyiség szám legyen");
				return;
			}

			DateTime rendelesDatum = datum.SelectedDate ?? DateTime.Today;

			bool kiszall = kiszallitva.IsChecked == true;

			try
			{
				using var kapcsolat = new MySqlConnection(kapcsolatString);
				kapcsolat.Open();

				string hozzaad = "INSERT INTO `orders`(`CustomerId`, `Product`, `Quantity`, `OrderDate`, `Delivered`) VALUES (@ugyfel,@termek,@mennyiseg,@rendeles,@kiszallitas)";

				using var cmd = new MySqlCommand(hozzaad, kapcsolat);
				cmd.Parameters.AddWithValue("@ugyfel", ugyfelId);
				cmd.Parameters.AddWithValue("@termek", termekNeve);
				cmd.Parameters.AddWithValue("@mennyiseg", mennyisegInt);
				cmd.Parameters.AddWithValue("@rendeles", rendelesDatum);
				cmd.Parameters.AddWithValue("@kiszallitas", kiszall);
				cmd.ExecuteNonQuery();

				MessageBox.Show("Sikeres megrendeles!", "Mentés", MessageBoxButton.OK, MessageBoxImage.Information);

				this.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Hiba " + ex.Message);
			}
		}
	}
}
