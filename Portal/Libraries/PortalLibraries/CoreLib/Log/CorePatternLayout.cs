using System;
using System.Collections.Generic;
using System.Text;
using log4net.Appender;

namespace Core.Log
{
    /// <summary>
    /// Layout. Добавляет новый конвертер для параметра fullname.
    /// </summary>
    public class CoreFullNamePatternLayout : log4net.Layout.PatternLayout
    {
        public CoreFullNamePatternLayout()
        {
            this.AddConverter("fullname", typeof(FullNamePatternConverter));
        }                
    }

    /// <summary>
    /// Layout. Добавляет новый конвертер для параметра message_en.
    /// </summary>
    public class CoreEnglishMessagePatternLayout : log4net.Layout.PatternLayout
    {
        public CoreEnglishMessagePatternLayout()
        {
            this.AddConverter("message_en", typeof(EnglishMessagePatternConverter));
        }
    }

    /// <summary>
    /// Layout. Добавляет новый конвертер для параметра message_ru.
    /// </summary>
    public class CoreRussianMessagePatternLayout : log4net.Layout.PatternLayout
    {
        public CoreRussianMessagePatternLayout()
        {
            this.AddConverter("message_ru", typeof(RussianMessagePatternConverter));
        }
    }
}
