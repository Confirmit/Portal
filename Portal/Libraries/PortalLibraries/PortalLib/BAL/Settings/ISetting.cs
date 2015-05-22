using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfirmIt.PortalLib.BAL.Settings
{
    /// <summary>
    /// Интерфейс любого класса настроек
    /// </summary>
    public interface ISetting
    {
        /// <summary>
        /// Имя столбца с названиями настроек
        /// </summary>
        string KeyColumnName { get; }

        /// <summary>
        /// Атрибут настройки
        /// </summary>
        SettingAttribute SettingAttribute { get; }

        /// <summary>
        /// Значение настройки
        /// </summary>
        object Value { get; set; }
    }
}
