using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class ICPMS
    {
        public ICPMS(double acqTime)
        {
            AcqTime = acqTime; 
        }

        public double AcqTime
        {
            get
            {
                return _AcqTime;
            }
            set
            {
                _AcqTime = value;
            }
        }
        private double _AcqTime;

        public double[,] Acquisition(ref double[,] backmixedMatrix, ref System.Windows.Forms.ProgressBar progressBar, ref System.Windows.Forms.Label timeRemaining)
        {
            double[,] ICPMSMatrix = new double[backmixedMatrix.GetLength(0),
                                               (int)Math.Ceiling(backmixedMatrix.GetLength(1) / (AcqTime*100))]; // time precision ( the * 100 part)

            for (int i = 0; i < ICPMSMatrix.GetLength(0); i++)
            {
                int j = 0; // raw data counter
                int k = 0; // final data counter
                double localSum = 0;
                while (j < backmixedMatrix.GetLength(1))
                {
                    // adds up all the raw data within an acquisition time
                    localSum += backmixedMatrix[i, j];
                    j++;

                    // writes the data to the final array
                    if (j % (AcqTime*100) == 0) // time precision (the * 100 part)
                    {
                        ICPMSMatrix[i, k] = localSum;
                        localSum = 0;
                        k++;
                    }
                }

              
            }

            return ICPMSMatrix;
        }

    }
}
