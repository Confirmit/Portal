﻿using System.Collections.Generic;
using System.Linq;

namespace TestConsoleExecutorRules.Factory
{
    public class UserFactory
    {
        public List<int> GetUserIdForMoscow()
        {
            var result = new[] { 198 };

            return result.ToList();
        }

        public List<int> GetUserIdForNotifyByTime()
        {
            var result = new[] { 198 };

            return result.ToList();
        }

        public List<int> GetUserIdForNotifyLastUser()
        {
            var result = new[] { 198 };

            return result.ToList();
        } 
    }
}