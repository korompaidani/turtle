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
        public void XYImmutability()
        {
            var list = new List<SmartPoint> { new SmartPoint(5, 9) };

            Assert.AreEqual(list[0], new SmartPoint(5, 9));

            var getListFirst = list[0];
            getListFirst.ClockWise();
            getListFirst.SetSouth();
            getListFirst.Move();

            Assert.AreEqual(list[0], new SmartPoint(5, 9));
        }

        [Test]
        public void OutOfRangeTest()
        {
            var range = new SmartPoint(7, 3);
            var testPoint0 = new SmartPoint(3, 7);
            var testPoint1 = new SmartPoint(7, 4);
            var testPoint2 = new SmartPoint(8, 3);
            var testPoint3 = new SmartPoint(99, 500000);
            var testPoint4 = new SmartPoint(-1, -1);

            Assert.IsFalse(testPoint0.IsPointInRange(range));
            Assert.IsFalse(testPoint1.IsPointInRange(range));
            Assert.IsFalse(testPoint2.IsPointInRange(range));
            Assert.IsFalse(testPoint3.IsPointInRange(range));
            Assert.IsFalse(testPoint4.IsPointInRange(range));
        }

        [Test]
        public void InRangeTest()
        {
            var range = new SmartPoint(7, 3);
            var testPoint0 = new SmartPoint(7, 3);
            var testPoint1 = new SmartPoint(1, 2);
            var testPoint2 = new SmartPoint(2, 1);
            var testPoint3 = new SmartPoint(1, 0);
            var testPoint4 = new SmartPoint(0, 1);
            var testPoint5 = new SmartPoint(0, 0);

            Assert.IsTrue(testPoint0.IsPointInRange(range));
            Assert.IsTrue(testPoint1.IsPointInRange(range));
            Assert.IsTrue(testPoint2.IsPointInRange(range));
            Assert.IsTrue(testPoint3.IsPointInRange(range));
            Assert.IsTrue(testPoint4.IsPointInRange(range));
            Assert.IsTrue(testPoint5.IsPointInRange(range));
        }
    }
}
