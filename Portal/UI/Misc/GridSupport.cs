using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Core;
using EPAMSWeb.UI;

namespace Confirmit.Portal
{
    #region enum EditModes - Режимы редактирования элементов

    /// <summary>
    /// Режимы редактирования элементов.
    /// </summary>
    public enum EditModes
    {
        /// <summary>
        /// В этом режиме на события автоматически создаваемый кнопок поднимаются собственные события Grid'а.
        /// </summary>
        Custom,
        /// <summary>
        /// 
        /// </summary>
        Simple,
        /// <summary>
        /// 
        /// </summary>
        WithMarker,
        /// <summary>
        /// 
        /// </summary>
        WithParentMarker
    }

    #endregion

    #region Классы аргументов событий
    /// <summary>
    /// Класс для аргументов события на удаление.
    /// </summary>
    public class GridDeleteEventArgs : EventArgs
    {
        private DataKeyArray m_DataKeys;

        public GridDeleteEventArgs(GridDeleteEventArgs args)
            : this(args.DataKeys)
        {
        }

        public GridDeleteEventArgs(DataKeyArray keys)
        {
            m_DataKeys = keys;
        }

        /// <summary>
        /// Список ключей и значений, для которых нужно произвести удаление.
        /// </summary>
        public DataKeyArray DataKeys
        {
            get
            {
                return m_DataKeys;
            }
        }
    }

    /// <summary>
    /// Класс для аргументов события на редактирование.
    /// </summary>
    public class GridEditEventArgs : EventArgs
    {
        private string m_DataKeyValue;

        public GridEditEventArgs(GridEditEventArgs args)
            : this(args.DataKeyValue)
        {
        }
        public GridEditEventArgs(string dataKeyValue)
        {
            m_DataKeyValue = dataKeyValue;
        }

        /// <summary>
        /// Значение ключа
        /// </summary>
        public string DataKeyValue
        {
            get
            {
                return m_DataKeyValue;
            }
        }
    }

    /// <summary>
    /// Класс для аргументов события на выделение.
    /// </summary>
    public class GridSelectEventArgs : EventArgs
    {
        private int m_SelectedIndex;
        private bool m_Selected;

        public GridSelectEventArgs(GridSelectEventArgs args)
            : this(args.SelectedIndex, args.Selected)
        {
        }
        public GridSelectEventArgs(int selectedIndex, bool selected)
        {
            m_SelectedIndex = selectedIndex;
            m_Selected = selected;
        }

        /// <summary>
        /// Индекс выделенной строки.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return m_SelectedIndex;
            }
        }

        /// <summary>
        /// True, если строка выделена. Falsе, если строка сброшена.
        /// </summary>
        public bool Selected
        {
            get
            {
                return m_Selected;
            }
        }
    }
    #endregion

    #region Делегаты
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate PagingResult GridRequestDatasourceHandler(object sender, PagingArgs args);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void GridDeleteEventHandler(object sender, GridDeleteEventArgs args);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void GridEditEventHandler(object sender, GridEditEventArgs args);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public delegate Dictionary<string, string> GridAdditionalParametersHandler();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void GridSelectEventHandler(object sender, GridSelectEventArgs args);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public delegate GridSelection GridResolveSelectionEventHandler(object sender);

    #endregion

    #region enum HierarchicalRowState - Состояние вложенного элемента
    /// <summary>
    /// Состояние вложенного элемента.
    /// </summary>
    public enum HierarchicalRowState
    {
        Collapsed,
        Expanded
    }

    #endregion

    #region HierarchicalStateCollection - Коллекция для доступа к состоянию подэлементов иерархии
    /// <summary>
    /// Коллекция для доступа к состоянию подэлементов иерархии.
    /// </summary>
    [Serializable]
    public class HierarchicalRowStateCollection
    {
        private Dictionary<int, HierarchicalRowState> m_States;

        protected Dictionary<int, HierarchicalRowState> States
        {
            get
            {
                if (m_States == null)
                {
                    m_States = new Dictionary<int, HierarchicalRowState>();
                }
                return m_States;
            }
        }

        /// <summary>
        /// Возвращает или устанавливает состояние строки с заданным индексом.
        /// Внимание: при установке нового состояния строки нужно 
        /// обязательно обновлять данные грида (метод RefreshData(false)).
        /// </summary>
        /// <param name="index">Индекс строки (rowIndex).</param>
        /// <returns></returns>
        public HierarchicalRowState this[int index]
        {
            get
            {
                return States.ContainsKey(index) ? States[index] : HierarchicalRowState.Collapsed;
            }
            set
            {
                States[index] = value;
            }
        }

        /// <summary>
        /// Сбрасывает все состояния в "закрытое".
        /// </summary>
        public void Clear()
        {
            States.Clear();
        }
    }

    #endregion
}