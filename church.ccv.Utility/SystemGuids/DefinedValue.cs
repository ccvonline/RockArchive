﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace church.ccv.Utility.SystemGuids
{
    public static class DefinedValue
    {
        /// <summary>
        /// Coach wants to follow up with the group member at a later date
        /// </summary>
        public const string NEXT_STEPS_OPT_OUT_FOLLOW_UP_LATER = "A6A94945-D7B5-4EF7-9284-D42C4E1AB2D3";

        /// <summary>
        /// Member does not want to be contacted anymore
        /// </summary>
        public const string NEXT_STEPS_OPT_OUT_DO_NOT_CONTACT = "EFDF942F-66D2-4066-A560-59BD6753FB2E";

        /// <summary>
        /// Coach needs this person reassigned for some reason
        /// </summary>
        public const string NEXT_STEPS_OPT_OUT_REASSIGN_TO_NEW_COACH = "1B005C59-A2A8-4DF8-A2F2-4EE6BE34BCB3";

        /// <summary>
        /// Coach can't get ahold of the member
        /// </summary>
        public const string NEXT_STEPS_OPT_OUT_UNABLE_TO_REACH = "8A675978-A443-48AE-90FE-6B860A4943FC";

        /// <summary>
        /// Member wants to be in a neighborhood group (and therefore shouldn't be in a NS group)
        /// </summary>
        public const string NEXT_STEPS_OPT_OUT_REGISTERED_FOR_NEIGHBORHOOD_GROUP = "929D0921-3FDB-4AE3-82BE-0EE322276BF4";
        
        /// <summary>
        /// Member is no longet attending CCV
        /// </summary>
        public const string NEXT_STEPS_OPT_OUT_NO_LONGER_ATTENDING_CCV = "BA6C1F11-9020-4C15-BC2B-9E12FDF84168";




        /// <summary>
        /// Coach wants to follow up with the group member at a later date
        /// </summary>
        public const string NEIGHBORHOOD_OPT_OUT_FOLLOW_UP_LATER = "2B23AE6C-0B1C-4CB3-AE6B-502420DEE9EC";

        /// <summary>
        /// Member is no longer attending group
        /// </summary>
        public const string NEIGHBORHOOD_OPT_OUT_NOT_ATTENDING_GROUP = "D6FCA7CF-D178-4686-AFB1-594D70EFFF6A";
        
        /// <summary>
        /// Member needs a next step coach
        /// </summary>
        public const string NEIGHBORHOOD_OPT_OUT_NEEDS_NEXT_STEPS_COACH = "7AACEE8D-D764-4743-A43F-D152F0075A99";

        /// <summary>
        /// Member no longer attends CCV
        /// </summary>
        public const string NEIGHBORHOOD_OPT_OUT_NO_LONGER_ATTENDING_CCV = "7B58B1FC-1B52-409E-BFFE-1B8D2D0AA85E";

        
        
        /// <summary>
        /// Coach wants to follow up with the group member at a later date
        /// </summary>
        public const string NEXT_GEN_OPT_OUT_FOLLOW_UP_LATER = "F49B30D0-33FF-4ECF-A3D7-DE24F1E3CA68";
        
        /// <summary>
        /// Coach needs this person reassigned for some reason
        /// </summary>
        public const string NEXT_GEN_OPT_OUT_REASSIGN_TO_NEW_COACH = "C78D0095-343C-4F0E-A05B-6F7BCC7AEF8B";
                
        /// <summary>
        /// Member is no longer attending group
        /// </summary>
        public const string NEXT_GEN_OPT_OUT_NOT_ATTENDING_GROUP = "7F83CC51-2428-4543-8005-C390EEDF4ABC";

        /// <summary>
        /// Member no longer attends CCV
        /// </summary>
        public const string NEXT_GEN_OPT_OUT_NO_LONGER_ATTENDING_CCV = "4C50A71B-ABB3-4FD9-8291-46036BA1559E";

        /// <summary>
        /// Member changed schools
        /// </summary>
        public const string NEXT_GEN_OPT_OUT_MOVED_SCHOOLS  = "E62445C5-82AA-4FDE-8035-9D8C6EAE1EC2";
    }
}
