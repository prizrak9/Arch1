using static System.Math;
using System.Reflection;


namespace Arch1
{

    enum Form { Simple, Full }

    class Complex : ComplexBase, IComplex
    {
        public (double real, double imaginary) ComplexForm => (real, imaginary);

        public (double r, double power) ExponentialForm => (Magnitude, Angle);



        public double Magnitude => Sqrt(Pow(real, 2) + Pow(imaginary, 2));

        public double Angle => GetAngle();


        // Proxies test
        public double Real
        {
            get => real;
            set => this.GetType().GetField("real", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, value);
        }
        public double Imaginary { get => imaginary; set => this.GetType().GetField("imaginary").SetValue(this, value); }





        public Complex(double real, double imaginary) 
            : base(real, imaginary)
        {
        }

        [My]
        private double GetAngle()
        {
            double at = Atan(Abs(imaginary) / Abs(real));
            return real > 0 ? (imaginary > 0 ? at : 2 * PI - at) : (imaginary > 0 ? PI - at : 3 * PI / 2 - at);
        }
        
        [My]
        public string ToComplexForm()
        {
            return $"z={real}+{imaginary}i";
        }

        [My]
        public string ToExponentialForm(Form form = Form.Simple)
        {
            if (form == Form.Full)
            {
                return $"z={ExponentialForm.r:n}e^({ExponentialForm.power:n} + 2*PI*n)*i";
            }
            else
            {
                return $"z={ExponentialForm.r:n}e^{ExponentialForm.power:n}*i";
            }
        }

        [My]
        public override string ToString()
        {
            return ToComplexForm();
        }
    }
}
