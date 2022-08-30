using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    public static class AssertHelper
    {
        public static class MultiAssert
        {
            public static void Aggregate(params Action[] actions)
            {
                _aggregate(actions);
            }

            public static void Aggregate(IEnumerable<Action> actions)
            {
                _aggregate(actions);
            }

            private static void _aggregate(IEnumerable<Action> actions)
            {
                var exceptions = new List<AssertFailedException>();

                foreach (var action in actions)
                {
                    try
                    {
                        action();
                    }
                    catch (AssertFailedException ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                var assertionTexts =
                    exceptions.Select(assertFailedException => assertFailedException.Message);
                if (0 != assertionTexts.Count())
                {
                    throw new
                        AssertFailedException(
                        assertionTexts.Aggregate(
                            (aggregatedMessage, next) => aggregatedMessage + Environment.NewLine + next));
                }
            }
        }

        public static class DelayedAssert
        {
            public static void AddAreEqualAssert<T>(List<Action> assertList, T expected, T actual, string message, params object[] msgArgs)
            {
                assertList.Add(() => Assert.AreEqual(expected, actual, message, msgArgs));
            }

            public static void AddAreEqualAssert(List<Action> assertList, double expected, double actual, double delta, string message, params object[] msgArgs)
            {
                assertList.Add(() => Assert.AreEqual(expected, actual, delta, message, msgArgs));
            }

            public static void AddAreEqualAssert(List<Action> assertList, float expected, float actual, double delta, string message, params object[] msgArgs)
            {
                assertList.Add(() => Assert.AreEqual(expected, actual, delta, message, msgArgs));
            }

            public static void AddIsNotNullAssert<T>(List<Action> assertList, T actual, string message, params object[] msgArgs)
            {
                assertList.Add(() => Assert.IsNotNull(actual, message, msgArgs));
            }

            public static void AddIsNullAssert<T>(List<Action> assertList, T actual, string message, params object[] msgArgs)
            {
                assertList.Add(() => Assert.IsNull(actual, message, msgArgs));
            }

            public static void AddIsTrueAssert(List<Action> assertList, bool actual, string message, params object[] msgArgs)
            {
                assertList.Add(() => Assert.IsTrue(actual, message, msgArgs));
            }

            public static void AddIsFalseAssert(List<Action> assertList, bool actual, string message, params object[] msgArgs)
            {
                assertList.Add(() => Assert.IsFalse(actual, message, msgArgs));
            }

            public static void AddFailAssert(List<Action> assertList, string message, params object[] msgArgs)
            {
                assertList.Add(() => Assert.Fail(message, msgArgs));
            }

            public static void AddAreNotEqualAssert<T>(List<Action> asserts, T notExpected, T actual, string message, params object[] msgArgs)
            {
                asserts.Add(() => Assert.AreNotEqual(notExpected, actual, message, msgArgs));
            }

            public class Collection
            {
                public static void AddAreEqualAssert(List<Action> assertList, ICollection expected, ICollection actual, string message, params object[] msgArgs)
                {
                    assertList.Add(() => CollectionAssert.AreEqual(expected, actual, message, msgArgs));
                }
            }
        }
    }
}