using System;
using NUnit.Framework;

namespace _20200926BudgetService
{
    public class BudgetServiceTests
    {
        private BudgetService _budgetService;

        [SetUp]
        public void SetUp()
        {
            _budgetService = new BudgetService();
        }

        // 非法起迄
        [Test]
        public void InvalidDate()
        {
            var start = new DateTime(2020, 9, 5);
            var end = new DateTime(2020, 1, 1);
            var actual = _budgetService.Query(start, end);

            Assert.AreEqual(0, actual);
        }

        // 無資料
        [Test]
        public void NoData()
        {
        }

        // 查整月
        [Test]
        public void QueryEntireMonth()
        {
        }

        // 2個整月
        [Test]
        public void QueryTwoMonth()
        {
        }

        // 查整日
        [Test]
        public void QueryEntireDay()
        {
        }

        // 月區間
        [Test]
        public void IntervalMonth()
        {
        }

        // 跨月
        [Test]
        public void CrossTwoMonth()
        {
        }

        // 跨3個月
        [Test]
        public void CrossThreeMonth()
        {
        }
    }
}