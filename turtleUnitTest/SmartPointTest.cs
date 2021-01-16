using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using turtle;

namespace turtleUnitTest
{
    class SmartPointTest
    {
        [Test]
        public void EqualsAndGetHashCodeTest()
        {
            var dictionary = new Dictionary<SmartPoint, string>();
            dictionary.Add(new SmartPoint(5, 4), "");

            dictionary[new SmartPoint(new Point(5, 4))] = "expected";

            Assert.IsTrue(dictionary.Count == 1);
            Assert.AreEqual(dictionary[new SmartPoint(5, 4)], "expected");
            Assert.Throws<ArgumentException>(() =>
                dictionary.Add(new SmartPoint(5, 4), "IamSureCannotAdd"));
        }

        [Test]
        public void XYStateLess()
        {
            var list = new List<SmartPoint> { new SmartPoint(5, 9) };

            Assert.AreEqual(list[0], new SmartPoint(5, 9));

            var getListFirst = list[0];
            getListFirst.ClockWise();
            getListFirst.SetSouth();
            getListFirst.Move();

            Assert.AreEqual(list[0], new SmartPoint(5, 9));
        }
    }
}
