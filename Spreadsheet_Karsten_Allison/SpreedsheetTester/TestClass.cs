// <copyright file="TestClass.cs" company="Karsten Allison 011783232">
// Copyright (c) Karsten Allison 011783232. All rights reserved.
// </copyright>

using System;
using CptS321;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spreadsheet_Karsten_Allison;
using SpreadsheetEngine;

namespace SpreedsheetTester
{
    /// <summary>
    /// Test class for checking if spreadsheet properties work.
    /// </summary>
    [TestClass]
    public class TestClass
    {
        /// <summary>
        /// Generates a 4*4 spreadsheet instance.
        /// </summary>
        private readonly Spreadsheet spreadSheetTest = new Spreadsheet(4, 4);

        /// <summary>
        /// Tests if RowCount/ColumnCount are properly passed.
        /// </summary>
        [TestMethod]
        public void TestMethodA()
        {
            Assert.AreEqual(4, this.spreadSheetTest.RowCount);
            Assert.AreEqual(4, this.spreadSheetTest.ColumnCount);
        }

        /// <summary>
        /// Tests simple expressions inside ExpressionTree instances.
        /// </summary>
        [TestMethod]
        public void ExpressionTreeTestNormal()
        {
            // normal cases
            ExpressionTree expNormalA = new ExpressionTree("3+3");
            ExpressionTree expNormalB = new ExpressionTree("3-3");
            ExpressionTree expNormalC = new ExpressionTree("3*3");
            ExpressionTree expNormalD = new ExpressionTree("3/3");
            ExpressionTree expNormalE = new ExpressionTree("3+4+5+6");
            ExpressionTree expNormalF = new ExpressionTree("3*3*2*1*6");
            ExpressionTree expNormalG = new ExpressionTree("120/10/2/6");

            Assert.AreEqual(6.0, expNormalA.Evaluate());
            Assert.AreEqual(0.0, expNormalB.Evaluate());
            Assert.AreEqual(9.0, expNormalC.Evaluate());
            Assert.AreEqual(1.0, expNormalD.Evaluate());
            Assert.AreEqual(18.0, expNormalE.Evaluate());
            Assert.AreEqual(108.0, expNormalF.Evaluate());
            Assert.AreEqual(1.0, expNormalG.Evaluate());
        }

        /// <summary>
        /// Tests boundry cases (div/mult into 0) inside ExpressionTree instances.
        /// </summary>
        [TestMethod]
        public void ExpressionTreeTestBoundry()
        {
            // boundry cases
            ExpressionTree expBoundryA = new ExpressionTree("0/5");
            ExpressionTree expBoundryB = new ExpressionTree("0*5");

            Assert.AreEqual(0, expBoundryA.Evaluate());
            Assert.AreEqual(0, expBoundryB.Evaluate());
        }

        /// <summary>
        /// Tests Edge cases (divide by 0, or unset variables) inside ExpressionTree instances.
        /// </summary>
        [TestMethod]
        public void ExpressionTreeTestEdge()
        {
            // edge cases
            ExpressionTree expEdgeA = new ExpressionTree("5/0");
            ExpressionTree expEdgeB = new ExpressionTree("string");
            ExpressionTree expEdgeC = new ExpressionTree("(string)");
            ExpressionTree expEdgeD = new ExpressionTree("(string)+1");
            expEdgeD.SetVariable("string", 1);

            Assert.AreEqual(double.PositiveInfinity, expEdgeA.Evaluate());
            Assert.AreEqual(0, expEdgeB.Evaluate());
            Assert.AreEqual(0, expEdgeC.Evaluate());
            Assert.AreEqual(2, expEdgeD.Evaluate());
        }

        /// <summary>
        /// Tests Parenthesis shunting inside ExpressionTree instances.
        /// </summary>
        [TestMethod]
        public void ExpressionTreeParenTest()
        {
            // Normal Parenthesis cases.
            ExpressionTree expParenthesisA = new ExpressionTree("(3+3)+3");
            ExpressionTree expParenthesisB = new ExpressionTree("(3-3)+(2+3)");
            ExpressionTree expParenthesisC = new ExpressionTree("(3+3)*3");
            ExpressionTree expParenthesisD = new ExpressionTree("(3-3)*(2+3)");
            ExpressionTree expParenthesisE = new ExpressionTree("((3-3)*(2+3))");
            ExpressionTree expParenthesisF = new ExpressionTree("(3-3)/(2+3)");
            ExpressionTree expParenthesisG = new ExpressionTree("(rabbit*(2+3))");
            expParenthesisG.SetVariable("rabbit", 2);

            Assert.AreEqual(9, expParenthesisA.Evaluate());
            Assert.AreEqual(5, expParenthesisB.Evaluate());
            Assert.AreEqual(18, expParenthesisC.Evaluate());
            Assert.AreEqual(0, expParenthesisD.Evaluate());
            Assert.AreEqual(0, expParenthesisE.Evaluate());
            Assert.AreEqual(0, expParenthesisF.Evaluate());
            Assert.AreEqual(10, expParenthesisG.Evaluate());
        }

        /// <summary>
        /// Tests Order of operations on expressions.
        /// </summary>
        [TestMethod]
        public void ExpressionTreeOrderTest()
        {
            // Normal Parenthesis cases.
            ExpressionTree expParenthesisA = new ExpressionTree("3+3/3");
            ExpressionTree expParenthesisB = new ExpressionTree("3-3*2+3");
            ExpressionTree expParenthesisC = new ExpressionTree("3+3*3");
            ExpressionTree expParenthesisE = new ExpressionTree("3*2+3");

            Assert.AreEqual(3 + (3 / 3), expParenthesisA.Evaluate());
            Assert.AreEqual(3 - (3 * 2) + 3, expParenthesisB.Evaluate());
            Assert.AreEqual(3 + (3 * 3), expParenthesisC.Evaluate());
            Assert.AreEqual((3 * 2) + 3, expParenthesisE.Evaluate());
        }
    }
}
