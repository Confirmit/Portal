using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BAL.Settings;
using ConfirmIt.PortalLib.DAL;
using Core;
using Core.ORM.Attributes;
using UlterSystems.PortalLib.BusinessObjects;

namespace Migration.Utility.PersonNameType
{
    [DBTable("Users")]
    public class Person<T> : BasePlainObject
    {
        #region Classes
        /// <summary>
        /// Gender of user.
        /// </summary>
        public enum UserSex
        {
            Male = 1,
            Female = 2,
            Unknown = 0
        }
        #endregion

        #region Fields

        #region IIdentity fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_IdentityName = string.Empty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_IsAuthenticated = true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string m_AuthenticationType = string.Empty;
        #endregion

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PersonalSettings m_Settings = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T m_FirstName;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T m_MiddleName;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T m_LastName;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private short _SexID = 1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UserSex _Sex = UserSex.Unknown;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime? _BirthDate = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _PrimaryEMail = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _Project = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _Room = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _PrimaryIP = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _LongServiceEmployees = true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _PersonnelReserve = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _EmployeesUlterSYSMoscow = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _USL_name = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// First name of person.
        /// </summary>
        [DBRead("FirstName")]
        [DBNullable]
        public T FirstName
        {
            [DebuggerStepThrough]
            get { return m_FirstName; }
            [DebuggerStepThrough]
            set { m_FirstName = value; }
        }

        /// <summary>
        /// Middle name of person.
        /// </summary>
        [DBRead("MiddleName")]
        [DBNullable]
        public T MiddleName
        {
            [DebuggerStepThrough]
            get { return m_MiddleName; }
            [DebuggerStepThrough]
            set { m_MiddleName = value; }
        }

        /// <summary>
        /// Surname of person.
        /// </summary>
        [DBRead("LastName")]
        public T LastName
        {
            [DebuggerStepThrough]
            get { return m_LastName; }
            [DebuggerStepThrough]
            set { m_LastName = value; }
        }

        /// <summary>
        /// Full name of person.
        /// </summary>
        public string FullName
        {
            get
            {
                StringBuilder fullName = new StringBuilder();

                fullName.Append(LastName.ToString() + " ");

                if (!string.IsNullOrEmpty(FirstName.ToString()))
                    fullName.Append(FirstName.ToString() + " ");

                if (!string.IsNullOrEmpty(MiddleName.ToString()))
                    fullName.Append(MiddleName.ToString());
                return fullName.ToString();
            }
        }

        //@idm{
        /// <summary>
        /// First and Last name of person.
        /// </summary>
        //public string FirstAndLastNames
        //{
        //    get
        //    {
        //        string name = string.Empty;
        //        if (FirstName.ToString() != string.Empty)
        //            name += FirstName.ToString() + " ";
        //        name += LastName.ToString();
        //        return name;
        //    }
        //}
        //@idm}


        /// <summary>
        /// Domain names of person.
        /// </summary>
        /// <remarks>Use only for stored persons!</remarks>
        public string[] DomainNames
        {
            get
            {
                if (ID == null)
                    return new string[0];

                try
                {
                    IList<PersonAttribute> attrs = PersonAttributes.GetPersonAttributesByKeyword(ID.Value, PersonAttributeTypes.DomainName.ToString());
                    if ((attrs == null) || (attrs.Count == 0))
                        return new string[0];
                    else
                    {
                        List<string> dNames = new List<string>(attrs.Count);
                        foreach (PersonAttribute pa in attrs)
                        {
                            if (!string.IsNullOrEmpty(pa.StringField))
                                dNames.Add(pa.StringField);
                        }
                        return dNames.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    return new string[0];
                }
            }
        }

        /// <summary>
        /// Get Attribute USL_Name
        /// </summary>
        public string USL_name
        {
            get
            {
                if (ID == null)
                    return String.Empty;

                IList<PersonAttribute> attrs = PersonAttributes.GetPersonAttributesByKeyword(ID.Value, PersonAttributeTypes.USL_name.ToString());
                if (attrs != null && attrs.Count > 0)
                {

                    _USL_name = attrs[0].StringField;
                    return _USL_name;
                }
                else return String.Empty;
            }
        }

        /// <summary>
        /// Идентификатор пола.
        /// </summary>
        [DBRead("Sex")]
        public short SexID
        {
            [DebuggerStepThrough]
            get { return _SexID; }
            set
            {
                _SexID = value;
                _Sex = (UserSex)value;
            }
        }

        /// <summary>
        /// Пол пользователя.
        /// </summary>
        public UserSex Sex
        {
            [DebuggerStepThrough]
            get { return _Sex; }
            set
            {
                _Sex = value;
                _SexID = (short)value;
            }
        }

        /// <summary>
        /// День рождения пользователя.
        /// </summary>
        [DBRead("Birthday")]
        [DBNullable]
        public DateTime? Birthday
        {
            [DebuggerStepThrough]
            get { return _BirthDate; }
            [DebuggerStepThrough]
            set { _BirthDate = value; }
        }

        /// <summary>
        /// E-Mail пользователя.
        /// </summary>
        [DBRead("PrimaryEMail")]
        [DBNullable]
        public string PrimaryEMail
        {
            [DebuggerStepThrough]
            get { return _PrimaryEMail; }
            [DebuggerStepThrough]
            set { _PrimaryEMail = value; }
        }

        /// <summary>
        /// Проект пользователя.
        /// </summary>
        [DBRead("Project")]
        [DBNullable]
        public string Project
        {
            [DebuggerStepThrough]
            get { return _Project; }
            [DebuggerStepThrough]
            set { _Project = value; }
        }

        /// <summary>
        /// Комната пользователя.
        /// </summary>
        [DBRead("Room")]
        [DBNullable]
        public string Room
        {
            [DebuggerStepThrough]
            get { return _Room; }
            [DebuggerStepThrough]
            set { _Room = value; }
        }

        /// <summary>
        /// IP пользователя.
        /// </summary>
        [DBRead("PrimaryIP")]
        [DBNullable]
        public string PrimaryIP
        {
            [DebuggerStepThrough]
            get { return _PrimaryIP; }
            [DebuggerStepThrough]
            set { _PrimaryIP = value; }
        }

        /// <summary>
        /// Постоянный рабочий.
        /// </summary>
        [DBRead("LongServiceEmployees")]
        public bool LongServiceEmployees
        {
            [DebuggerStepThrough]
            get { return _LongServiceEmployees; }
            [DebuggerStepThrough]
            set { _LongServiceEmployees = value; }
        }

        /// <summary>
        /// ?.
        /// </summary>
        [DBRead("PersonnelReserve")]
        public bool PersonnelReserve
        {
            [DebuggerStepThrough]
            get { return _PersonnelReserve; }
            [DebuggerStepThrough]
            set { _PersonnelReserve = value; }
        }

        /// <summary>
        /// Работает в Москве.
        /// </summary>
        [DBRead("EmployeesUlterSYSMoscow")]
        public bool EmployeesUlterSYSMoscow
        {
            [DebuggerStepThrough]
            get { return _EmployeesUlterSYSMoscow; }
            [DebuggerStepThrough]
            set { _EmployeesUlterSYSMoscow = value; }
        }

        /// <summary>
        /// Способности пользователя.
        /// </summary>
        public IList<Ability> Abilities
        {
            get { return SiteProvider.Abilities.GetAllUserAbilities(ID.Value); }
        }

        /// <summary>
        /// Groups for which this person is a member.
        /// </summary>
        public IList<Role> Roles
        {
            get
            {
                if (!ID.HasValue)
                    return new Role[0];

                List<Role> userRoles = new List<Role>();
                foreach (Role role in Role.GetUserRoles(ID.Value))
                {
                    userRoles.Add(role);
                }

                return userRoles;//.ToArray();
            }
        }

        /// <summary>
        /// Names of groups for which this person is a member.
        /// </summary>
        public string[] GroupNames
        {
            get
            {
                List<string> roles = new List<string>();
                if (ID.HasValue)
                {
                    foreach (Role role in Role.GetUserRoles(ID.Value))
                    {
                        roles.Add(role.RoleID);
                    }
                }
                return roles.ToArray();
            }
        }

        /// <summary>
        /// Return all events for user.
        /// </summary>
        public IList<UserEvent> Events
        {
            get
            {
                if (!ID.HasValue)
                    return new List<UserEvent>();

                return SiteProvider.Events.GetAllUserEvents(ID.Value);
            }
        }

        #endregion
    }
}
