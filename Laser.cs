using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using TestSimpleRNG;

public class Client
{
    public LAICPMS AblateSample(int scanSpeed, int laserBeamSize, int laserFrequency, int lineSpacing, int chamberVolume, int gasFlow, int[,] sampleMatrix, double acqTime, double ablationNoise, bool beamShape, bool thinSample, int numberOfShots, bool thickSample, double transferNoise, double detectorNoise)
    {
        return new LAICPMS(scanSpeed, laserBeamSize, laserFrequency, lineSpacing, chamberVolume, gasFlow, sampleMatrix, acqTime, ablationNoise, beamShape, thinSample, numberOfShots, thickSample, transferNoise, detectorNoise);
    }
}
    public class LAICPMS : System.ComponentModel.BackgroundWorker
    {
        // The full overloaded constructor of the class Laser,
        // You create a new class Laser by calling:
        // Laser laser1 = new Laser(scanSpeed, laserBeamSize, laserFrequency, lineSpacing, chamberVolume, gasFlow, sampleMatrix, acqTime, amountNoise, beamShape, thinSample, numberOfShots, thickSample, absoluteNoise);

        public LAICPMS()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true; 
        }

        public LAICPMS(int scanSpeed, int laserBeamSize, int laserFrequency, int lineSpacing, int chamberVolume, int gasFlow, int[,] sampleMatrix, double acqTime, double ablationNoise, bool beamShape, bool thinSample, int numberOfShots, bool thickSample, double transferNoise, double detectorNoise) : this()
        {
            ScanSpeed = scanSpeed;
            LaserBeamSize = laserBeamSize;
            LaserFrequency = laserFrequency;
            LineSpacing = lineSpacing;
            ChamberVolume = chamberVolume;
            GasFlow = gasFlow;
            AblationNoise = ablationNoise;
            SampleSizeHor = sampleMatrix.GetLength(1);
            SampleSizeVer = sampleMatrix.GetLength(0);
            AcqTime = acqTime;
            SampleMatrix = sampleMatrix;
            BeamShape = beamShape;
            CurrentCalc = 0;
            HowManyCalculations = 0;
            ThinSample = thinSample;
            ThickSample = thickSample;
            NumberOfShots = numberOfShots;
            TransferNoise = transferNoise;
            DetectorNoise = detectorNoise;
        }      

        public int[,] SampleMatrix
        {
            get
            {
                return _SampleMatrix;
            }
            set
            {
                _SampleMatrix = value;
            }
        }
        private int[,] _SampleMatrix;

        public int ScanSpeed
        {
            get
            {
                return _ScanSpeed;
            }
            set
            {
                _ScanSpeed = value;
            }
        }
        private int _ScanSpeed;

        public int LaserBeamSize
        {
            get
            {
                return _LaserBeamSize;
            }
            set
            {
                _LaserBeamSize = value;
            }
        }
        private int _LaserBeamSize;

        public int LaserFrequency
        {
            get
            {
                return _LaserFrequency;
            }
            set
            {
                _LaserFrequency = value;
            }
        }
        private int _LaserFrequency;

        public int LineSpacing
        {
            get
            {
                return _LineSpacing;
            }
            set
            {
                _LineSpacing = value;
            }
        }
        private int _LineSpacing;
        public int SampleSizeHor
        {
            get
            {
                return _SampleSizeHor;
            }
            set
            {
                _SampleSizeHor = value;
            }
        }
        private int _SampleSizeHor;
        public int SampleSizeVer
        {
            get
            {
                return _SampleSizeVer;
            }
            set
            {
                _SampleSizeVer = value;
            }
        }
        private int _SampleSizeVer;
        public int ChamberVolume
        {
            get
            {
                return _ChamberVolume;
            }
            set
            {
                _ChamberVolume = value;
            }
        }
        private int _ChamberVolume;
        public int GasFlow
        {
            get
            {
                return _GasFlow;
            }
            set
            {
                _GasFlow = value;
            }
        }
        private int _GasFlow;
        public double CurrentCalc
        {
            get
            {
                return _CurrentCalc;
            }
            set
            {
                _CurrentCalc = value;
            }
        }
        private double _CurrentCalc;
        public double HowManyCalculations
        {
            get
            {
                return _HowManyCalculations;
            }
            set
            {
                _HowManyCalculations = value;
            }
        }
        private double _HowManyCalculations;

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
        public double AblationNoise
        {
            get
            {
                return _AblationNoise;
            }
            set
            {
                _AblationNoise = value;
            }
        }
        private double _AblationNoise;
        public double TransferNoise
        {
            get
            {
                return _TransferNoise;
            }
            set
            {
                _TransferNoise = value;
            }
        }
        private double _TransferNoise;
        public double DetectorNoise
        {
            get
            {
                return _DetectorNoise;
            }
            set
            {
                _DetectorNoise = value;
            }
        }
        private double _DetectorNoise;

        public bool BeamShape
        {
            get
            {
                return _BeamShape;
            }
            set
            {
                _BeamShape = value;
            }
        }
        private bool _BeamShape;

        public bool ThinSample
        {
            get
            {
                return _ThinSample;
            }
            set
            {
                _ThinSample = value;
            }
        }
        private bool _ThinSample;
        public bool ThickSample
        {
            get
            {
                return _ThickSample;
            }
            set
            {
                _ThickSample = value;
            }
        }
        private bool _ThickSample;

        public int NumberOfShots
        {
            get
            {
                return _NumberOfShots;
            }
            set
            {
                _NumberOfShots = value;
            }
        }
        private int _NumberOfShots;

        public bool AbsoluteNoise
        {
            get
            {
                return _AbsoluteNoise;
            }
            set
            {
                _AbsoluteNoise = value;
            }
        }
        private bool _AbsoluteNoise;

        // This function calculates the area fractions of a circle within a squary matrix.

        public double[,] CalcCircleSquareAreaFractions()
        {
            double[,] fractions = new double[LaserBeamSize, LaserBeamSize];
            double y1, x1, y2, x2;
            int y1inside, x1inside, y2inside, x2inside = 0;
            double r = LaserBeamSize/2;
            double area;
            double integralPartB;
            double integralPartA;
            double bottomSquare;
            double insideSquare;

            for (int ydim = LaserBeamSize/2 - 1; ydim >= 0; ydim--)
            {
                for (int xdim = 0; xdim < (LaserBeamSize/2); xdim++)
                {
                    // Find out where the points in the circle are 
                    // and if there is a corresponding x or y point to a y and x point respectively.
                    
                    if (Math.Pow(r, 2) - Math.Pow(xdim - r, 2) >= 0)
                    {
                        y1 = Math.Sqrt(Math.Pow(r, 2) - Math.Pow(xdim - r, 2));
                    }
                    else
                    {
                        y1 = -9999;
                    }
                    if (Math.Pow(r, 2) - Math.Pow(xdim + 1 - r, 2) >= 0)
                    {
                        y2 = Math.Sqrt(Math.Pow(r, 2) - Math.Pow(xdim + 1 - r, 2));
                    }
                    else
                    {
                        y2 = -9999;
                    }

                    if (Math.Pow(r, 2) - Math.Pow(ydim, 2) >= 0)
                    {
                        x1 = Math.Sqrt(Math.Pow(r, 2) - Math.Pow(ydim, 2)) + r;
                        x1 = 2 * r - x1;
                    }
                    else
                    {
                        x1 = -9999;
                    }
                    if (Math.Pow(r, 2) - Math.Pow(ydim + 1, 2) >= 0)
                    {
                        x2 = Math.Sqrt(Math.Pow(r, 2) - Math.Pow(ydim + 1, 2)) + r;
                        x2 = 2 * r - x2;
                    }
                    else
                    {
                        x2 = -9999;
                    }

                    // Find out if the particular points are inside the square or outside of it.

                    if(y1 >= ydim && y1 <= ydim+1 && y1 != -9999 ) y1inside = 1;
                    else y1inside = 0;
                    if (y2 >= ydim && y2 <= ydim + 1 && y2 != -9999) y2inside = 1;
                    else y2inside = 0;
                    if (x1 >= xdim && x1 <= xdim + 1 && x1 != -9999) x1inside = 1;
                    else x1inside = 0;
                    if (x2 >= xdim && x2 <= xdim + 1 && x2 != -9999) x2inside = 1;
                    else x2inside = 0;

                    if (x1inside == 0 && x2inside == 1 && y1inside == 1 && y2inside == 0)
                    {
                        // Curve from left boundary to top boundary
                        integralPartA = (-1.0 / 4.0) * (-2 * xdim + 2 * r) * Math.Sqrt((-Math.Pow(xdim, 2) + 2 * xdim * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((xdim - r) / Math.Sqrt(-Math.Pow(xdim, 2) + 2 * xdim * r));
                        integralPartB = (-1.0 / 4.0) * (-2 * x2 + 2 * r) * Math.Sqrt((-Math.Pow(x2, 2) + 2 * x2 * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((x2 - r) / Math.Sqrt(-Math.Pow(x2, 2) + 2 * x2 * r)); 

                        bottomSquare = (x2 - xdim) * ydim;
                        insideSquare = (xdim + 1 - x2) * 1;

                        area = integralPartB - integralPartA - bottomSquare + insideSquare;
                        
                    }
                    else if (x1inside == 1 && x2inside == 0 && y1inside == 0 && y2inside == 1)
                    {
                        // Curve from bottom boundary to right boundary
                        integralPartA = (-1.0 / 4.0) * (-2 * x1 + 2 * r) * Math.Sqrt((-Math.Pow(x1, 2) + 2 * x1 * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((x1 - r) / Math.Sqrt(-Math.Pow(x1, 2) + 2 * x1 * r));
                        integralPartB = (-1.0 / 4.0) * (-2 * (xdim + 1) + 2 * r) * Math.Sqrt((-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan(((xdim + 1) - r) / Math.Sqrt(-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r));

                        bottomSquare = ((xdim+1)-x1) * ydim;

                        area = integralPartB - integralPartA - bottomSquare;
                    }
                    else if (x1inside == 1 && x2inside == 1 && y1inside == 0 && y2inside == 0)
                    {
                        // Curve from bottom boundary to top boundary
                        integralPartA = (-1.0 / 4.0) * (-2 * x1 + 2 * r) * Math.Sqrt((-Math.Pow(x1, 2) + 2 * x1 * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((x1 - r) / Math.Sqrt(-Math.Pow(x1, 2) + 2 * x1 * r));
                        integralPartB = (-1.0 / 4.0) * (-2 * x2 + 2 * r) * Math.Sqrt((-Math.Pow(x2, 2) + 2 * x2 * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((x2 - r) / Math.Sqrt(-Math.Pow(x2, 2) + 2 * x2 * r));
                        bottomSquare = (x2 - x1) * ydim;
                        insideSquare = (xdim + 1 - x2) * 1;

                        area = integralPartB - integralPartA - bottomSquare + insideSquare;
                    }
                    else if (x1inside == 0 && x2inside == 0 && y1inside == 1 && y2inside == 1)
                    {
                        // Curve from left boundary to right boundary
                        integralPartA = (-1.0 / 4.0) * (-2 * xdim + 2 * r) * Math.Sqrt((-Math.Pow(xdim, 2) + 2 * xdim * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((xdim - r) / Math.Sqrt(-Math.Pow(xdim, 2) + 2 * xdim * r));
                        integralPartB = (-1.0 / 4.0) * (-2 * (xdim + 1) + 2 * r) * Math.Sqrt((-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan(((xdim + 1) - r) / Math.Sqrt(-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r));
                        bottomSquare = 1 * ydim;

                        area = integralPartB - integralPartA - bottomSquare;

                    }
                    else if (x1inside == 1 && x2inside == 1 && y1inside == 1 && y2inside == 1)
                    {
                        // Curve from left bottom corner to top right corner
                        integralPartA = (-1.0 / 4.0) * (-2 * x1 + 2 * r) * Math.Sqrt((-Math.Pow(x1, 2) + 2 * x1 * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((x1 - r) / Math.Sqrt(-Math.Pow(x1, 2) + 2 * x1 * r));
                        integralPartB = (-1.0 / 4.0) * (-2 * x2 + 2 * r) * Math.Sqrt((-Math.Pow(x2, 2) + 2 * x2 * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((x2 - r) / Math.Sqrt(-Math.Pow(x2, 2) + 2 * x2 * r));
                        bottomSquare = 1 * ydim;

                        area = integralPartB - integralPartA - bottomSquare;
                    }
                    else if (x1inside == 1 && x2inside == 0 && y1inside == 1 && y2inside == 1)
                    {
                        // Curve from left bottom corner to right boundary
                        integralPartA = (-1.0 / 4.0) * (-2 * xdim + 2 * r) * Math.Sqrt((-Math.Pow(xdim, 2) + 2 * xdim * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((xdim - r) / Math.Sqrt(-Math.Pow(xdim, 2) + 2 * xdim * r));
                        integralPartB = (-1.0 / 4.0) * (-2 * (xdim+1) + 2 * r) * Math.Sqrt((-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan(((xdim + 1) - r) / Math.Sqrt(-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r));
                        bottomSquare = 1 * ydim;
                        area = integralPartB - integralPartA - bottomSquare;
                    }
                    else if (x1inside == 1 && x2inside == 1 && y1inside == 0 && y2inside == 1)
                    {
                        // Curve from bottom boundary to top right corner
                        integralPartA = (-1.0 / 4.0) * (-2 * x1 + 2 * r) * Math.Sqrt((-Math.Pow(x1, 2) + 2 * x1 * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((x1 - r) / Math.Sqrt(-Math.Pow(x1, 2) + 2 * x1 * r));
                        integralPartB = (-1.0 / 4.0) * (-2 * (xdim+1) + 2 * r) * Math.Sqrt((-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan(((xdim + 1) - r) / Math.Sqrt(-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r));
                        bottomSquare = (xdim+1 - x1) * ydim;

                        area = integralPartB - integralPartA - bottomSquare;
                    }
                    else if (x1inside == 0 && x2inside == 1 && y1inside == 1 && y2inside == 1)
                    {
                        // Curve from left boundary to top right corner
                        integralPartA = (-1.0 / 4.0) * (-2 * xdim + 2 * r) * Math.Sqrt((-Math.Pow(xdim, 2) + 2 * xdim * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((xdim - r) / Math.Sqrt(-Math.Pow(xdim, 2) + 2 * xdim * r));
                        integralPartB = (-1.0 / 4.0) * (-2 * (xdim+1) + 2 * r) * Math.Sqrt((-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan(((xdim + 1) - r) / Math.Sqrt(-Math.Pow((xdim + 1), 2) + 2 * (xdim + 1) * r));
                        bottomSquare = 1 * ydim;

                        area = integralPartB - integralPartA - bottomSquare;
                    }
                    else if (x1inside == 1 && x2inside == 1 && y1inside == 1 && y2inside == 0)
                    {
                        // Curve from left boundary to top right corner
                        integralPartA = (-1.0 / 4.0) * (-2 * xdim + 2 * r) * Math.Sqrt((-Math.Pow(xdim, 2) + 2 * xdim * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((xdim - r) / Math.Sqrt(-Math.Pow(xdim, 2) + 2 * xdim * r));
                        integralPartB = (-1.0 / 4.0) * (-2 * x2 + 2 * r) * Math.Sqrt((-Math.Pow(x2, 2) + 2 * x2 * r)) + (1.0 / 2.0) * Math.Pow(r, 2) * Math.Atan((x2 - r) / Math.Sqrt(-Math.Pow(x2, 2) + 2 * x2 * r));
                        bottomSquare = 1 * ydim;
                        insideSquare = (xdim + 1 - x2) * 1;
                        area = integralPartB - integralPartA - bottomSquare + insideSquare;
                    }
                    else 
                    {
                        // Curve on the edge or higher
                        if ((y1 >= ydim + 1) && (y2 >= ydim + 1))
                        {
                            area = 1;
                        }
                        // Curve lower
                        else
                        {
                            area = 0;
                        }
                    }
                    

                    // Assign fraction values 
                    fractions[LaserBeamSize / 2 - (ydim + 1), xdim] = area;
                    fractions[LaserBeamSize / 2 - (ydim + 1), LaserBeamSize - (xdim + 1)] = area;
                    fractions[LaserBeamSize - (LaserBeamSize / 2 - ydim), xdim] = area;
                    fractions[LaserBeamSize - (LaserBeamSize / 2 - ydim), LaserBeamSize - (xdim + 1)] = area;
                }
            }
            return fractions;
        }
        
        protected override void OnDoWork(System.ComponentModel.DoWorkEventArgs e)
        {
            int matrixHorizontalSize = 0;
            double currentHorizontalPos = 0;
            int counter = 0;

            // Get Circle/Square fractions needed for round laser beam
            double[,] fractions = CalcCircleSquareAreaFractions();
            
            while (currentHorizontalPos < SampleSizeHor)
            {
                currentHorizontalPos = LaserBeamSize + (counter * ScanSpeed) / LaserFrequency;
                counter++;
            }

            int[,] SampleOriginal = new int[SampleMatrix.GetLength(0), SampleMatrix.GetLength(1)];
            SampleOriginal = SampleMatrix;

            matrixHorizontalSize = counter;

            double[,] ablatedSampleMatrix = new double
                            [(int)Math.Ceiling((double)SampleSizeVer / LineSpacing),         // the vertical sample size divided by the LaserBeamSize
                            matrixHorizontalSize // the horizontal sample size divided by ScanSpeed and multiplied by LaserFrequency, to get the number of pulses in a line
                             ];                                                               // ** removed, i can calculate time if i need it; 1 for the resulting sum, and 1 for time

            double horLocation = 0; // the horizontal, x, location of the laser beam (within a line)
            int verLocation = 0; // the vertical, y, location of the laser beam (lines)
            int laserPulseNumber = 0; // will be used to show which the current pulse is, within a single line scen
            int lineNumber = 0; // will be used to show which the current line is
            double localSum = 0; // initiation and declaration

            // GasFlow * 0.01 is there because of 10 ms time precision
            double ratio = (double)GasFlow * 0.01 / (double)ChamberVolume;
            // now to sum up these exponential fall-out values in time, so we get a representation
            // of how the concentration of ablated material/element changes in the gas with time
            int maxTime = 0;
            //Determine the approxiate washout time of the model, so we don't waste calculations
            int washoutTime = (int)Math.Round(6.746 * Math.Pow(ratio * 100, -1.0661) * 100);
            
            // If ratio is for example 1, (meaning 1 ml chamber, 100 ml/s gas flow), to keep the AUC for
            // integrate A*e^((-t)*1) from 0 to (6.746*((1*100)^(-1.0661))*100)
            // A, ergo peak, should be the ablated matrix value.
            // If the ratio is 0.5, the peak should be twice the ablated matrix value.

            double peakHeightFactor = ratio * 100;

            double exp = Math.Exp(1);

            int timeInCentiseconds = (int)Math.Floor((double)SampleSizeHor * 100 / ScanSpeed);  // how much time it takes for one line to complete

            double[,] finalBackmixedMatrix = new double[ablatedSampleMatrix.GetLength(0),
                                                        timeInCentiseconds];



            // Calculate the number of required calculations, so we can display the percent done in the
            // progress bar.

            for (int i = 0; i < (ablatedSampleMatrix.GetLength(0)); i++)
            {
                for (int j = 0; j < (ablatedSampleMatrix.GetLength(1)); j++)
                {
                    int offsetTime = (int)Math.Floor(((double)j / (double)LaserFrequency) * 100);
                    if ((offsetTime + washoutTime) < timeInCentiseconds) maxTime = offsetTime + washoutTime;
                    else maxTime = timeInCentiseconds;

                    HowManyCalculations += maxTime - offsetTime;
                }
            }

            HowManyCalculations += ablatedSampleMatrix.GetLength(1) * ablatedSampleMatrix.GetLength(0);
 
            // Simulate ablation (without backmixing)

            //// Noise
            Random random = new Random();

            SimpleRNG.SetSeedFromSystemTime();

            RNGCryptoServiceProvider secureRandom = new RNGCryptoServiceProvider();
            byte[] randBytes = new byte[4];
            double RSD = 3.009 * Math.Exp(-0.2293 * LaserBeamSize) + 0.0568; 
            
            /* Derived from MATLAB function fitting  
               a =       3.009  (0.9428, 5.076)
               b =      0.2293  (0.1363, 0.3223)
               c =      0.0568  (0.00159, 0.112)
            */
            while ((verLocation + LaserBeamSize) <= SampleSizeVer) // whole sample
            {
                if (CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    while ((horLocation + LaserBeamSize) <= SampleSizeHor) // one line
                    {
                        localSum = 0;

                        // this double for loop sums up the values within LaserBeamSize (down and right) of horLocation and verLocation
                        int LBS = LaserBeamSize;
                        for (int i = 0; i < LBS; i++)
                        {
                            for (int j = 0; j < LBS; j++)
                            {
                                // Square beam
                                if (BeamShape == false)
                                {
                                    // Summation of values in the area of the matrix, that the laser ablates
                                    localSum = localSum + SampleMatrix[i + verLocation, j + (int)Math.Floor(horLocation)];

                                    // Ablate the thin sample, so that nothing remains in the impact site
                                    if (ThinSample)
                                    {
                                        SampleMatrix[i + verLocation, j + (int)Math.Floor(horLocation)] = 0;
                                        
                                    }
                                    // Ablate a certain fraction
                                    else if (!ThinSample && !ThickSample)
                                    {
                                        SampleMatrix[i + verLocation, j + (int)Math.Floor(horLocation)] -= (int)Math.Round(SampleOriginal[i + verLocation, j + (int)Math.Floor(horLocation)] * (1 / (double)NumberOfShots));
                                    }
                                }
                                // Round beam. Multiply border areas with calculated fractions.
                                else if (BeamShape == true)
                                {
                                    // Summation of values in the area of the matrix, that the laser ablates
                                    localSum = localSum + SampleMatrix[i + verLocation, j + (int)Math.Floor(horLocation)]*fractions[i,j];
                                    
                                    // Ablate the thin sample, so that nothing remains in the impact site
                                    if (ThinSample)
                                    {
                                        SampleMatrix[i + verLocation, j + (int)Math.Floor(horLocation)] -= (int)Math.Round(SampleMatrix[i + verLocation, j + (int)Math.Floor(horLocation)] * fractions[i, j]);
                                    }
                                    else if (!ThinSample && !ThickSample)
                                    {
                                        SampleMatrix[i + verLocation, j + (int)Math.Floor(horLocation)] -= (int)Math.Round(SampleOriginal[i + verLocation, j + (int)Math.Floor(horLocation)] * fractions[i, j] * (1/(double)NumberOfShots));
                                    }
                                }

                             }
                        }
                        // Introduce ablation noise                       

                        secureRandom.GetBytes(randBytes);
                        double currentRandom = Math.Abs((double)BitConverter.ToInt32(randBytes,0));

                        double noise2 = 0;
                        double noise = 0;
                        // noise = AblationNoise * localSum * ((random.NextDouble() - 0.5) * 2);
                        if (AblationNoise != 0)
                        {
                            noise = localSum * SimpleRNG.GetNormal(0, AblationNoise);
                            noise2 = SimpleRNG.GetNormal(0, AblationNoise);
                        }
                        ablatedSampleMatrix[lineNumber, laserPulseNumber] = Math.Abs(noise2 + localSum);

                        // resultingMatrix[lineNumber, laserPulseNumber, 1] = (int)Math.Floor((double)laserPulseNumber * 100/ LaserFrequency); // time at which the laser beam pulse had happened (in 1/100 seconds)   
                        laserPulseNumber++;

                        CurrentCalc++;

                        horLocation = ScanSpeed * laserPulseNumber * (1 / (double)LaserFrequency);
                        // the next horizontal location equals the current one + how much the laser moves in one pulse

                    }
                }
                // the next vertical location is the current one + the width of the laser (LaserBeamSize); moves down one width to the next line
                ReportProgress((int)(CurrentCalc*100 / HowManyCalculations), "Simulating ... (1/3 - Sample ablation)");


                lineNumber++;
                laserPulseNumber = 0;
                horLocation = 0;
                verLocation = verLocation + LineSpacing;
              //  progressBar.Value = (int)(100 * ((double)verLocation / (double)SampleSizeVer));
              //  progressBar.Update();
            }

           // commented because we are putting everything in a single function
           // return ablatedSampleMatrix;
           // } 

        // this is an inherent physical property of the LA (and ICP-MS?) machine, not its technical function

     //   public double[,] SampleBackmix(ref double[,] ablatedSampleMatrix, ref System.Windows.Forms.ProgressBar progressBar, ref System.Windows.Forms.Label timeRemaining) 
     //   {


            //Report second step - sample backmixing
           // progressReport.Text = "Simulating ... (2/3 - Sample backmixing)";
           // form.Refresh();


            
            for (int i = 0; i < (ablatedSampleMatrix.GetLength(0)); i++)
            {
                if (CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    for (int j = 0; j < (ablatedSampleMatrix.GetLength(1)); j++)
                    {
                        int offsetTime = (int)Math.Ceiling(((double)j / (double)LaserFrequency) * 100);
                        if ((offsetTime + washoutTime) < timeInCentiseconds) maxTime = offsetTime + washoutTime;
                        else maxTime = timeInCentiseconds;

                        for (int k = offsetTime; k < maxTime; k++)
                        {
                            // so it starts the curve at the right point,*100 time precision
                            double noise = 0;
                            noise = TransferNoise * ((random.NextDouble() - 0.5) * 2);
                            finalBackmixedMatrix[i, k] += Math.Abs(noise + (ablatedSampleMatrix[i, j] * peakHeightFactor * Math.Pow(exp, (-(k - offsetTime)) * ratio)));

                        }
                        CurrentCalc += maxTime - offsetTime;

                    }
                }
                ReportProgress((int)(CurrentCalc * 100/ HowManyCalculations), "Simulating ... (2/3 - Sample backmixing)");
            }

            double[,] ICPMSMatrix = new double[finalBackmixedMatrix.GetLength(0),
                                             (int)Math.Ceiling(finalBackmixedMatrix.GetLength(1) / (AcqTime * 100))]; // time precision ( the * 100 part)

            // Detector Noise generation
  //          Random detectorNoise = new Random();
  //          double strength = AmountNoise * 1 / (AdvancedNoiseSettings[10] * AdvancedNoiseSettings[11]);
            
            // Complete noise generation
            Random holisticNoise = new Random();

            // ICP-MS Acquisition
            for (int i = 0; i < ICPMSMatrix.GetLength(0); i++)
            {
                int j = 0; // raw data counter
                int k = 0; // final data counter
                double localSum1 = 0;
                
                
                while (j < finalBackmixedMatrix.GetLength(1))
                {
                    // adds up all the raw data within an acquisition time
                //  localSum1 += finalBackmixedMatrix[i, j] + strength*detectorNoise.NextDouble();

                    // Trapezoidal rule of approximating integration
                    if ((j + 1 < finalBackmixedMatrix.GetLength(1)) && ((j + 1)%(AcqTime*100)!=0))
                    {
                        localSum1 += ((finalBackmixedMatrix[i, j] + finalBackmixedMatrix[i, j + 1]) / 2) * 0.01;
                    }
                    else
                    {
                        localSum1 += finalBackmixedMatrix[i, j] * 0.01; 
                    }
                    j++;

                    // writes the data to the final array
                    if ((j % (AcqTime * 100) == 0) || j == finalBackmixedMatrix.GetLength(1)) // time precision (the * 100 part)
                    {
                        double noise = DetectorNoise * ((random.NextDouble() - 0.5) * 2);
                        //ICPMSMatrix[i, k] = localSum1 + holisticNoise.NextDouble()*AmountNoise ;
                        //ICPMSMatrix[i, k] = localSum1;
                        ICPMSMatrix[i, k] = Math.Abs(noise + localSum1);
                        localSum1 = 0;
                        k++;
                    }
                }


            }

            ReportProgress((int)(CurrentCalc * 100/ HowManyCalculations), "Simulating ... (3/3 - ICPMS acquisition)");

            e.Result = ICPMSMatrix;
        }



    }

