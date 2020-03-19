using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestProject.Exceptions;

namespace UnitTestProject
{
    
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Test_Eq()
        {
            6.Expect().Eq(6);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectationFailedException))]
        public void Test_Equal_Fails()
        {
            6.Expect().Eq(5);
        }

        [TestMethod]
        public void Test_Greater()
        {
            10.Expect().IsGreater(6);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectationFailedException))]
        public void Test_Greater_Fails()
        {
            (-10).Expect().IsGreater(4);
        }

        [TestMethod]
        public void Test_Not_Equal()
        {
            10.Expect().Not().Eq(9);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectationFailedException))]
        public void Test_Not_Equal_Fails()
        {
            10.Expect().Not().Eq(10);
        }

        [TestMethod]
        public void Test_Raise_Error()
        {
            new Action(() => { throw new Exception(); }).Expect().RaiseError();
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectationFailedException))]
        public void Test_Raise_Error_Fails()
        {
            (new Action(() => FizzBuzz.SampleMethod())).Expect().RaiseError();
        }

        [TestMethod]
         public void Test_Properties_Equality()
        {
            var tested = new FizzBuzz() { Buzz = "Buzz", Fiz = String.Empty };
            var expected = new { Buzz = "Buzz", Fiz = String.Empty };

            tested.Expect().Properties().Eq(expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectationFailedException))]
        public void Test_Properties_Equality_Fails()
        {
            var tested = new FizzBuzz() { Buzz = "Buzz", Fiz = String.Empty };
            var expected = new { Buzz = "Buzz2", Fiz = String.Empty };
            tested.Expect().Properties().Eq(expected);
        }

        // Changed method name due to duplication below
        [TestMethod]
        public void Test_PropertiesWithoutLambdaArgument()
        {
            var tested = new FizzBuzz() { Buzz = "Buzz", Fiz = "Fiz" };
            var expected = new { Buzz = "Buzz", Fiz = "Fiz2" };
            tested.Expect().PropertiesWithout(x => x.Fiz).Eq(expected);
        }

        // Changed method name due to duplication below
        [TestMethod]
        [ExpectedException(typeof(ExpectationFailedException))]
        public void Test_PropertiesWithoutLambdaArgument_Fails()
        {
            var tested = new FizzBuzz() { Buzz = "Buzz", Fiz = "Fiz" };
            var expected = new { Buzz = "Bar2", Fiz = "Fiz2" };
            tested.Expect().PropertiesWithout(x => x.Fiz).Eq(expected);
        }

        [TestMethod]
        public void Test_PropertiesWithout()
        {
            var tested = new FizzBuzz() { Buzz = "Buzz", Fiz = "Fiz" };
            var expected = new { Buzz = "Buzz", Fiz = "Fiz2" };
            tested.Expect().PropertiesWithout("Fiz").Eq(expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectationFailedException))]
        public void Test_PropertiesWithout_Fails()
        {
            var tested = new FizzBuzz() { Buzz = "Buzz", Fiz = "Fiz" };
            var expected = new { Buzz = "Buzz2", Fiz = "Fiz2" };
            tested.Expect().PropertiesWithout("Fiz").Eq(expected);
        }
    }

    public class FizzBuzz
    {
        public string Buzz { get; set; }
        public string Fiz { get; set; }

        public static int SampleMethod()
        {
            return 2 + 2;
        }
    }
}
