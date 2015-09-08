using System;
using System.Collections.Generic;

namespace Microsoft.UnityPlugins
{
    /// <summary>
    /// Provides a strongly-typed representation of a Facebook Location as defined by the Graph API.
    /// </summary>
    /// <remarks>
    /// The FBLocation class represents the most commonly used properties of a Facebook Location object.
    /// </remarks>
    public class FBLocation
    {
        /// <summary>
        /// Initializes a new instance of the FBLocation class.
        /// </summary>
        public FBLocation()
        {
        }

        /// <summary>
        /// Gets or sets the street component of the location.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the city component of the location.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state component of the location.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country component of the location.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the postal code component of the location.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the latitude component of the location.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of the location.
        /// </summary>
        public double Longitude { get; set; }
    }
}
