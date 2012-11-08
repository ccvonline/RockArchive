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
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Cms
{
    /// <summary>
    /// Data Transfer Object for BlockType object
    /// </summary>
    [Serializable]
    [DataContract]
    public partial class BlockTypeDto : IDto
    {
        /// <summary />
        [DataMember]
        public bool IsSystem { get; set; }

        /// <summary />
        [DataMember]
        public string Path { get; set; }

        /// <summary />
        [DataMember]
        public string Name { get; set; }

        /// <summary />
        [DataMember]
        public string Description { get; set; }

        /// <summary />
        [DataMember]
        public int Id { get; set; }

        /// <summary />
        [DataMember]
        public Guid Guid { get; set; }

        /// <summary>
        /// Instantiates a new DTO object
        /// </summary>
        public BlockTypeDto ()
        {
        }

        /// <summary>
        /// Instantiates a new DTO object from the entity
        /// </summary>
        /// <param name="blockType"></param>
        public BlockTypeDto ( BlockType blockType )
        {
            CopyFromModel( blockType );
        }

        /// <summary>
        /// Creates a dictionary object.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add( "IsSystem", this.IsSystem );
            dictionary.Add( "Path", this.Path );
            dictionary.Add( "Name", this.Name );
            dictionary.Add( "Description", this.Description );
            dictionary.Add( "Id", this.Id );
            dictionary.Add( "Guid", this.Guid );
            return dictionary;
        }

        /// <summary>
        /// Creates a dynamic object.
        /// </summary>
        /// <returns></returns>
        public virtual dynamic ToDynamic()
        {
            dynamic expando = new ExpandoObject();
            expando.IsSystem = this.IsSystem;
            expando.Path = this.Path;
            expando.Name = this.Name;
            expando.Description = this.Description;
            expando.Id = this.Id;
            expando.Guid = this.Guid;
            return expando;
        }

        /// <summary>
        /// Copies the model property values to the DTO properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyFromModel( IEntity model )
        {
            if ( model is BlockType )
            {
                var blockType = (BlockType)model;
                this.IsSystem = blockType.IsSystem;
                this.Path = blockType.Path;
                this.Name = blockType.Name;
                this.Description = blockType.Description;
                this.Id = blockType.Id;
                this.Guid = blockType.Guid;
            }
        }

        /// <summary>
        /// Copies the DTO property values to the entity properties
        /// </summary>
        /// <param name="model">The model.</param>
        public void CopyToModel ( IEntity model )
        {
            if ( model is BlockType )
            {
                var blockType = (BlockType)model;
                blockType.IsSystem = this.IsSystem;
                blockType.Path = this.Path;
                blockType.Name = this.Name;
                blockType.Description = this.Description;
                blockType.Id = this.Id;
                blockType.Guid = this.Guid;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class BlockTypeDtoExtension
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static BlockType ToModel( this BlockTypeDto value )
        {
            BlockType result = new BlockType();
            value.CopyToModel( result );
            return result;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<BlockType> ToModel( this List<BlockTypeDto> value )
        {
            List<BlockType> result = new List<BlockType>();
            value.ForEach( a => result.Add( a.ToModel() ) );
            return result;
        }

        /// <summary>
        /// To the dto.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<BlockTypeDto> ToDto( this List<BlockType> value )
        {
            List<BlockTypeDto> result = new List<BlockTypeDto>();
            value.ForEach( a => result.Add( new BlockTypeDto( a ) ) );
            return result;
        }
    }
}