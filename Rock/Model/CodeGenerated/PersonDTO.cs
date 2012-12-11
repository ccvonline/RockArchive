//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Data Transfer Object for Person object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class PersonDto : DtoSecured<PersonDto>
    {
        /// <summary />
        [DataMember]
        public bool IsSystem { get; set; }

        /// <summary />
        [DataMember]
        public int? RecordTypeValueId { get; set; }

        /// <summary />
        [DataMember]
        public int? RecordStatusValueId { get; set; }

        /// <summary />
        [DataMember]
        public int? RecordStatusReasonValueId { get; set; }

        /// <summary />
        [DataMember]
        public int? PersonStatusValueId { get; set; }

        /// <summary />
        [DataMember]
        public int? TitleValueId { get; set; }

        /// <summary />
        [DataMember]
        public string GivenName { get; set; }

        /// <summary />
        [DataMember]
        public string NickName { get; set; }

        /// <summary />
        [DataMember]
        public string LastName { get; set; }

        /// <summary />
        [DataMember]
        public int? SuffixValueId { get; set; }

        /// <summary />
        [DataMember]
        public int? PhotoId { get; set; }

        /// <summary />
        [DataMember]
        public int? BirthDay { get; set; }

        /// <summary />
        [DataMember]
        public int? BirthMonth { get; set; }

        /// <summary />
        [DataMember]
        public int? BirthYear { get; set; }

        /// <summary />
        [DataMember]
        public Gender Gender { get; set; }

        /// <summary />
        [DataMember]
        public int? MaritalStatusValueId { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? AnniversaryDate { get; set; }

        /// <summary />
        [DataMember]
        public DateTime? GraduationDate { get; set; }

        /// <summary />
        [DataMember]
        public string Email { get; set; }

        /// <summary />
        [DataMember]
        public bool? IsEmailActive { get; set; }

        /// <summary />
        [DataMember]
        public string EmailNote { get; set; }

        /// <summary />
        [DataMember]
        public bool DoNotEmail { get; set; }

        /// <summary />
        [DataMember]
        public string SystemNote { get; set; }

        /// <summary />
        [DataMember]
        public int? ViewedCount { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public PersonDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="person"></param>
        public PersonDto ( Person person )
        {
            CopyFromModel( person );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> ToDictionary()
        {
            var dictionary = base.ToDictionary();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "RecordTypeValueId", this.RecordTypeValueId );
            dictionary.Add( "RecordStatusValueId", this.RecordStatusValueId );
            dictionary.Add( "RecordStatusReasonValueId", this.RecordStatusReasonValueId );
            dictionary.Add( "PersonStatusValueId", this.PersonStatusValueId );
            dictionary.Add( "TitleValueId", this.TitleValueId );
            dictionary.Add( "GivenName", this.GivenName );
            dictionary.Add( "NickName", this.NickName );
            dictionary.Add( "LastName", this.LastName );
            dictionary.Add( "SuffixValueId", this.SuffixValueId );
            dictionary.Add( "PhotoId", this.PhotoId );
            dictionary.Add( "BirthDay", this.BirthDay );
            dictionary.Add( "BirthMonth", this.BirthMonth );
            dictionary.Add( "BirthYear", this.BirthYear );
            dictionary.Add( "Gender", this.Gender );
            dictionary.Add( "MaritalStatusValueId", this.MaritalStatusValueId );
            dictionary.Add( "AnniversaryDate", this.AnniversaryDate );
            dictionary.Add( "GraduationDate", this.GraduationDate );
            dictionary.Add( "Email", this.Email );
            dictionary.Add( "IsEmailActive", this.IsEmailActive );
            dictionary.Add( "EmailNote", this.EmailNote );
            dictionary.Add( "DoNotEmail", this.DoNotEmail );
            dictionary.Add( "SystemNote", this.SystemNote );
            dictionary.Add( "ViewedCount", this.ViewedCount );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public override dynamic ToDynamic()
        {
            dynamic expando = base.ToDynamic();
            expando.IsSystem = this.IsSystem;
            expando.RecordTypeValueId = this.RecordTypeValueId;
            expando.RecordStatusValueId = this.RecordStatusValueId;
            expando.RecordStatusReasonValueId = this.RecordStatusReasonValueId;
            expando.PersonStatusValueId = this.PersonStatusValueId;
            expando.TitleValueId = this.TitleValueId;
            expando.GivenName = this.GivenName;
            expando.NickName = this.NickName;
            expando.LastName = this.LastName;
            expando.SuffixValueId = this.SuffixValueId;
            expando.PhotoId = this.PhotoId;
            expando.BirthDay = this.BirthDay;
            expando.BirthMonth = this.BirthMonth;
            expando.BirthYear = this.BirthYear;
            expando.Gender = this.Gender;
            expando.MaritalStatusValueId = this.MaritalStatusValueId;
            expando.AnniversaryDate = this.AnniversaryDate;
            expando.GraduationDate = this.GraduationDate;
            expando.Email = this.Email;
            expando.IsEmailActive = this.IsEmailActive;
            expando.EmailNote = this.EmailNote;
            expando.DoNotEmail = this.DoNotEmail;
            expando.SystemNote = this.SystemNote;
            expando.ViewedCount = this.ViewedCount;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyFromModel( IEntity model )
        {
            base.CopyFromModel( model );

            if ( model is Person )
            {
                var person = (Person)model;
                this.IsSystem = person.IsSystem;
                this.RecordTypeValueId = person.RecordTypeValueId;
                this.RecordStatusValueId = person.RecordStatusValueId;
                this.RecordStatusReasonValueId = person.RecordStatusReasonValueId;
                this.PersonStatusValueId = person.PersonStatusValueId;
                this.TitleValueId = person.TitleValueId;
                this.GivenName = person.GivenName;
                this.NickName = person.NickName;
                this.LastName = person.LastName;
                this.SuffixValueId = person.SuffixValueId;
                this.PhotoId = person.PhotoId;
                this.BirthDay = person.BirthDay;
                this.BirthMonth = person.BirthMonth;
                this.BirthYear = person.BirthYear;
                this.Gender = person.Gender;
                this.MaritalStatusValueId = person.MaritalStatusValueId;
                this.AnniversaryDate = person.AnniversaryDate;
                this.GraduationDate = person.GraduationDate;
                this.Email = person.Email;
                this.IsEmailActive = person.IsEmailActive;
                this.EmailNote = person.EmailNote;
                this.DoNotEmail = person.DoNotEmail;
                this.SystemNote = person.SystemNote;
                this.ViewedCount = person.ViewedCount;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public override void CopyToModel ( IEntity model )
        {
            base.CopyToModel( model );

            if ( model is Person )
            {
                var person = (Person)model;
                person.IsSystem = this.IsSystem;
                person.RecordTypeValueId = this.RecordTypeValueId;
                person.RecordStatusValueId = this.RecordStatusValueId;
                person.RecordStatusReasonValueId = this.RecordStatusReasonValueId;
                person.PersonStatusValueId = this.PersonStatusValueId;
                person.TitleValueId = this.TitleValueId;
                person.GivenName = this.GivenName;
                person.NickName = this.NickName;
                person.LastName = this.LastName;
                person.SuffixValueId = this.SuffixValueId;
                person.PhotoId = this.PhotoId;
                person.BirthDay = this.BirthDay;
                person.BirthMonth = this.BirthMonth;
                person.BirthYear = this.BirthYear;
                person.Gender = this.Gender;
                person.MaritalStatusValueId = this.MaritalStatusValueId;
                person.AnniversaryDate = this.AnniversaryDate;
                person.GraduationDate = this.GraduationDate;
                person.Email = this.Email;
                person.IsEmailActive = this.IsEmailActive;
                person.EmailNote = this.EmailNote;
                person.DoNotEmail = this.DoNotEmail;
                person.SystemNote = this.SystemNote;
                person.ViewedCount = this.ViewedCount;
            }
        }

    }


    /// <summary>
    /// Person Extension Methods
    /// </summary>
    public static class PersonExtensions
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Person ToModel( this PersonDto value )
        {
            Person result = new Person();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<Person> ToModel( this List<PersonDto> value )
        {
            List<Person> result = new List<Person>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<PersonDto> ToDto( this List<Person> value )
        {
            List<PersonDto> result = new List<PersonDto>();
            value.ForEach( a => result.Add( a.ToDto() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static PersonDto ToDto( this Person value )
        {
            return new PersonDto( value );
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static string ToJson( this Person value, bool deep = false )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject( ToDynamic( value, deep ) );
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic( this ICollection<Person> values )
        {
            var dynamicList = new List<dynamic>();
            foreach ( var value in values )
            {
                dynamicList.Add( value.ToDynamic( true ) );
            }
            return dynamicList;
        }

        /// <summary>
        /// To the dynamic.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        public static dynamic ToDynamic( this Person value, bool deep = false )
        {
            dynamic dynamicPerson = new PersonDto( value ).ToDynamic();

            if ( !deep )
            {
                return dynamicPerson;
            }


            if (value.Users != null)
            {
                dynamicPerson.Users = value.Users.ToDynamic();
            }

            if (value.EmailTemplates != null)
            {
                dynamicPerson.EmailTemplates = value.EmailTemplates.ToDynamic();
            }

            if (value.PhoneNumbers != null)
            {
                dynamicPerson.PhoneNumbers = value.PhoneNumbers.ToDynamic();
            }

            if (value.Members != null)
            {
                dynamicPerson.Members = value.Members.ToDynamic();
            }

            if (value.Pledges != null)
            {
                dynamicPerson.Pledges = value.Pledges.ToDynamic();
            }

            if (value.PersonAccountLookups != null)
            {
                dynamicPerson.PersonAccountLookups = value.PersonAccountLookups.ToDynamic();
            }

            if (value.MaritalStatusValue != null)
            {
                dynamicPerson.MaritalStatusValue = value.MaritalStatusValue.ToDynamic();
            }

            if (value.PersonStatusValue != null)
            {
                dynamicPerson.PersonStatusValue = value.PersonStatusValue.ToDynamic();
            }

            if (value.RecordStatusValue != null)
            {
                dynamicPerson.RecordStatusValue = value.RecordStatusValue.ToDynamic();
            }

            if (value.RecordStatusReasonValue != null)
            {
                dynamicPerson.RecordStatusReasonValue = value.RecordStatusReasonValue.ToDynamic();
            }

            if (value.RecordTypeValue != null)
            {
                dynamicPerson.RecordTypeValue = value.RecordTypeValue.ToDynamic();
            }

            if (value.SuffixValue != null)
            {
                dynamicPerson.SuffixValue = value.SuffixValue.ToDynamic();
            }

            if (value.TitleValue != null)
            {
                dynamicPerson.TitleValue = value.TitleValue.ToDynamic();
            }

            if (value.Photo != null)
            {
                dynamicPerson.Photo = value.Photo.ToDynamic();
            }

            if (value.ImpersonatedUser != null)
            {
                dynamicPerson.ImpersonatedUser = value.ImpersonatedUser.ToDynamic();
            }

            return dynamicPerson;
        }

        /// <summary>
        /// Froms the json.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="json">The json.</param>
        public static void FromJson( this Person value, string json )
        {
            //Newtonsoft.Json.JsonConvert.PopulateObject( json, value );
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject( json, typeof( ExpandoObject ) );
            value.FromDynamic( obj, true );
        }

        /// <summary>
        /// Froms the dynamic.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        public static void FromDynamic( this Person value, object obj, bool deep = false )
        {
            new PageDto().FromDynamic(obj).CopyToModel(value);

            if (deep)
            {
                var expando = obj as ExpandoObject;
                if (obj != null)
                {
                    var dict = obj as IDictionary<string, object>;
                    if (dict != null)
                    {

                        // Users
                        if (dict.ContainsKey("Users"))
                        {
                            var UsersList = dict["Users"] as List<object>;
                            if (UsersList != null)
                            {
                                value.Users = new List<UserLogin>();
                                foreach(object childObj in UsersList)
                                {
                                    var UserLogin = new UserLogin();
                                    UserLogin.FromDynamic(childObj, true);
                                    value.Users.Add(UserLogin);
                                }
                            }
                        }

                        // EmailTemplates
                        if (dict.ContainsKey("EmailTemplates"))
                        {
                            var EmailTemplatesList = dict["EmailTemplates"] as List<object>;
                            if (EmailTemplatesList != null)
                            {
                                value.EmailTemplates = new List<EmailTemplate>();
                                foreach(object childObj in EmailTemplatesList)
                                {
                                    var EmailTemplate = new EmailTemplate();
                                    EmailTemplate.FromDynamic(childObj, true);
                                    value.EmailTemplates.Add(EmailTemplate);
                                }
                            }
                        }

                        // PhoneNumbers
                        if (dict.ContainsKey("PhoneNumbers"))
                        {
                            var PhoneNumbersList = dict["PhoneNumbers"] as List<object>;
                            if (PhoneNumbersList != null)
                            {
                                value.PhoneNumbers = new List<PhoneNumber>();
                                foreach(object childObj in PhoneNumbersList)
                                {
                                    var PhoneNumber = new PhoneNumber();
                                    PhoneNumber.FromDynamic(childObj, true);
                                    value.PhoneNumbers.Add(PhoneNumber);
                                }
                            }
                        }

                        // Members
                        if (dict.ContainsKey("Members"))
                        {
                            var MembersList = dict["Members"] as List<object>;
                            if (MembersList != null)
                            {
                                value.Members = new List<GroupMember>();
                                foreach(object childObj in MembersList)
                                {
                                    var GroupMember = new GroupMember();
                                    GroupMember.FromDynamic(childObj, true);
                                    value.Members.Add(GroupMember);
                                }
                            }
                        }

                        // Pledges
                        if (dict.ContainsKey("Pledges"))
                        {
                            var PledgesList = dict["Pledges"] as List<object>;
                            if (PledgesList != null)
                            {
                                value.Pledges = new List<Pledge>();
                                foreach(object childObj in PledgesList)
                                {
                                    var Pledge = new Pledge();
                                    Pledge.FromDynamic(childObj, true);
                                    value.Pledges.Add(Pledge);
                                }
                            }
                        }

                        // PersonAccountLookups
                        if (dict.ContainsKey("PersonAccountLookups"))
                        {
                            var PersonAccountLookupsList = dict["PersonAccountLookups"] as List<object>;
                            if (PersonAccountLookupsList != null)
                            {
                                value.PersonAccountLookups = new List<PersonAccount>();
                                foreach(object childObj in PersonAccountLookupsList)
                                {
                                    var PersonAccount = new PersonAccount();
                                    PersonAccount.FromDynamic(childObj, true);
                                    value.PersonAccountLookups.Add(PersonAccount);
                                }
                            }
                        }

                        // MaritalStatusValue
                        if (dict.ContainsKey("MaritalStatusValue"))
                        {
                            value.MaritalStatusValue = new DefinedValue();
                            new DefinedValueDto().FromDynamic( dict["MaritalStatusValue"] ).CopyToModel(value.MaritalStatusValue);
                        }

                        // PersonStatusValue
                        if (dict.ContainsKey("PersonStatusValue"))
                        {
                            value.PersonStatusValue = new DefinedValue();
                            new DefinedValueDto().FromDynamic( dict["PersonStatusValue"] ).CopyToModel(value.PersonStatusValue);
                        }

                        // RecordStatusValue
                        if (dict.ContainsKey("RecordStatusValue"))
                        {
                            value.RecordStatusValue = new DefinedValue();
                            new DefinedValueDto().FromDynamic( dict["RecordStatusValue"] ).CopyToModel(value.RecordStatusValue);
                        }

                        // RecordStatusReasonValue
                        if (dict.ContainsKey("RecordStatusReasonValue"))
                        {
                            value.RecordStatusReasonValue = new DefinedValue();
                            new DefinedValueDto().FromDynamic( dict["RecordStatusReasonValue"] ).CopyToModel(value.RecordStatusReasonValue);
                        }

                        // RecordTypeValue
                        if (dict.ContainsKey("RecordTypeValue"))
                        {
                            value.RecordTypeValue = new DefinedValue();
                            new DefinedValueDto().FromDynamic( dict["RecordTypeValue"] ).CopyToModel(value.RecordTypeValue);
                        }

                        // SuffixValue
                        if (dict.ContainsKey("SuffixValue"))
                        {
                            value.SuffixValue = new DefinedValue();
                            new DefinedValueDto().FromDynamic( dict["SuffixValue"] ).CopyToModel(value.SuffixValue);
                        }

                        // TitleValue
                        if (dict.ContainsKey("TitleValue"))
                        {
                            value.TitleValue = new DefinedValue();
                            new DefinedValueDto().FromDynamic( dict["TitleValue"] ).CopyToModel(value.TitleValue);
                        }

                        // Photo
                        if (dict.ContainsKey("Photo"))
                        {
                            value.Photo = new BinaryFile();
                            new BinaryFileDto().FromDynamic( dict["Photo"] ).CopyToModel(value.Photo);
                        }

                    }
                }
            }
        }

    }
}