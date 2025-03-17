using System;
using System.Drawing;
using System.Windows.Forms;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

namespace qr_code
{
    public partial class Form1 : Form
    {
        private Color qrColor = Color.Black; // Цвет QR-кода
        private NumericUpDown sizeSelector; // Элемент управления для выбора размера

        public Form1()
        {
            InitializeComponent();
            // Инициализация элементов управления для выбора цвета и размера
            InitializeCustomControls();
        }

        private void InitializeCustomControls()
        {
            // Кнопка для выбора цвета


            // NumericUpDown для выбора размера
            sizeSelector = new NumericUpDown
            {
                Location = new Point(145, 500),
                Minimum = 1,
                Maximum = 100,
                Value = 80 // Значение по умолчанию
            };
            Controls.Add(sizeSelector);

            // Кнопка для генерации QR-кода

        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    qrColor = colorDialog.Color; // Устанавливаем выбранный цвет
                }
            }
        }

        private void GenerateQRCode()
        {
            try
            {
                string qrtext = textBox1.Text; // Считываем текст из TextBox'a

                if (string.IsNullOrWhiteSpace(qrtext))
                {
                    MessageBox.Show("Введите текст для генерации QR-кода.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                QRCodeEncoder encoder = new QRCodeEncoder();
                encoder.QRCodeForegroundColor = qrColor; // Устанавливаем цвет QR-кода
                encoder.QRCodeBackgroundColor = Color.White; // Устанавливаем цвет фона
                encoder.QRCodeScale = (int)sizeSelector.Value / 10; // Устанавливаем масштаб QR-кода

                Bitmap qrcode = encoder.Encode(qrtext); // Кодируем текст в переменную qrcode класса Bitmap
                pictureBox1.Image = qrcode as Image; // pictureBox выводит qrcode как изображение.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации QR-кода: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            GenerateQRCode(); // Генерируем QR-код при нажатии кнопки
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PNG|*.png|JPEG|*.jpg|GIF|*.gif|BMP|*.bmp";
            if (save.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(save.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog load = new OpenFileDialog();
            if (load.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = load.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                QRCodeDecoder decoder = new QRCodeDecoder();
                string decodedText = decoder.Decode(new QRCodeBitmapImage(pictureBox1.Image as Bitmap));
                MessageBox.Show(decodedText, "Раскодированный текст", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при декодировании QR-кода: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

