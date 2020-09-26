using System;
using System.Collections.Generic;
using System.Linq;

namespace _20200926BudgetService
{
    public class BudgetService
    {
        private readonly IBudgetRepo _repo;

        public BudgetService(IBudgetRepo repo)
        {
            _repo = repo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (start > end || !_repo.GetAll().Any())
            {
                return 0;
            }

            decimal amount;
            if (IsSameDay(start, end))
            {
                amount = (decimal) _repo.GetAll().FirstOrDefault(x => x.YearMonth == start.ToString("yyyyMM"))?.Amount;
                return amount / GetDays(start);
            }

            if (IsSameMonth(start, end))
            {
                amount = (decimal) _repo.GetAll().FirstOrDefault(x => x.YearMonth == start.ToString("yyyyMM"))?.Amount;
                return amount / GetDays(start) * ((end - start).Days + 1);
            }


            return GetStartMonthBudget(start) +
                   GetSecondMonthBudget(end) +
                   GetEntireMonthBudget(start, end);
        }

        private static int GetEndDatePortion(DateTime end)
        {
            return end.Day;
        }

        private static int GetStartDatePortion(DateTime start)
        {
            return GetDays(start) - start.Day + 1;
        }

        private decimal GetEntireMonthBudget(DateTime start, DateTime end)
        {
            return _repo.GetAll().Where(m =>
                    int.Parse(m.YearMonth) >= int.Parse(start.AddMonths(1).ToString("yyyyMM")) &&
                    int.Parse(m.YearMonth) <= int.Parse(end.AddMonths(-1).ToString("yyyyMM")))
                .Sum(m => m.Amount);
        }

        private decimal GetSecondMonthBudget(DateTime end)
        {
            return (decimal) _repo.GetAll().FirstOrDefault(x => x.YearMonth == end.ToString("yyyyMM"))?.Amount /
                GetDays(end) * GetEndDatePortion(end);
        }

        private decimal GetStartMonthBudget(DateTime start)
        {
            return (decimal) _repo.GetAll().FirstOrDefault(x => x.YearMonth == start.ToString("yyyyMM"))?.Amount /
                GetDays(start) * GetStartDatePortion(start);
        }

        private static bool IsSameMonth(DateTime start, DateTime end)
        {
            return start.ToString("yyyyMM") == end.ToString("yyyyMM");
        }

        private static int GetDays(DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        private static bool IsSameDay(DateTime start, DateTime end)
        {
            return start == end;
        }
    }


    public interface IBudgetRepo
    {
        IEnumerable<Budget> GetAll();
    }

    public class BudgetRepo : IBudgetRepo
    {
        public IEnumerable<Budget> GetAll()
        {
            throw new NotImplementedException();
        }
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public decimal Amount { get; set; }
    }
}