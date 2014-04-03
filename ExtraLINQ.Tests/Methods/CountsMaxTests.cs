using System;
using System.Collections.Generic;
using System.Linq;
using ExtraLinq;
using FluentAssertions;
using NUnit.Framework;

namespace ExtraLINQ.Tests.Methods
{
    [TestFixture]
    public class CountsMaxTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void ThrowsArgumentNullExceptionWhenCollectionIsNull()
        {
            IEnumerable<object> nullCollection = null;

            nullCollection.CountsMax(1);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Test]
        public void ThrowsArgumentOutOfRangeExceptionWhenExpectedCountIsNegative()
        {
            IEnumerable<char> letters = "abcde";

            letters.CountsMax(-10);
        }

        [Test]
        public void ReturnsTrueWhenActualCountIsEqualToOrLowerThanExpectedCount()
        {
            IEnumerable<char> letters = "abcd";
            IEnumerable<char> emptyCollection = Enumerable.Empty<char>();

            letters.CountsMax(4).Should().BeTrue();
            letters.CountsMax(5).Should().BeTrue();
            emptyCollection.CountsMax(0).Should().BeTrue();

            // Test ICollection.Count early exit strategy
            letters.ToList().CountsMax(4).Should().BeTrue();
            letters.ToList().CountsMax(5).Should().BeTrue();
            emptyCollection.ToList().CountsMax(0).Should().BeTrue();
        }

        [Test]
        public void ReturnsFalseWhenActualCountIsGreaterThanExpectedMaxCount()
        {
            IEnumerable<char> letters = "abcd";

            letters.CountsMax(4).Should().BeTrue();
            letters.CountsMax(5).Should().BeTrue();

            // Test ICollection.Count early exit strategy
            letters.ToList().CountsMax(4).Should().BeTrue();
            letters.ToList().CountsMax(5).Should().BeTrue();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void ThrowsArgumentNullExceptionWhenCollectionIsNullWithPredicate()
        {
            IEnumerable<object> nullCollection = null;
            Func<object, bool> alwaysTruePredicate = _ => true;

            nullCollection.CountsMax(1, alwaysTruePredicate);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void ThrowsArgumentNullExceptionWhenPredicateIsNull()
        {
            IEnumerable<char> validCollection = "abcd";

            validCollection.CountsMax(1, null);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Test]
        public void ThrowsArgumentOutOfRangeExceptionWhenExpectedMaxCountIsNegative()
        {
            IEnumerable<char> validCollection = "abcd";
            Func<char, bool> validPredicate = c => c == 'a';

            validCollection.CountsMax(-1, validPredicate);
        }

        [Test]
        public void ReturnsTrueWhenActualCountIsEqualToOrLowerThanExpectedCountWithPredicate()
        {
            IEnumerable<string> fruits = new[] { "apple", "apricot", "banana" };
            Func<string, bool> startsWithLowercasedA = fruit => fruit.StartsWith("a");

            fruits.CountsMax(2, startsWithLowercasedA).Should().BeTrue();
            fruits.CountsMax(3, startsWithLowercasedA).Should().BeTrue();
            fruits.CountsMax(int.MaxValue, startsWithLowercasedA).Should().BeTrue();
        }

        [Test]
        public void ReturnsFalseWhenActualCountHigherThanExpectedCountWithPredicate()
        {
            IEnumerable<string> fruits = new[] { "apple", "apricot", "banana" };
            Func<string, bool> startsWithLowercasedA = fruit => fruit.StartsWith("a");

            fruits.CountsMax(0, startsWithLowercasedA).Should().BeFalse();
            fruits.CountsMax(1, startsWithLowercasedA).Should().BeFalse();
        }
    }
}
