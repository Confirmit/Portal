using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Core.Dictionaries.ExportImport.Serialization;
using Core.Exceptions;
using Core.Resources;

namespace Core.Dictionaries.ExportImport.Packing
{
	public abstract class PackerWithSerializationBase : IPacker
	{
		#region Конструкторы

        public PackerWithSerializationBase(params ISerializationMethod[] serializationMethods)
        {
            if (serializationMethods == null || serializationMethods.Length == 0)
                throw new CoreArgumentException(ResourceManager.GetString("SerializationException"));

            m_serializationMethods = serializationMethods;
        }
		
		#endregion
		
		#region Свойства
		
		private ISerializationMethod[] m_serializationMethods;
		protected ISerializationMethod[] SerializationMethods
		{
			get { return m_serializationMethods; }
		}

		private Stream m_outputStream = null;
		protected Stream OutputStream
		{
            get
            {
                if (m_outputStream == null)
                    throw new CoreInvalidOperationException(Resources.ResourceManager.GetString("OutputStreamException"));

                return m_outputStream;
            }
		}

		#endregion

		#region IPacker Members

        public virtual void StartBatch(Stream outputStream)
        {
            if (outputStream == null)
                throw new CoreArgumentException(Resources.ResourceManager.GetString("OutputStreamException"));

            m_outputStream = outputStream;
        }

		public abstract void AddToBatch( DataTable data );
		
		public abstract void FinishBatch();

		public abstract IEnumerable<DataTable> UnPack( Stream stream );

		#endregion
	}
}
