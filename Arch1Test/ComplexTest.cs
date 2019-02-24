using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Arch1;

namespace Arch1Test
{
    [TestClass]
    public class ComplexTest
    {
        private Complex CreateComplex(double real, double imaginary)
        {
            return new Complex(real, imaginary);
        }

        
        [DataTestMethod]
        [DataRow(0, 0, "z=0+0i")]
        [DataRow(0, 1, "z=0+1i")]
        [DataRow(1, 0, "z=1+0i")]
        [DataRow(1, 1, "z=1+1i")]
        [DataRow(-1, 1, "z=-1+1i")]
        [DataRow(1, -1, "z=1-1i")]
        [DataRow(-1, -1, "z=-1-1i")]
        public void ToComplexForm(double real, double imaginary, string expected)
        {
            var obj = CreateComplex(real, imaginary);

            var actual = obj.ToComplexForm();

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow(0, 0, Form.Simple, "z=0.00e^NaN*i")]
        [DataRow(0, 1, Form.Simple, "z=1.00e^1.57*i")]
        [DataRow(1, 0, Form.Simple, "z=1.00e^6.28*i")]
        [DataRow(1, 1, Form.Simple, "z=1.41e^0.79*i")]
        [DataRow(-1, 1, Form.Simple, "z=1.41e^2.36*i")]
        [DataRow(1, -1, Form.Simple, "z=1.41e^5.50*i")]
        [DataRow(-1, -1, Form.Simple, "z=1.41e^3.93*i")]
        public void ToExponentialFormSimple(double real, double imaginary, Form form, string expected)
        {
            var obj = CreateComplex(real, imaginary);

            var actual = obj.ToExponentialForm(form);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow(0, 0, Form.Full, "z=0.00e^(NaN + 2*PI*n)*i")]
        [DataRow(0, 1, Form.Full, "z=1.00e^(1.57 + 2*PI*n)*i")]
        [DataRow(1, 0, Form.Full, "z=1.00e^(6.28 + 2*PI*n)*i")]
        [DataRow(1, 1, Form.Full, "z=1.41e^(0.79 + 2*PI*n)*i")]
        [DataRow(-1, 1, Form.Full, "z=1.41e^(2.36 + 2*PI*n)*i")]
        [DataRow(1, -1, Form.Full, "z=1.41e^(5.50 + 2*PI*n)*i")]
        [DataRow(-1, -1, Form.Full, "z=1.41e^(3.93 + 2*PI*n)*i")]
        public void ToExponentialFormFull(double real, double imaginary, Form form, string expected)
        {
            var obj = CreateComplex(real, imaginary);

            var actual = obj.ToExponentialForm(form);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, 0)]
        [DataRow(0, 1, 0, 1)]
        [DataRow(1, 0, 1, 0)]
        [DataRow(1, 1, 1, 1)]
        [DataRow(-1, 1, -1, 1)]
        [DataRow(1, -1, 1, -1)]
        [DataRow(-1, -1, -1, -1)]
        public void ComplexForm(double real, double imaginary, double expectedReal, double expectedImaginary)
        {
            var obj = CreateComplex(real, imaginary);

            var expected = (expectedReal, expectedImaginary);

            var actual = obj.ComplexForm;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow(0, 1, 1, 1.57)]
        [DataRow(1, 0, 1, 6.28)]
        [DataRow(1, 1, 1.41, 0.78)]
        [DataRow(-1, 1, 1.41, 2.35)]
        [DataRow(1, -1, 1.41, 5.49)]
        [DataRow(-1, -1, 1.41, 3.92)]
        public void ExponentialForm(double real, double imaginary, double expectedR, double expectedPower)
        {
            var obj = CreateComplex(real, imaginary);

            var (actualR, actualPower) = obj.ExponentialForm;

            Assert.AreEqual(expectedR, actualR, 1e-2);
            Assert.AreEqual(expectedPower, actualPower, 1e-2);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, double.NaN)]
        public void ExponentialFormReturnZeroNaNGivenZeros(double real, double imaginary, double expectedR, double expectedPower)
        {
            var obj = CreateComplex(real, imaginary);

            var (actualR, actualPower) = obj.ExponentialForm;

            Assert.AreEqual(expectedR, actualR, 1e-2);
            Assert.AreEqual(expectedPower, actualPower);
        }
    }
}
