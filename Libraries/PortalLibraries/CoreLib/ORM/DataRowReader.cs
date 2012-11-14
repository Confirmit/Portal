using System;
using System.Data;

namespace Core.ORM
{
    public static class DataRowReader
    {
        #region [ Методы Read{Type}Value() для чтения значения из записи БД. ]

        /// <summary>
        /// Читает и возвращает из строки значение MLString-поля (два значения - русское  и английское).
        /// Имя поля с русским значением долно заканчиваться на _ru, а с английским - на _en.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static MLString ReadMLStringValue(DataRow row, string fieldName)
        {
            return new MLString(
                row[fieldName + ObjectMapper.RussianEnding] != DBNull.Value ? (string)row[fieldName + ObjectMapper.RussianEnding] : string.Empty,
                row[fieldName + ObjectMapper.EnglishEnding] != DBNull.Value ? (string)row[fieldName + ObjectMapper.EnglishEnding] : string.Empty
                );
        }

        /// <summary>
        /// Читает и возвращает из строки значение String-поля. Для NULL-значений используется значение NullValue.NullString.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string ReadStringValue(DataRow row, string fieldName)
        {
            //return ReadStringValue( row, fieldName, NullValue.NullString );
            return ReadStringValue(row, fieldName, string.Empty);
        }

        /// <summary>
        /// Читает и возвращает из строки значение String-поля.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string ReadStringValue(DataRow row, string fieldName, string defaultValue)
        {
            return row[fieldName] != DBNull.Value ? (string)row[fieldName] : defaultValue;
        }

        /// <summary>
        /// Читает и возвращает из строки значение Double-поля. Для NULL-значений используется значение NullValue.NullDouble.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static double? ReadDoubleValue(DataRow row, string fieldName)
        {
            return row[fieldName] != DBNull.Value ? Convert.ToDouble(row[fieldName]) : (double?)null;
        }

        /// <summary>
        /// Читает и возвращает из строки значение Double-поля.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ReadDoubleValue(DataRow row, string fieldName, double defaultValue)
        {
            return ReadDoubleValue(row, fieldName).GetValueOrDefault(defaultValue);
        }

        /// <summary>
        /// Читает и возвращает из строки значение Int32-поля. Для NULL-значений используется значение NullValue.NullInt32.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static int? ReadInt32Value(DataRow row, string fieldName)
        {
            return row[fieldName] != DBNull.Value ? Convert.ToInt32(row[fieldName]) : (int?)null;
        }

        /// <summary>
        /// Читает и возвращает из строки значение Int32-поля.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ReadInt32Value(DataRow row, string fieldName, int defaultValue)
        {
            return ReadInt32Value(row, fieldName).GetValueOrDefault(defaultValue);
        }

        /// <summary>
        /// Читает и возвращает из строки значение DateTime-поля. Для NULL-значений используется значение NullValue.NullDateTime.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DateTime? ReadDateTimeValue(DataRow row, string fieldName)
        {
            return row[fieldName] != DBNull.Value ? (DateTime)row[fieldName] : (DateTime?)null;
        }

        /// <summary>
        /// Читает и возвращает из строки значение DateTime-поля.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ReadDateTimeValue(DataRow row, string fieldName, DateTime defaultValue)
        {
            return ReadDateTimeValue(row, fieldName).GetValueOrDefault(defaultValue);
        }

        /// <summary>
        /// Читает и возвращает из строки значение Decimal-поля. Для NULL-значений используется значение 0.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static decimal? ReadDecimalValue(DataRow row, string fieldName)
        {
            return row[fieldName] != DBNull.Value ? (decimal)row[fieldName] : (decimal?)null;
        }

        /// <summary>
        /// Читает и возвращает из строки значение Decimal-поля.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ReadDecimalValue(DataRow row, string fieldName, decimal defaultValue)
        {
            return ReadDecimalValue(row, fieldName).GetValueOrDefault(defaultValue);
        }

        #endregion
    }
}
