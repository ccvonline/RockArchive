﻿namespace Rock.Data
    
    /// <summary>
    /// Interface to compose ability to Export an object's data
    /// </summary>
    public interface IExportable
        
        /// <summary>
        /// Exports the object as JSON.
        /// </summary>
        /// <returns></returns>
        string ExportJson();


        /// <summary>
        /// Exports the object.
        /// </summary>
        /// <returns></returns>
        object ExportObject();


        /// <summary>
        /// Imports the object from JSON.
        /// </summary>
        /// <param name="data">The data.</param>
        void ImportJson( string data );
    }
}
