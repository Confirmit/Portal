using System;
using System.Configuration;
using System.Diagnostics;

namespace ConfirmIt.PortalLib.Configuration
{
	/// <summary>
	/// Configuration section for basic settings.
	/// </summary>
    public class SettingsSection : ConfigurationElement
    {
        /// <summary>
        /// Is break buttons will enable.
        /// </summary>
        [ConfigurationProperty("isEnableBreakButtons", DefaultValue = "true")]
        public bool IsEnableBreakButtons
        {
            [DebuggerStepThrough]
            get { return (bool)base["isEnableBreakButtons"]; }
            [DebuggerStepThrough]
            set { base["isEnableBreakButtons"] = value; }
        }

        /// <summary>
        /// Is break buttons will enable.
        /// </summary>
        [ConfigurationProperty("bonusWorkMinutes", DefaultValue = "5")]
        public int BonusWorkMinutes
        {
            [DebuggerStepThrough]
            get { return (int)base["bonusWorkMinutes"]; }
            [DebuggerStepThrough]
            set { base["bonusWorkMinutes"] = value; }
        }

        /// <summary>
        /// Is break buttons will enable.
        /// </summary>
      /*  [ConfigurationProperty("uploadFolderPath", DefaultValue = "~/NewsTape/Upload/")]
        public String UploadFolderPath
        {
            [DebuggerStepThrough]
            get { return (String)base["uploadFolderPath"]; }
            [DebuggerStepThrough]
            set { base["uploadFolderPath"] = value; }
        }*/
    }
}

