using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ProduceManagement1
{
    public partial class Produce : Form
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Bogdan\\Documents\\StorageDB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public Produce()
        {
            InitializeComponent();
            tbxProduct.Focus();
            cbxStorage.Items.Clear();
            cbxStorage.Items.Add("Fridge 1");
            cbxStorage.Items.Add("Fridge 2");
            LoadData();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            string productName = tbxProduct.Text;
            int productStorage = cbxStorage.SelectedItem.ToString() == "Fridge 1" ? 1 : 2;

            DateTime productReceived = dateRecieved.Value;
            DateTime productBBE = dateBefore.Value;

            // Insert into database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO StorageTbl ([ProductName], [ProductStorage], [ProductRecieved], [ProductBBE]) " +
                                   "VALUES (@ProductName, @ProductStorage, @ProductRecieved, @ProductBBE)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@ProductStorage", productStorage);
                        cmd.Parameters.AddWithValue("@ProductRecieved", productReceived);
                        cmd.Parameters.AddWithValue("@ProductBBE", productBBE);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        MessageBox.Show(rowsAffected > 0 ? "Product successfully saved to storage." : "No rows were affected. Product was not saved.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {
                    // Clear the form fields
                    tbxProduct.Clear();
                    cbxStorage.SelectedIndex = -1;
                    dateRecieved.Value = DateTime.Today;
                    dateBefore.Value = DateTime.Today;
                }
            }

            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                int productID = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);

                var confirmResult = MessageBox.Show("Are you sure you want to delete this entry?",
                                                     "Confirm Delete",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    DeleteProduct(productID);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidateInputs() // Checks input boxes and validates if input is correct
        {
            if (string.IsNullOrWhiteSpace(tbxProduct.Text))
            {
                MessageBox.Show("Please enter a product name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxProduct.Focus();
                return false;
            }

            if (cbxStorage.SelectedItem == null)
            {
                MessageBox.Show("Please select a storage location (Fridge 1 or Fridge 2).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxStorage.Focus();
                return false;
            }

            if (dateBefore.Value.Date < dateRecieved.Value.Date)
            {
                MessageBox.Show("The Best Before End (BBE) date cannot be earlier than the product received date.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateBefore.Focus();
                return false;
            }

            return true;
        }

        private void DeleteProduct(int productID) // Deletes data from database
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM StorageTbl WHERE ProductID = @ProductID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product deleted successfully.");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("No product found with the specified ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void LoadData() // Loads data into grid
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM StorageTbl";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
              
            }
        }
    }
}
