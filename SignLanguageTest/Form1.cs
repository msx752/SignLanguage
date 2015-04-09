using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using WMPLib;

namespace _isaretDili1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<int, string> Listele = new Dictionary<int, string>();
        void Form1_Load(object sender, EventArgs e)
        {
            string xmlpath = Application.StartupPath + "\\kayitlilar.xml";
            List<KeyValuePair<int, string>> thisOrdered = new List<KeyValuePair<int, string>>();
            if (!File.Exists(xmlpath))
            {
                MessageBox.Show(@"gerekli videolar bulunamadı \Videos\ klasörünü kontrol ediniz.");
                #region internetten video güncellemesi
                //XmlTextWriter createXML = new XmlTextWriter(xmlpath, UTF8Encoding.UTF8);
                //createXML.WriteStartDocument();
                //createXML.WriteComment("İşaret Dili Programı: http://www.cmpe.boun.edu.tr/tid/?v=(VALUE)");
                //createXML.WriteStartElement("Kayitlar");
                //createXML.WriteEndDocument();
                //createXML.Close();

                //XmlDocument _data = new XmlDocument();
                //_data.Load(xmlpath);
                //foreach (var item in thisOrdered)
                //{
                //    XmlElement _kelime = _data.CreateElement("kelimeid");
                //    _kelime.SetAttribute(item.Key.ToString(), item.Value.ToLower());
                //    _data.DocumentElement.AppendChild(_kelime);
                //}
                //_data.Save(xmlpath);
                #endregion
            }
            else
            {
                XmlDocument _data = new XmlDocument();
                _data.Load(xmlpath);
                XmlNodeList yeniliste = _data["Kayitlar"].ChildNodes;
                foreach (XmlNode item in yeniliste)
                    Listele.Add(int.Parse(item.Attributes[0].Name.Substring(1)), item.Attributes[0].InnerText);
            }

            thisOrdered = Listele.OrderBy(p => p.Value).ToList();
            foreach (var item in thisOrdered)
            {
                listBox1.Items.Add(item.Value);
                comboBox1.Items.Add(item.Value);
                comboBox1.AutoCompleteCustomSource.Add(item.Value);
            }
            axWindowsMediaPlayer1.stretchToFit = true;
        }
        void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Kontrol(true);
        }

        void Kontrol(bool isListbx)
        {
            string myValues = "";
            if (isListbx)
                myValues = listBox1.SelectedItem.ToString();
            else
                myValues = comboBox1.SelectedItem.ToString();

            if (myValues == "")
                return;

            if (myValues != null)
            {
                int video_ID = Listele.Where(p => p.Value == myValues).ToList()[0].Key;
                string _path = Application.StartupPath + "\\videos\\" + video_ID + ".wmv";
                if (File.Exists(_path))
                {
                    IWMPMedia media = axWindowsMediaPlayer1.newMedia(_path);
                    axWindowsMediaPlayer1.currentPlaylist.clear();
                    axWindowsMediaPlayer1.currentPlaylist.appendItem(media);
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                    MessageBox.Show("dosya yok");
            }
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Kontrol(false);
        }


    }
}
