﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOGL
{
    public partial class CanvasForm : Form
    {
        //CanvasForm przyjmuje w konstruktorze:
        // - wskaznik do portu seryjnego serialPort, 
        // - nazwe pliku, 
        // - oraz wskaznik do tabcontrol
        private TabControl __ownerTab;
        private string __fileName;
        private SerialPort __serialPort;


        public CanvasForm(SerialPort serialPort, string fileName, TabControl ownerTab)
        {
            InitializeComponent();

            //Zdefiniuj argumenty
            __serialPort = serialPort;
            __ownerTab = ownerTab;
            __fileName = fileName;

            //Wczytaj podglad obrazu
            CanvasPicture canvasPicture = new CanvasPicture(fileName)
            {
                TopLevel = false,
                TopMost = true
            };

            //Dodaj die do panelShowcase i wyswietl
            panelShowcase.Controls.Add(canvasPicture);
            canvasPicture.Show();
        }

        private void goClose_Click(object sender, EventArgs e)
        {
            //Kasuj zakladke z obrazem oraz zamnkij te okno
            __ownerTab.TabPages.Remove(__ownerTab.SelectedTab);
            this.Close();
        }

        private void goGCode_Click(object sender, EventArgs e)
        {
            /*
             * 
             * Work in progress <-------------------------------------------------------------------------------
             * 
             */
            GCodeCreator codeCreator = new GCodeCreator(
                    __fileName,
                    GCodeCreator.EngravingDirection.Horizontal,
                    1500,
                    0,
                    400
                );
            //GCodeGenerator codeCreator = new GCodeGenerator(
            //        100,
            //        100,
            //        1,
            //        1,
            //        1500,
            //        true
            //    );
            List<string> gcode = codeCreator.toGCode();
            //gcode.ForEach(delegate (String line)
            //{
            //    Debug.WriteLine(line);
            //});
            TabPage loadGcodeTab = new TabPage(Path.GetFileNameWithoutExtension(__fileName) + " (Converted to GCode)");

            GCodeForm codeForm = new GCodeForm(__serialPort, __ownerTab, __fileName, gcode)
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            //Dodaj do nowej zakladki canvasForm
            loadGcodeTab.Controls.Add(codeForm);
            //Dodaj nowa zakladke do TabControl
            __ownerTab.Controls.Add(loadGcodeTab);
            //Wybierz ta zakladke
            __ownerTab.SelectedTab = loadGcodeTab;
            //Pokaz
            codeForm.Show();
        }
    }
}
