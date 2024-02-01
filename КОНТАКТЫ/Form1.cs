using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using КОНТАКТЫ;
using MaskedTextBox;


namespace КОНТАКТЫ
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.MaskedTextBox TextBox2;

        private const string ConnectionString = "Host=localhost;Username=postgres;Password=1111;Database=number";
        public Form1()
        {
            InitializeComponent();
       
          

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string numberPhone = maskedTextBox1.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(numberPhone))
            {
                MessageBox.Show("Заполните все обязательные поля", "Предупреждение");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "INSERT INTO Контакты  VALUES (DEFAULT,@Имя_контакта, @Номер)";
                        cmd.Parameters.AddWithValue("@Имя_контакта", name);
                        cmd.Parameters.AddWithValue("@Номер", numberPhone);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Контакт успешно добавлен в базу данных", "Успех");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить контакт в базу данных", "Ошибка");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nameSearchTerm = textBox1.Text.Trim();
            string numberSearchTerm = maskedTextBox1   .Text.Trim();

            if (string.IsNullOrWhiteSpace(nameSearchTerm) && string.IsNullOrWhiteSpace(numberSearchTerm))
            {
                MessageBox.Show("Введите имя или номер для удаления контакта", "Предупреждение");
                return;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(nameSearchTerm))
                {
                    DeleteContactFromDatabase("Имя_контакта", nameSearchTerm);
                }
                else if (!string.IsNullOrWhiteSpace(numberSearchTerm))
                {
                    DeleteContactFromDatabase("Номер", numberSearchTerm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void DeleteContactFromDatabase(string column, string searchTerm)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = $"DELETE FROM Контакты WHERE {column} = @SearchTerm";
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Контакт успешно удален из базы данных", "Успех");
                    }
                    else
                    {
                        MessageBox.Show("Контакт не найден", "Предупреждение");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string numberSearchTerm = maskedTextBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(numberSearchTerm))
            {
                MessageBox.Show("Введите номер для поиска контакта", "Предупреждение");
                return;
            }

            try
            {
                OpenForm2WithContact(numberSearchTerm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void OpenForm2WithContact(string searchTerm)
        {

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM Контакты WHERE Номер = @SearchTerm";
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            // Извлечение информации о контакте из DataTable
                            string contactName = dataTable.Rows[0]["Имя_контакта"].ToString();
                            string contactNumber = dataTable.Rows[0]["Номер"].ToString();

                            // Отображение информации в MessageBox
                            MessageBox.Show($"Имя контакта: {contactName}\nНомер телефона: {contactNumber}", "Детали контакта");
                        }
                        else
                        {
                            MessageBox.Show("Контакт не найден", "Предупреждение");
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string nameSearchTerm = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(nameSearchTerm))
            {
                MessageBox.Show("Введите имя для поиска контакта", "Предупреждение");
                return;
            }

            try
            {
                SearchContactByName(nameSearchTerm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка");
            }
        }
        private void SearchContactByName(string searchTerm)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM Контакты WHERE Имя_контакта = @SearchTerm";
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            // Извлечение информации о контакте из DataTable
                            string contactName = dataTable.Rows[0]["Имя_контакта"].ToString();
                            string contactNumber = dataTable.Rows[0]["Номер"].ToString();

                            // Отображение информации в MessageBox
                            MessageBox.Show($"Имя контакта: {contactName}\nНомер телефона: {contactNumber}", "Детали контакта");
                        }
                        else
                        {
                            MessageBox.Show("Контакт не найден", "Предупреждение");
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string nameSearchTerm = textBox1.Text.Trim();
            string newNumber = maskedTextBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(nameSearchTerm) || string.IsNullOrWhiteSpace(newNumber))
            {
                MessageBox.Show("Введите имя и новый номер для поиска контакта", "Предупреждение");
                return;
            }

            try
            {
                // Получение информации о контакте до обновления
                string contactName = nameSearchTerm;
                string oldNumber = GetOldNumber(contactName);

                // Вывод MessageBox с информацией о контакте до обновления
                MessageBox.Show($"Имя контакта: {contactName}\nСтарый номер: {oldNumber}\nНовый номер: {newNumber}", "Детали контакта");

                // Затем выполняется обновление в базе данных
                UpdateContactInDatabase(nameSearchTerm, newNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void UpdateContactInDatabase(string contactName, string newNumber)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE Контакты SET Номер = @NewNumber WHERE Имя_контакта = @ContactName";
                    cmd.Parameters.AddWithValue("@NewNumber", newNumber);
                    cmd.Parameters.AddWithValue("@ContactName", contactName);

                    cmd.ExecuteNonQuery();
                }
            }

            
        }

        private string GetOldNumber(string contactName)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT Номер FROM Контакты WHERE Имя_контакта = @ContactName";
                    cmd.Parameters.AddWithValue("@ContactName", contactName);

                    object result = cmd.ExecuteScalar();

                    // Проверка на null и преобразование в строку
                    return result != null ? result.ToString() : string.Empty;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string contactNumber = maskedTextBox1.Text.Trim();
            string newName = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(contactNumber) || string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Введите номер и новое имя для поиска контакта", "Предупреждение");
                return;
            }

            try
            {
                string oldName = GetOldName(contactNumber);  // Получение старого имени перед обновлением
                UpdateContactNameInDatabase(contactNumber, newName);

                // Вывод MessageBox с информацией о контакте
                MessageBox.Show($"Старое имя контакта: {oldName}\nНовое имя: {newName}\nНомер: {contactNumber}", "Детали контакта");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка");
            }
        }

        private string GetOldName(string contactNumber)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT Имя_контакта FROM Контакты WHERE Номер = @ContactNumber";
                    cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);

                    object result = cmd.ExecuteScalar();

                    // Проверка на null и преобразование в строку
                    return result != null ? result.ToString() : string.Empty;
                }
            }
        }

        private void UpdateContactNameInDatabase(string contactNumber, string newName)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE Контакты SET Имя_контакта = @NewName WHERE Номер = @ContactNumber";
                    cmd.Parameters.AddWithValue("@NewName", newName);
                    cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}










