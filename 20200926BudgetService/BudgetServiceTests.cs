using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace _20200926BudgetService
{
    public class BudgetServiceTests
    {
        private IBudgetRepo _budgetRepo;
        private BudgetService _budgetService;
        private DateTime _endDate;
        private DateTime _startDate;

        [SetUp]
        public void SetUp()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
            _budgetService = new BudgetService(_budgetRepo);
        }

        // 查整日
        [Test]
        public void QueryEntireDay()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget {YearMonth = "202001", Amount = 3100}
            });

            GivenDatePeriod(new DateTime(2020, 1, 1), new DateTime(2020, 1, 1));

            ShouldBe(100);
        }

        // 查整月
        [Test]
        public void QueryEntireMonth()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget {YearMonth = "202001", Amount = 3100}
            });

            GivenDatePeriod(new DateTime(2020, 1, 1), new DateTime(2020, 1, 31));

            ShouldBe(3100);
        }

        private void ShouldBe(int expected)
        {
            Assert.AreEqual(expected, _budgetService.Query(_startDate, _endDate));
        }

        private void GivenBudgets(List<Budget> budgets)
        {
            _budgetRepo.GetAll().Returns(budgets);
        }

        private void GivenDatePeriod(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        // 2個整月
        [Test]
        public void QueryTwoMonth()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget {YearMonth = "202001", Amount = 3100},
                new Budget {YearMonth = "202002", Amount = 2900}
            });
            GivenDatePeriod(new DateTime(2020, 1, 1), new DateTime(2020, 2, 29));

            ShouldBe(6000);
        }


        // 月區間
        [Test]
        public void IntervalMonth()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget {YearMonth = "202001", Amount = 3100}
            });
            GivenDatePeriod(new DateTime(2020, 1, 1), new DateTime(2020, 1, 12));

            ShouldBe(1200);
        }

        // 跨月
        [Test]
        public void CrossTwoMonth()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget {YearMonth = "202001", Amount = 3100},
                new Budget {YearMonth = "202002", Amount = 290}
            });
            GivenDatePeriod(new DateTime(2020, 1, 30), new DateTime(2020, 2, 3));

            ShouldBe(230);
        }

        // 跨3個月
        [Test]
        public void CrossThreeMonth()
        {
            GivenBudgets(new List<Budget>
            {
                new Budget {YearMonth = "202001", Amount = 3100},
                new Budget {YearMonth = "202002", Amount = 290},
                new Budget {YearMonth = "202003", Amount = 31}
            });
            GivenDatePeriod(new DateTime(2020, 1, 30), new DateTime(2020, 3, 1));

            ShouldBe(491);
        }

        // 非法起迄
        [Test]
        public void InvalidDate()
        {
            GivenDatePeriod(new DateTime(2020, 1, 13), new DateTime(2020, 1, 1));

            ShouldBe(0);
        }

        // 無資料
        [Test]
        public void NoData()
        {
            GivenBudgets(new List<Budget>());

            GivenDatePeriod(new DateTime(2020, 1, 1), new DateTime(2020, 1, 1));

            ShouldBe(0);
        }
    }
}