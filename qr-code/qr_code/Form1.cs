using System;
using System.Drawing;
using System.Windows.Forms;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

namespace qr_code
{
    public partial class Form1 : Form
    {
        private Color qrColor = Color.Black; 
        private NumericUpDown sizeSelector; 

        public Form1()
        {
            InitializeComponent();
            InitializeCustomControls();
        }
        private TextBox userTextBox;
        private void InitializeCustomControls()
        {
            userTextBox = new TextBox
            {
                Location = new Point(205, 550),
                Width = 120,
            };
            Controls.Add(userTextBox);

            sizeSelector = new NumericUpDown
            {
                Location = new Point(205, 500),
                Minimum = 1,
                Maximum = 100,
                Value = 80 // Значение по умолчанию
            };
            Controls.Add(sizeSelector);

        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    qrColor = colorDialog.Color; 
                }
            }
        }

        private DatabaseHelper dbHelper = new DatabaseHelper();
        private void GenerateQRCode()
        {
            try
            { 
            string qrtext = textBox1.Text; 
            string username = userTextBox.Text; 
                dbHelper.SaveQRCode(username, qrtext); 
                if (string.IsNullOrWhiteSpace(qrtext))
                {
                    MessageBox.Show("Введите текст для генерации QR-кода.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                QRCodeEncoder encoder = new QRCodeEncoder();
                encoder.QRCodeForegroundColor = qrColor; // 
                encoder.QRCodeBackgroundColor = Color.White; 
                encoder.QRCodeScale = (int)sizeSelector.Value / 10; 

                Bitmap qrcode = encoder.Encode(qrtext); 
                pictureBox1.Image = qrcode as Image; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации QR-кода: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            GenerateQRCode(); 
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

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

