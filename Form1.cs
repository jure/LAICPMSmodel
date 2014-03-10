using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class MainWindow : Form
    {
        // declaration of 'matrix1'
        int[,] matrix1;
        int scanRunning = 0;
        // static BackgroundWorker bw;
        //double[,] matrix2;
        // double[,] matrix3;
        // double[,] matrix4;
        bool imageLoaded = false; // if the image is loaded or not
        LAICPMS laicpms1;
        // ICPMS icpms1;
        // 1 pixel = 1 um x 1 um 
        Bitmap sampleImage;
        Bitmap resizedSampleImage;
        Bitmap finalPicture;
        OpenFileDialog open;
        System.Diagnostics.Stopwatch watch;
        List<float[]> colorMap;
        byte[] byteArray;
        double[,] finalMatrix;
        double[] rsdsCalc;
        bool firstOpen = true;

        public MainWindow()
        {
            InitializeComponent();
          // Adaptations for the review version:
          //  sampleImage = new Bitmap(WindowsFormsApplication1.Properties.Resources.Lincoln);
          //  pictureBox1.Image = sampleImage;
          //  imageLoaded = true;
          //  groupBox3.Text = "Theoretical sample";
          // End of Adaptations for the review version.
 
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    sampleImage = new Bitmap(open.FileName);
                    pictureBox1.Image = sampleImage;
                 // Commented out so the image will not be resized upon loading, only upon running the scan **FASTER**
                 //   if(!radioButton2.Checked)
                 //   {
                 //   resizedSampleImage = Utilities.ResizeBitmap(sampleImage, int.Parse(textBox7.Text), int.Parse(textBox8.Text));
                 //   pictureBox1.Image = resizedSampleImage;
                 //   }
                 //   else
                 //   {
                 // so it will be "just files" somewhere
                 //   }
                    imageLoaded = true;
                    groupBox3.Text = "Theoretical sample";
                }
            }
            catch (Exception)
            {
                imageLoaded = false;
                throw new ApplicationException("Failed loading image.");
            }

            


            
        }
       // public delegate void CalcDelegateWithParams(object sender, EventArgs e);

        private void abortScan(object sender, EventArgs e)
        {
                laicpms1.CancelAsync();
        }
        private void button2_Click(object sender, EventArgs e)
        {

            if (scanRunning == 0)
            {
                scanRunning = 1;

                button2.Text = "Abort scan";
                button2.BackColor = Color.Red;
                button2.ForeColor = Color.White;
                button2.Click -= this.button2_Click;
                button2.Click += new System.EventHandler(this.abortScan);

                // MethodInvoker calculationDelegate = new MethodInvoker(Calculate);
                // IAsyncResult tag = calculationDelegate.BeginInvoke(sender, e, null, null);

                //int allOk = 1;
                // Proceed with the scan only if there is an image (sample) loaded
                if (imageLoaded)
                {
                    // Measure execution time
                    watch = new System.Diagnostics.Stopwatch();
                    watch.Reset();
                    watch.Start();

                    // If dimensions are different from the original image and "just files" is not checked, resize the
                    //try
                    //{
                    if ((int)numericUpDown5.Value * (int)numericUpDown6.Value > 25000000)
                    {
                        if (MessageBox.Show("You are risking an out of memory exception! \n\n By clicking OK you will accept the risk and the model will run with the current parameters. \n\n By clicking Cancel the width and height paramerers will be set to 5000 by 5000 micrometers.", "Risk of running out of memory", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            numericUpDown5.Value = 5000;
                            numericUpDown6.Value = 5000;
                        }
                    }
                    if (!radioButton2.Checked && (((int)numericUpDown5.Value != sampleImage.Width) || ((int)numericUpDown6.Value != sampleImage.Height)))
                    {
                        try
                        {
                            resizedSampleImage = Utilities.ResizeBitmap(sampleImage, (int)numericUpDown5.Value, (int)numericUpDown6.Value);
                            pictureBox1.Image = resizedSampleImage;

                            // Convert to 2D array
                            matrix1 = Utilities.imageTo2DArray(ref resizedSampleImage);
                        }
                        catch (OutOfMemoryException ex)
                        {
                            MessageBox.Show(this, "Out of memory exception has occured during resizing of the image. \n 1. Try using smaller theoretical sample dimensions, \n 2. Try using faster scanning speeds, \n 3. Try using bigger beam diameters, \n 4. Try using lower frequencies \n 5. Try using bigger acquisition times. \n \n If nothing above works, set the settings to their default values.", "Out of Memory tips.");
                            scanRunning = 0;
                            button2.Text = "Run";
                            button2.BackColor = Color.Transparent;
                            button2.ForeColor = Color.Black;
                            button2.Click -= this.abortScan;
                            button2.Click += new System.EventHandler(this.button2_Click);
                            pictureBox1.Image = sampleImage;
                            pictureBox1.Show();
                        }

                    }
                    else
                    {
                        try
                        {
                            matrix1 = Utilities.imageTo2DArray(ref sampleImage);
                        }
                        catch (OutOfMemoryException ex)
                        {
                            MessageBox.Show(this, "Out of memory exception has occured during conversion of image to array. \n 1. Try using smaller theoretical sample dimensions, \n 2. Try using faster scanning speeds, \n 3. Try using bigger beam diameters, \n 4. Try using lower frequencies \n 5. Try using bigger acquisition times. \n \n If nothing above works, set the settings to their default values.", "Out of Memory tips.");
                            scanRunning = 0;
                            button2.Text = "Run";
                            button2.BackColor = Color.Transparent;
                            button2.ForeColor = Color.Black;
                            button2.Click -= this.abortScan;
                            button2.Click += new System.EventHandler(this.button2_Click);
                            pictureBox1.Image = sampleImage;
                            pictureBox1.Show();
                        }
                    }
                    if (scanRunning == 1)
                    {
                        if (radioButton10.Checked) // Convert to optical density
                        {
                            for (int i = 0; i < matrix1.GetLength(0); i++)
                            {
                                for (int j = 0; j < matrix1.GetLength(1); j++)
                                {
                                    if (matrix1[i, j] == 0) { matrix1[i, j] = 1; }
                                    matrix1[i, j] = (int)Math.Round(255.0 * (Math.Log10(255.0 / (double)matrix1[i, j]) / Math.Log10(255.0 / 1.0)));
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < matrix1.GetLength(0); i++)
                            {
                                for (int j = 0; j < matrix1.GetLength(1); j++)
                                {
                                    matrix1[i, j] = 255 - matrix1[i, j];
                                }
                            }
                        }

                        //}
                        //catch (OutOfMemoryException ex)
                        //{
                        //MessageBox.Show(this, "Out of memory exception has occured. Try using smaller theoretical sample dimensions.", "Out of memory");
                        //    allOk = 0;
                        //}
                        // If everything is ok and no exception has occured, then continue with the simulation
                        // Prepare the form for display of progress bar
                        pictureBox1.Hide();
                        Refresh();
                        label12.Visible = true;
                        progressBar1.Visible = true;
                        label13.Visible = true;

                        // Laser laser1 = new Laser(scanSpeed, laserBeamSize, laserFrequency, laserPower, chamberVolume, gasFlow, sampleMatrix);
                        int laserBeamSize = (int)numericUpDown1.Value;

                        // Validate laserBeamSize for round beam == must be divisible by 2.
                        if (radioButton6.Checked && (laserBeamSize % 2 != 0))
                        {
                            laserBeamSize = laserBeamSize + 1;
                        }

                        int lineSpacing = (int)numericUpDown2.Value;
                        int laserFrequency = (int)numericUpDown3.Value;
                        int scanSpeed = (int)numericUpDown4.Value;
                        int chamberVolume = (int)numericUpDown7.Value;
                        int gasFlow = (int)numericUpDown8.Value;
                        double ablationNoise = (double)numericUpDown10.Value;
                        double transferNoise = (double)numericUpDown12.Value;
                        double detectorNoise = (double)numericUpDown13.Value;

                        double acqTime = (double)numericUpDown9.Value;
                        bool beamShape = false;
                        if (radioButton6.Checked) beamShape = true;
                        bool thinSample = false;
                        bool thickSample = false;
                        if (radioButton11.Checked) { thinSample = true; thickSample = false; }
                        int numberOfShots = (int)numericUpDown11.Value;

                        if (radioButton7.Checked) { thickSample = true; thinSample = false; }

                        laicpms1 = new LAICPMS(scanSpeed, laserBeamSize, laserFrequency, lineSpacing, chamberVolume, gasFlow, matrix1, acqTime, ablationNoise, beamShape, thinSample, numberOfShots, thickSample, transferNoise, detectorNoise);
                        laicpms1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(laicpms1_RunWorkerCompleted);
                        laicpms1.ProgressChanged += new ProgressChangedEventHandler(laicpms1_ProgressChanged);

                        //laicpms1.ProgressChanged += laicpms1_ProgressChanged;
                        //laicpms1.RunWorkerCompleted += laicpms1_RunWorkerCompleted;

                        laicpms1.RunWorkerAsync("Hello!");
                    }
                }
                else {
                    MessageBox.Show("Sample image is not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    scanRunning = 0;
                    button2.Text = "Run";
                    button2.BackColor = Color.Transparent;
                    button2.ForeColor = Color.Black;
                    button2.Click -= this.abortScan;
                    button2.Click += new System.EventHandler(this.button2_Click);
                }
            }
        }

        private void laicpms1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label12.Text = e.UserState.ToString();
        }

        private void laicpms1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                scanRunning = 0;
                button2.Text = "Run";
                button2.BackColor = Color.Transparent;
                button2.ForeColor = Color.Black;
                button2.Click -= this.abortScan;
                button2.Click += new System.EventHandler(this.button2_Click);
                pictureBox1.Show();
                MessageBox.Show(this, "Out of memory exception has occured during simulation: \n 1. Try using smaller theoretical sample dimensions, \n 2. Try using faster scanning speeds, \n 3. Try using bigger beam diameters, \n 4. Try using lower frequencies \n 5. Try using bigger acquisition times. \n \n If nothing above works, set the settings to their default values.", "Out of memory");
            }
            else if (e.Cancelled)
            {
                scanRunning = 0;
                button2.Text = "Run";
                button2.BackColor = Color.Transparent;
                button2.ForeColor = Color.Black;
                button2.Click -= this.abortScan;
                button2.Click += new System.EventHandler(this.button2_Click);
                pictureBox1.Show();
            }
            else
            {
                scanRunning = 0;
                button2.Text = "Run";
                button2.BackColor = Color.Transparent;
                button2.ForeColor = Color.Black;
                button2.Click -= this.abortScan;
                button2.Click += new System.EventHandler(this.button2_Click);

                // Setting the color map
                colorMap = new List<float[]>();

                // If JET color map option is set, then load it into the program 
                if (radioButton3.Checked)
                {
                    // colorMap = Utilities.parseCSV(Application.StartupPath + "\\ColorMapJet.csv");
                    colorMap = Utilities.parseCSV(WindowsFormsApplication1.Properties.Resources.ColorMapJet, true);
                    
                }
                // If grayscale is selected, then set color map to null and calculate on the fly
                else
                {
                    colorMap = null;
                }

                
                finalMatrix = (double[,])e.Result; 
                byteArray = Utilities.ArrayFromMyArray(ref finalMatrix, ref colorMap);
                finalPicture = Utilities.BitmapFromArray(byteArray, finalMatrix);

                // Measured simulation time and calculated real experiment time

                pictureBox1.Image = Utilities.ResizeBitmap(finalPicture, pictureBox1.Width, pictureBox1.Height);
                groupBox3.Text = "Simulated measurement of the sample";
                pictureBox1.Visible = true;
                label12.Visible = false;
                progressBar1.Visible = false;
                label13.Visible = false;
                Refresh();

                int analysisTime = (matrix1.GetLength(1) / laicpms1.LaserBeamSize) * (matrix1.GetLength(0) / laicpms1.ScanSpeed);

                List<double> line = new List<double>();

                rsdsCalc = new double[finalMatrix.GetLength(0)];
                for (int i = 0; i < finalMatrix.GetLength(0); i++)
                {
                    line.Clear();

                    for (int j = 0; j < finalMatrix.GetLength(1); j++)
                    {
                        line.Add(finalMatrix[i,j]);
                    }
                    rsdsCalc[i] = Utilities.getSNR(line);
                }
                watch.Stop();
                TimeSpan executionTime = watch.Elapsed;
                TimeSpan experimentTime = new TimeSpan(0, 0, 0, analysisTime);
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", executionTime.Hours, executionTime.Minutes, executionTime.Seconds, executionTime.Milliseconds / 10);
                string calculatedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", experimentTime.Hours, experimentTime.Minutes, experimentTime.Seconds, experimentTime.Milliseconds / 10);

                toolStripStatusLabel1.Text = "Simulation duration: " + elapsedTime + ". Real experiment duration: " + calculatedTime + ".";

            }
        }

        //private void Calculate()
        //{
        //    finalPicture = null;
        //    int allOk = 1;
      
        //            // Here is where the magic happens, first the sum of concentrations under the area of a laser beam
        //            //label12.Text = "Simulating ... (1/3 - Sample ablation)";
        //            // Refresh();

        //            //try
        //            //{
        //            //    matrix2 = laicpms1.LaserAblation();
        //            //}
        //            //catch (OutOfMemoryException ex)
        //            //{
        //            //    MessageBox.Show(this, "Out of memory exception has occured. Try using smaller theoretical sample dimensions.", "Out of memory");
        //            //    allOk = 0;
        //            //    throw ex;
        //            //}

        //            // Then the backmixing of the gas in the ablation chamber
        //            // commented because everything is single function now (SampleAblation + SampleBackmix)
        //            //label12.Text = "Simulating ... (2/3 - Sample backmixing)";
        //            //Refresh();
        //            // matrix3 = laser1.SampleBackmix(ref matrix2, ref progressBar1, ref label13);
        //            // Then acquisition in the ICPMS
        //            if (allOk == 1)
        //            {
        //             //   label12.Text = "Simulating ... (3/3 - Acquisition)";
        //            //    Refresh();
                      
        //                // matrix4 = icpms1.Acquisition(ref matrix2, ref progressBar1, ref label12);


        //                // COMMENTED: Various intermediate matrices for debugging purposes
        //                // Utilities.saveMatrix(matrix2, Application.StartupPath + "\\Matrix3D.txt");
        //                // Utilities.saveMatrix(matrix3, Application.StartupPath + "\\Matrix2D.txt");
        //                // Utilities.saveMatrix(matrix4, Application.StartupPath + "\\MatrixFinal.txt");



                  
        //             }
        //}

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void exportMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog(); 
            saveFileDialog1.Title = "Specify Destination Filename";
            saveFileDialog1.Filter = "CSV files|*.csv";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.OverwritePrompt = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                
                Utilities.saveMatrix(finalMatrix, saveFileDialog1.FileName);
                toolStripStatusLabel1.Text = "Matrix saved as " + saveFileDialog1.FileName;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.Title = "Specify Destination Filename";
            saveFileDialog2.Filter = "TIFF files|*.tif";
            saveFileDialog2.FilterIndex = 1;
            saveFileDialog2.OverwritePrompt = true;

            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                finalPicture.Save(saveFileDialog2.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                toolStripStatusLabel1.Text = "Image saved as " + saveFileDialog2.FileName;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit?", "Exit", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void printPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Rectangle insideMargins = new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, e.MarginBounds.Width, e.MarginBounds.Height);
            System.Drawing.Point titlePos = new System.Drawing.Point(e.MarginBounds.X + 10, e.MarginBounds.Y + 10);
            String drawString = "LA ICP-MS simulation\n" + "Sample: " + open.FileName + "\n" + "Settings: " + "\n" + "Beam diameter: " + laicpms1.LaserBeamSize.ToString() + "um; Line spacing: " + laicpms1.LineSpacing.ToString() + "um; " + "\nRepetition rate: " + laicpms1.LaserFrequency.ToString() + " Hz; Scanning speed: " + laicpms1.ScanSpeed.ToString() + "um/s \nChamber volume: " + laicpms1.ChamberVolume.ToString() + "ml; Gas flow: " + laicpms1.GasFlow.ToString() + "ml/s Acquisiton time: " + laicpms1.AcqTime.ToString() + " s.";
            Font font = new Font("Arial",12);
            SolidBrush brush = new SolidBrush(Color.Black);
            e.Graphics.DrawString(drawString, font, brush, titlePos);
            System.Drawing.Point imagePos = new System.Drawing.Point(e.MarginBounds.X + 10, e.MarginBounds.Y + 300);

            //float hScale = (float)insideMargins.Width / pictureBox1.Image.Width;
            //float vScale = (float)insideMargins.Height / pictureBox1.Image.Height;
            //float scale = Math.Min(hScale, vScale);
            
            float scaledHeight = ((float)laicpms1.SampleSizeVer / (float)laicpms1.SampleSizeHor)* (insideMargins.Width - insideMargins.X);
            // Draws the bitmap on the print document
            e.Graphics.DrawImage(pictureBox1.Image,
                insideMargins.X, insideMargins.Y + 200,
                insideMargins.Width, scaledHeight);

          //  e.Graphics.DrawImage(pictureBox1.Image,imagePos);
         }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {

                printDocument1 = printDialog1.Document;
                printDocument1.Print();
            }

        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            NoiseSettings frm = null;
            
            if ((frm = (NoiseSettings)Utilities.IsFormAlreadyOpen(typeof(NoiseSettings))) == null)
            {

                frm = new NoiseSettings(rsdsCalc);
                frm.Show();
            }
            else
            {
                frm.Focus();
            }
            // NoiseSettings noiseSettings = new NoiseSettings(advancedNoiseSettings);
            //noiseSettings.Show();

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Jure Triglav, Johannes T. Van Elteren and Vid Šelih.\n\nLaboratory for Analytical Chemistry,\nNational Institute of Chemistry, Slovenia, \nHajdrihova 19, SI-1000 Ljubljana, Slovenia\n\nE-mail:elteren@ki.si", "About the authors", MessageBoxButtons.OK);
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {

        }

 
    }
}
