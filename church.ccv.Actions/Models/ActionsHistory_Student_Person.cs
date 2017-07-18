﻿using Rock.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace church.ccv.Actions.Models
{
        [Table( "_church_ccv_Actions_History_Student_Person" )]
        [DataContract]
        public class ActionsHistory_Student_Person : Model<ActionsHistory_Student_Person>, IRockEntity
        {
            [DataMember]
            public DateTime Date { get; set; }

            [DataMember]
            public int PersonAliasId { get; set; }

            // --- These are the actions that people can perform ---
            [DataMember]
            public DateTime? Baptised { get; set; }

            [DataMember]
            public bool ERA { get; set; }
	    
            [DataMember]
            public bool Give { get; set; }

            [DataMember]
            public DateTime? Member { get; set; }

            [DataMember]
            public DateTime? StartingPoint { get; set; }

            [DataMember]
            public bool Serving { get; set; }

            [DataMember]
            public bool SharedStory { get; set; }

            // CCV Considers this "Connected". But if / when they change it,
            // the concept will remain the same. Learning from peers.
            [DataMember]
            public bool PeerLearning { get; set; }
            
            // CCV Considers this "Coached". This would be someone in a group where
            // someone is helping them grow, like a NextStep Group
            [DataMember]
            public bool Mentored { get; set; }

            // CCV Considers this "Coaching". But if / when they change it,
            // the concept will remaing the same. They're teaching people. (not necessarily peers)
            [DataMember]
            public bool Teaching { get; set; }
            // ---

            // --- Breakdowns for actions that depend on group types ---
            // It tells us how many people are in each type of group per action.

            // Peer Learning group types
            [DataMember]
            public bool PeerLearning_NG { get; set; }

            // Mentored Group Types
            [DataMember]
            public bool Mentored_NG { get; set; }

            // Teaching Group Types
            [DataMember]
            public bool Teaching_Undefined { get; set; }
            // ---
        }
}
