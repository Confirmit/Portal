using System;
using System.Collections.Generic;
using System.Text;
using Core.Security;
using log4net.Util;
using log4net.Core;

namespace Core.Log
{
    /// <summary>
    /// ������������ �������� full name � ��� �����.
    /// </summary>
    public class FullNamePatternConverter : PatternConverter
    {
        public FullNamePatternConverter()
        {
            
        }

        protected override void Convert(System.IO.TextWriter writer, object state)
        {
            try
            {
                writer.Write(User.Current.ADUser.Name);
            }
            catch (Exception)
            {
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }

    /// <summary>
    /// ������������ �������� message_en � ���������� ���������.
    /// </summary>
    public class EnglishMessagePatternConverter : PatternConverter
    {
        public EnglishMessagePatternConverter()
        {

        }

        protected override void Convert(System.IO.TextWriter writer, object state)
        {
            try
            {
                if (state is LoggingEvent)
                {
                    LoggingEvent logEvent = (LoggingEvent)state;

                    if (logEvent.MessageObject is MLString)
                    {
						writer.Write( ((MLString)logEvent.MessageObject).EnglishValue );
                    }
                    else
                    {
						writer.Write(logEvent.MessageObject.ToString());
                    }

                }
                else
                {
                    writer.Write(SystemInfo.NotAvailableText);
                }
            }
            catch (Exception)
            {
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }

    /// <summary>
    /// ������������ �������� message_ru � ������� ���������.
    /// </summary>
    public class RussianMessagePatternConverter : PatternConverter
    {
        public RussianMessagePatternConverter()
        {

        }

        protected override void Convert(System.IO.TextWriter writer, object state)
        {
            try
            {
                if (state is LoggingEvent)
                {
                    LoggingEvent logEvent = (LoggingEvent)state;

                    if (logEvent.MessageObject is MLString)
                    {
						writer.Write( ((MLString)logEvent.MessageObject).RussianValue );
                    }
                    else
                    {
						writer.Write(logEvent.MessageObject.ToString());
                    }
                }
                else
                {
                    writer.Write(SystemInfo.NotAvailableText);
                }
            }
            catch (Exception)
            {
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }
}
