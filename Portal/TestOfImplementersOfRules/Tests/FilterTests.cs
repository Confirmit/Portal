using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestOfImplementersOfRules.Factories;
using TestOfImplementersOfRules.Factories.FilterFactories;
using TestOfImplementersOfRules.Factories.TimeEntityFactories;

namespace TestOfImplementersOfRules.Tests
{
    [TestClass]
    public class FilterTests
    {
        [TestMethod]
        public void DayOfWeekFilter_IsNeccessaryToExecute_ResultShouldBeTrue()
        {
            var filter = new RuleFilterFactory().GetDayOfWeekFilter();
            var daysOfWeek = new HashSet<DayOfWeek>
            {
                DayOfWeek.Friday,
                DayOfWeek.Monday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday,
                DayOfWeek.Thursday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday
            };
            var timeEntity = new TimeEntityFactory().GetConfiguratedTimeEntity(default(TimeSpan), default(TimeSpan),
                daysOfWeek, default(DateTime), default(DateTime));

            var rule = new RuleFactory().GetNotReportToMoscowRules(new List<TimeEntity>{timeEntity}).Single();

            Assert.IsTrue(filter.IsNeccessaryToExecute(rule, DateTime.Now));
        }

        [TestMethod]
        public void DayOfWeekFilter_IsNeccessaryToExecute_ResultShouldBeFalse()
        {
            var filter = new RuleFilterFactory().GetDayOfWeekFilter();
            var daysOfWeek = new HashSet<DayOfWeek>();
           
            var timeEntity = new TimeEntityFactory().GetConfiguratedTimeEntity(default(TimeSpan), default(TimeSpan),
                daysOfWeek, default(DateTime), default(DateTime));

            var rule = new RuleFactory().GetNotReportToMoscowRules(new List<TimeEntity> { timeEntity }).Single();

            Assert.IsFalse(filter.IsNeccessaryToExecute(rule, DateTime.Now));
        }

        [TestMethod]
        public void ActiveTimeFilter_IsNeccessaryToExecute_ResultShouldBeTrue()
        {
            var filter = new RuleFilterFactory().GetActiveTimeFilter();

            var beginTime = DateTime.Now.AddDays(-1);
            var endTime = DateTime.MaxValue;

            var timeEntity = new TimeEntityFactory().GetConfiguratedTimeEntity(default(TimeSpan), default(TimeSpan),
                new HashSet<DayOfWeek>(), beginTime, endTime);

            var rule = new RuleFactory().GetNotReportToMoscowRules(new List<TimeEntity> { timeEntity }).Single();

            Assert.IsTrue(filter.IsNeccessaryToExecute(rule, DateTime.Now));
        }

        [TestMethod]
        public void ActiveTimeFilter_IsNeccessaryToExecute_ResultShouldBeFalse()
        {
            var filter = new RuleFilterFactory().GetActiveTimeFilter();
            var daysOfWeek = new HashSet<DayOfWeek>();

            var beginTime = DateTime.MinValue;
            var endTime = beginTime.AddMilliseconds(1);

            var timeEntity = new TimeEntityFactory().GetConfiguratedTimeEntity(default(TimeSpan), default(TimeSpan),
                daysOfWeek, beginTime, endTime);

            var rule = new RuleFactory().GetNotReportToMoscowRules(new List<TimeEntity> { timeEntity }).Single();

            Assert.IsFalse(filter.IsNeccessaryToExecute(rule, DateTime.Now));
        }

        [TestMethod]
        public void ExperationTimeFilter_IsNeccessaryToExecute_ResultShouldBeTrue()
        {
            var filter = new RuleFilterFactory().GetExperationTimeFilter();
            
            var experationTime = new TimeSpan(9999,0,0,0);
            var launchTime = new TimeSpan(0);

            var timeEntity = new TimeEntityFactory().GetConfiguratedTimeEntity(experationTime, launchTime,
                new HashSet<DayOfWeek>(), default(DateTime), default(DateTime));

            var rule = new RuleFactory().GetNotReportToMoscowRules(new List<TimeEntity> { timeEntity }).Single();

            Assert.IsTrue(filter.IsNeccessaryToExecute(rule, DateTime.Now));
        }

        [TestMethod]
        public void ExperationTimeFilter_IsNeccessaryToExecute_ResultShouldBeFalse()
        {
            var filter = new RuleFilterFactory().GetExperationTimeFilter();
            var daysOfWeek = new HashSet<DayOfWeek>();

            var experationTime = new TimeSpan(0);
            var launchTime = new TimeSpan(0);

            var timeEntity = new TimeEntityFactory().GetConfiguratedTimeEntity(experationTime, launchTime,
                daysOfWeek, default(DateTime), default(DateTime));

            var rule = new RuleFactory().GetNotReportToMoscowRules(new List<TimeEntity> { timeEntity }).Single();

            Assert.IsFalse(filter.IsNeccessaryToExecute(rule, DateTime.Now.AddDays(-1)));
        }
    }
}