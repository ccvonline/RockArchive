﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using Rock.Model;

namespace Rock.CheckIn
{
    /// <summary>
    /// The status of a check-in kiosk.  
    /// </summary>
    [DataContract]
    public class KioskStatus : DeviceDto
    {
        /// <summary>
        /// The group types associated with this kiosk
        /// </summary>
        /// <value>
        /// The group types.
        /// </value>
        [DataMember]
        public List<KioskGroupType> GroupTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KioskStatus" /> class.
        /// </summary>
	    public KioskStatus() : base()
	    {
            GroupTypes = new List<KioskGroupType>();
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="KioskStatus" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public KioskStatus( Device device )
            : base( device )
        {
            GroupTypes = new List<KioskGroupType>();
        }
    }
}

