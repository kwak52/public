using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLCConvertor.Forms
{
    public partial class FormScrollableImage : Form
    {
        Image _image;
        public FormScrollableImage(Image image)
        {
            InitializeComponent();
            _image = image;
        }

        private void FormScrollableImage_Load(object sender, EventArgs e)
        {
            panel1.AutoScroll = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.Image = _image;
        }
    }
}
