﻿using System;

namespace Rock.Data
{
    public abstract class DTO<T> 
        where T : Model<T>
    {
        /// <summary>
        /// The Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the Created Date Time.
        /// </summary>
        /// <value>
        /// Created Date Time.
        /// </value>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Modified Date Time.
        /// </summary>
        /// <value>
        /// Modified Date Time.
        /// </value>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Created By Person Id.
        /// </summary>
        /// <value>
        /// Created By Person Id.
        /// </value>
        public int? CreatedByPersonId { get; set; }

        /// <summary>
        /// Gets or sets the Modified By Person Id.
        /// </summary>
        /// <value>
        /// Modified By Person Id.
        /// </value>
        public int? ModifiedByPersonId { get; set; }

        /// <summary>
        /// Initializes a new instance of the data transformation object.
        /// </summary>
        public DTO()
        {
        }

        /// <summary>
        /// Initializes a new instance of the data transformation object from a model
        /// </summary>
        /// <param name="model"></param>
        public DTO(T model)
        {
        }

        /// <summary>
        /// Copies properties to the model
        /// </summary>
        /// <param name="dto"></param>
        public abstract void CopyFromModel(T model);

        /// <summary>
        /// Copies properties from the model
        /// </summary>
        /// <param name="model"></param>
        public abstract void CopyToModel(T model);

    }
}