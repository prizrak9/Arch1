using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;

using Arch1;

namespace Arch1Test
{
    [TestClass]
    public class ComplexProxyTest
    {
        private ComplexProxy CreateComplexProxy()
        {
            return new ComplexProxy(new Complex(1, 1));
        }

        [TestMethod]
        public void GetReal()
        {
            dynamic obj = CreateComplexProxy();

            try
            {
                var real = obj.Real;
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void SetReal()
        {
            dynamic obj = CreateComplexProxy();

            Assert.ThrowsException<Exception>(() => obj.Real = 0);
        }

        [TestMethod]
        public void ToComplexForm()
        {
            dynamic obj = CreateComplexProxy();

            try
            {
                var real = obj.ToComplexForm();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void ToExponentialForm()
        {
            dynamic obj = CreateComplexProxy();

            try
            {
                var real = obj.ToExponentialForm(null);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
